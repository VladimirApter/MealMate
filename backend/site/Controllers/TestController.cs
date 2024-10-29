using host.DataBaseAccess;
using host.Models;
using Microsoft.AspNetCore.Mvc;

namespace site.Controllers;
    

[ApiController]
[Route("restaurant")]
public class TestController : Controller
{
    [HttpGet("{id}")]
    public IActionResult GetRestaurant(int id)
    {
        var apiClient = new ApiClient<Restaurant>();
        var restaurant = apiClient.Get(id);
        if (restaurant == null)
            return NotFound();
        

        return View("RestaurantDetails", restaurant);
    }
}