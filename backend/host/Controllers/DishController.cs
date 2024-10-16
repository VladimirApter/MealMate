using host.DataBaseAccess;
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
}