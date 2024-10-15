using host.DataBase;
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
}