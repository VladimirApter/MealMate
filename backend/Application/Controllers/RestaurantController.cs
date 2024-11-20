using System.Text.Json;
using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;


namespace site.Controllers;

[ApiController]
[Route("restaurant")]
public class RestaurantController : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRestaurant(int id)
    {
        var apiClient = new ApiClient<Restaurant>();
        var restaurant = apiClient.Get(id);
        if (restaurant == null)
            return NotFound();

        // Загрузка связанных данных
        await restaurant.TakeRelatedData(new ApplicationDbContext());

        return View("RestaurantDetails", restaurant);
    }
    
    [HttpPost("placeOrder")]
    public IActionResult PlaceOrder([FromBody] Order order)
    {
        Console.WriteLine("Received order:");

        foreach (var item in order.OrderItems)
        {
            Console.WriteLine($"Dish ID: {item.Id}, Count: {item.Count}, Price: {item.Price}");
        }

        // Возвращаем ID заказа (в данном случае, это просто пример)
        return Ok(new { id = order.Id });
    }
}