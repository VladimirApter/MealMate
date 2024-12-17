using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDish(long id)
    {
        var o = await DataBaseAccess<Dish>.GetAsync(id);
        return o == null ? NotFound("Dish is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostDish([FromBody] Dish? dish)
    {
        if (dish == null) return NotFound("Dish is null");
        
        DataBaseAccess<Dish>.AddOrUpdate(dish);
        return Ok(dish.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteDish(long id)
    {
        var dish = await DataBaseAccess<Dish>.GetAsync(id);
        if (dish == null) return NotFound("Dish is null");

        DataBaseAccess<Dish>.Delete(id);
        return Ok();
    }
}