using System.Text.Json;
using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Logic;
using Microsoft.EntityFrameworkCore;

namespace site.Controllers;

public static class Orders 
{
    public static readonly Dictionary<int?, Order> OrdersDictionary = new Dictionary<int?, Order>();
}

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
        Orders.OrdersDictionary[order.Id] = order;
        return Ok(new { url = order.Id.ToString() });
    }
    
    [HttpGet("{token}/{orderId}")]
    public IActionResult OrderDetails(int orderId)
    {
        var order = Orders.OrdersDictionary[orderId];
        return View("OrderDetails", order);
    }
}