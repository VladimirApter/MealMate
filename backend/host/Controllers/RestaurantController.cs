using Microsoft.AspNetCore.Mvc;
using host.DataBaseAccess;
using host.Models;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetRestaurant(int id)
    {
        var restaurant = RestaurantAccess.GetRestaurant(id);
        if (restaurant == null) return NotFound();

        return Ok(restaurant);
    }

    [HttpPost]
    public IActionResult PostRestaurant([FromBody] Restaurant? restaurant)
    {
        if (restaurant == null) return BadRequest("Restaurant is null.");
        RestaurantAccess.AddOrUpdateRestaurant(restaurant);

        return Ok(restaurant.Id);
    }
}