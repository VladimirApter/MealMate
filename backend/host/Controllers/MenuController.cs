using host.DataBaseAccess;
using host.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{

    [HttpGet("{id}")]
    public IActionResult GetMenu(int id)
    {
        var menu = MenuAccess.GetMenu(id);
        if (menu == null) return NotFound();
        
        return Ok(menu);
    }
    
    [HttpPost]
    public IActionResult PostMenu([FromBody] Menu? menu)
    {
        if (menu == null) return BadRequest("Menu is null.");
        MenuAccess.AddOrUpdateMenu(menu);
        
        return Ok(menu.Id);
    }
}