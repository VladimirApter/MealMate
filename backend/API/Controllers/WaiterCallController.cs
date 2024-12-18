using Domain.DataBaseAccess;
using Domain.Logic;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WaiterCallController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWaiterCall(long id)
    {
        var o = await DataBaseAccess<WaiterCall>.GetAsync(id);
        return o == null ? NotFound("WaiterCall is null") : Ok(o);
    }
    
    [HttpPost]
    public async Task<IActionResult> PostWaiterCall([FromBody] WaiterCall? waiterCall)
    {
        if (waiterCall == null) return NotFound("WaiterCall is null");
        
        if (waiterCall.Id != null)
        {
            var o = await DataBaseAccess<WaiterCall>.GetAsync(waiterCall.Id.Value);
            DataBaseAccess<WaiterCall>.AddOrUpdate(waiterCall);
            if (o == null)
            {
                await ForwardToPythonServer.ForwardObject(waiterCall, $"{HostsUrlGetter.PyServerUrl}/waitercall/");
            }
        }
        else
        {
            DataBaseAccess<WaiterCall>.AddOrUpdate(waiterCall);
            await ForwardToPythonServer.ForwardObject(waiterCall, $"{HostsUrlGetter.PyServerUrl}/waitercall/");
        }
        
        return Ok(waiterCall.Id);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWaiterCall(long id)
    {
        var waiterCall = await DataBaseAccess<WaiterCall>.GetAsync(id);
        if (waiterCall == null) return NotFound("WaiterCall is null");

        DataBaseAccess<WaiterCall>.Delete(id);
        return Ok();
    }
}