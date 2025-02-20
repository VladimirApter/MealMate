using System.Text.Json;
using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Logic;
using Microsoft.EntityFrameworkCore;

namespace site.Controllers;

public static class Orders 
{
    public static readonly Dictionary<long?, Order> OrdersDictionary = new Dictionary<long?, Order>();
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
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(clientIp))
            clientIp = "no ip";
        var client = new Client(order.ClientId, clientIp);
        order.Client = client;

        var apiClientDish = new ApiClient<Dish>();
        var apiClientDrink = new ApiClient<Drink>();
        foreach (var orderItem in order.OrderItems)
        {
            var objDish = apiClientDish.Get(orderItem.MenuItemId.Value);
            var objDrink = apiClientDrink.Get(orderItem.MenuItemId.Value);

            if (objDish == null)
                orderItem.MenuItem = objDrink;
            else
                orderItem.MenuItem = objDish;
        }
        
        Console.WriteLine($"Заказ {order.Id} получен");
        Orders.OrdersDictionary[order.Id] = order;
        var apiClient = new ApiClient<Order>();
        apiClient.Post(order);
        return Ok(new { url = order.Id.ToString() });
    }

    
    [HttpGet("{token}/{orderId}")]
    public IActionResult OrderDetails(long orderId)
    {
        var order = Orders.OrdersDictionary[orderId];
        var apiClient = new ApiClient<Order>();
        var obj = apiClient.Get(orderId);
        order.Status = obj.Status;
        return View("OrderDetails", order);
    }

    [HttpPost("/waitercall")]
    public IActionResult WaiterCall([FromBody] WaiterCall waiterCall)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(clientIp))
            clientIp = "no ip";
        var client = new Client(waiterCall.ClientId, clientIp);
        waiterCall.Client = client;
        
        var apiClient = new ApiClient<WaiterCall>();
        apiClient.Post(waiterCall);
        return Ok();
    }
}
