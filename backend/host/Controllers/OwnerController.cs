using host.DataBase;
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
}