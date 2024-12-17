using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderItem(long id)
    {
        var o = await DataBaseAccess<OrderItem>.GetAsync(id);
        return o == null ? NotFound("OrderItem is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostOrderItem([FromBody] OrderItem? orderItem)
    {
        if (orderItem == null) return NotFound("OrderItem is null");
        
        DataBaseAccess<OrderItem>.AddOrUpdate(orderItem);
        return Ok(orderItem.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteOrderItem(long id)
    {
        var orderItem = await DataBaseAccess<OrderItem>.GetAsync(id);
        if (orderItem == null) return NotFound("OrderItem is null");

        DataBaseAccess<OrderItem>.Delete(id);
        return Ok();
    }
}