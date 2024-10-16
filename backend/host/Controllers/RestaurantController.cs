using Microsoft.AspNetCore.Mvc;
using host.DataBaseAccess;

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
}