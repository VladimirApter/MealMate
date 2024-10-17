using host.DataBaseAccess;
using host.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetOwner(int id)
    {
        var owner = OwnerAccess.GetOwner(id);
        if (owner == null) return NotFound();
        
        return Ok(owner);
    }
    
    [HttpPost]
    public IActionResult PostOwner([FromBody] Owner? owner)
    {
        if (owner == null) return BadRequest("Owner is null.");
        OwnerAccess.AddOrUpdateOwner(owner);
        
        return Ok();
    }
}