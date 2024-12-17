using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NutrientsController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNutrients(long id)
    {
        var o = await DataBaseAccess<Nutrients>.GetAsync(id);
        return o == null ? NotFound("Nutrients is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostNutrients([FromBody] Nutrients? nutrients)
    {
        if (nutrients == null) return NotFound("Nutrients is null");
        
        DataBaseAccess<Nutrients>.AddOrUpdate(nutrients);
        return Ok(nutrients.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteNutrients(long id)
    {
        var nutrients = await DataBaseAccess<Nutrients>.GetAsync(id);
        if (nutrients == null) return NotFound("Nutrients is null");

        DataBaseAccess<Nutrients>.Delete(id);
        return Ok();
    }
}