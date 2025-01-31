using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRestaurant(long id)
    {
        var o = await DataBaseAccess<Restaurant>.GetAsync(id);
        return o == null ? NotFound("Restaurant is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostRestaurant([FromBody] Restaurant? restaurant)
    {
        if (restaurant == null) return NotFound("Restaurant is null");
        
        DataBaseAccess<Restaurant>.AddOrUpdate(restaurant);
        return Ok(restaurant.Id);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant(long id)
    {
        var restaurant = await DataBaseAccess<Restaurant>.GetAsync(id);
        if (restaurant == null) return NotFound("Restaurant is null");

        DataBaseAccess<Restaurant>.Delete(id);
        return Ok();
    }
}