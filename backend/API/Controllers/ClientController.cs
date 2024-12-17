using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(long id)
    {
        var o = await DataBaseAccess<Client>.GetAsync(id);
        return o == null ? NotFound("Client is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostClient([FromBody] Client? client)
    {
        if (client == null) return NotFound("Client is null");
        
        DataBaseAccess<Client>.AddOrUpdate(client);
        return Ok(client.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteClient(long id)
    {
        var client = await DataBaseAccess<Client>.GetAsync(id);
        if (client == null) return NotFound("Client is null");

        DataBaseAccess<Client>.Delete(id);
        return Ok();
    }
}