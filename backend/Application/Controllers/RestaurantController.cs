using System.Text.Json;
using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Logic;
using Microsoft.EntityFrameworkCore;

namespace site.Controllers;

[ApiController]
[Route("order")]
public class RestaurantController : Controller
{
    [HttpGet("{token}")]
    public IActionResult GetRestaurant(string token)
    {
        var (TableId, RestaurantId) = TokenEncryptor.DecryptToken(token);
        var apiRestaurantClient = new ApiClient<Restaurant>();
        var apiTableClient = new ApiClient<Table>();
        var restaurant = apiRestaurantClient.Get(RestaurantId);
        var table = apiTableClient.Get(TableId);

        if (restaurant == null || table == null)
            return NotFound();
        
        var restaurantDetails = new RestaurantDetailsViewModel()
        {
            Restaurant = restaurant,
            Table = table
        };

        return View("RestaurantDetails", restaurantDetails);
    }
    
    
    [HttpPost("/")]
    public IActionResult PlaceOrder([FromBody] Order order)
    {
        Console.WriteLine($"Заказ {order.Id} получен");
        var test = new { url = $"iPN9O0wbwKSg8z42aNdkuQ/{order.Id}" };
        Console.WriteLine(test);
        return Ok(test);
    }
    
    [HttpGet("{token}/{orderId}")]
    public IActionResult OrderDetails(int orderId)
    {
        try
        {
            using (var context = new ApplicationDbContext())
            {
                var order = context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                    .FirstOrDefault(o => o.Id == orderId && o.TableId == 1);

                if (order == null)
                {
                    return NotFound();
                }

                return View("OrderDetails", order);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Ошибка при получении деталей заказа");
        }
    }
}