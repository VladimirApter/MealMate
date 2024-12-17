using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenu(long id)
    {
        var o = await DataBaseAccess<Menu>.GetAsync(id);
        return o == null ? NotFound("Menu is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostMenu([FromBody] Menu? menu)
    {
        if (menu == null) return NotFound("Menu is null");
        
        DataBaseAccess<Menu>.AddOrUpdate(menu);
        return Ok(menu.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteMenu(long id)
    {
        var menu = await DataBaseAccess<Menu>.GetAsync(id);
        if (menu == null) return NotFound("Menu is null");

        DataBaseAccess<Menu>.Delete(id);
        return Ok();
    }
}