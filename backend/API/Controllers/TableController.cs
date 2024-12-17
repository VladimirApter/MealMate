using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTable(long id)
    {
        var o = await DataBaseAccess<Table>.GetAsync(id);
        return o == null ? NotFound("Table is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostTable([FromBody] Table? entityTable)
    {
        if (entityTable == null) return NotFound("Table is null");
        
        var table = new Table(entityTable.Id, entityTable.RestaurantId, entityTable.Number);
        if (entityTable.Id == null)
        {
            DataBaseAccess<Table>.AddOrUpdateTable(table, true);
            var tableNew = new Table(table.Id, table.RestaurantId, table.Number);
            DataBaseAccess<Table>.AddOrUpdateTable(tableNew, true);
        }
        else
            DataBaseAccess<Table>.AddOrUpdateTable(table, false);

        return Ok(table.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteTable(long id)
    {
        var table = await DataBaseAccess<Table>.GetAsync(id);
        if (table == null) return NotFound("Table is null");

        DataBaseAccess<Table>.Delete(id);
        return Ok();
    }
}