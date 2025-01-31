using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinkController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDrink(long id)
    {
        var o = await DataBaseAccess<Drink>.GetAsync(id);
        return o == null ? NotFound("Drink is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostDrink([FromBody] Drink? drink)
    {
        if (drink == null) return NotFound("Drink is null");
        
        DataBaseAccess<Drink>.AddOrUpdate(drink);
        return Ok(drink.Id);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrink(long id)
    {
        var drink = await DataBaseAccess<Drink>.GetAsync(id);
        if (drink == null) return NotFound("Drink is null");

        DataBaseAccess<Drink>.Delete(id);
        return Ok();
    }
}