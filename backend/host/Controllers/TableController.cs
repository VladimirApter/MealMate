using host.DataBaseAccess;
using host.Models;
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
    
    [HttpPost]
    public IActionResult PostTable([FromBody] Table? table)
    {
        if (table == null) return BadRequest("Table is null.");
        TableAccess.AddOrUpdateTable(table);
        
        return Ok();
    }
}