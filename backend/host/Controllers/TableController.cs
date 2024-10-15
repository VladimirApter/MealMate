using host.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{

    [HttpGet("{id}")]
    public IActionResult GetTable(int id)
    {
        var table = TableAccess.GetTable(id);
        if (table == null) return NotFound();
        
        return Ok(table);
    }
}