using host.DataBaseAccess;
using host.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishController : ControllerBase
{

    [HttpGet("{id}")]
    public IActionResult GetDish(int id)
    {
        var dish = DishesAccess.GetDish(id);
        if (dish == null) return NotFound();
        
        return Ok(dish);
    }
    
    [HttpPost]
    public IActionResult CreateDish([FromBody] Dish? dish)
    {
        if (dish == null) return BadRequest("Dish is null.");
        DishesAccess.AddOrUpdateDish(dish);
        
        return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
    }
}