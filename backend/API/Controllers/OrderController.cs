using Domain.DataBaseAccess;
using Domain.Logic;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(long id)
    {
        var o = await DataBaseAccess<Order>.GetAsync(id);
        return o == null ? NotFound("Order is null") : Ok(o);
    }
    
    [HttpPost]
    public async Task<IActionResult> PostOrder([FromBody] Order? order)
    {
        if (order == null) return NotFound("Order is null");
        if (order.Id == null) return NotFound(order.Id);
        var o = await DataBaseAccess<Order>.GetAsync(order.Id.Value);
        DataBaseAccess<Order>.AddOrUpdate(order);
        if (o == null) await ForwardToPythonServer.ForwardObject(order, $"{HostsUrlGetter.PyServerUrl}/order/");
        return Ok(order.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteOrder(long id)
    {
        var order = await DataBaseAccess<Order>.GetAsync(id);
        if (order == null) return NotFound("Order is null");

        DataBaseAccess<Order>.Delete(id);
        return Ok();
    }
}