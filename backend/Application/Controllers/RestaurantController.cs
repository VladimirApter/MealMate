using System.Text.Json;
using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Logic;

namespace site.Controllers;

[ApiController]
[Route("restaurant")]
public class RestaurantController : Controller
{
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetRestaurant(int id)
    // {
    //     var apiClient = new ApiClient<Restaurant>();
    //     var restaurant = apiClient.Get(id);
    //     if (restaurant == null)
    //         return NotFound();

    //     // Загрузка связанных данных
    //     await restaurant.TakeRelatedData(new ApplicationDbContext());

    //     return View("RestaurantDetails", restaurant);
    // }

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
    
    
    // [HttpPost("placeOrder")]
    // public IActionResult PlaceOrder([FromBody] Order order)
    // {
    //     Console.WriteLine("Received order:");

    //     foreach (var item in order.OrderItems)
    //     {
    //         Console.WriteLine($"Dish ID: {item.Id}, Count: {item.Count}, Price: {item.Price}");
    //     }

    //     // Возвращаем ID заказа (в данном случае, это просто пример)
    //     return Ok(new { id = order.Id });
    //}
}