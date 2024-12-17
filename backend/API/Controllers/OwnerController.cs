using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOwner(long id)
    {
        var o = await DataBaseAccess<Owner>.GetAsync(id);
        return o == null ? NotFound("Owner is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostOwner([FromBody] Owner? owner)
    {
        if (owner == null) return NotFound("Owner is null");
        
        DataBaseAccess<Owner>.AddOrUpdate(owner);
        return Ok(owner.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteOwner(long id)
    {
        var owner = await DataBaseAccess<Owner>.GetAsync(id);
        if (owner == null) return NotFound("Owner is null");

        DataBaseAccess<Owner>.Delete(id);
        return Ok();
    }
}