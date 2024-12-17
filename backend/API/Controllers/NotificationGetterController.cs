using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationGetterController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotificationGetter(long id)
    {
        var o = await DataBaseAccess<NotificationGetter>.GetAsync(id);
        return o == null ? NotFound("NotificationGetter is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostNotificationGetter([FromBody] NotificationGetter? notificationGetter)
    {
        if (notificationGetter == null) return NotFound("NotificationGetter is null");
        
        DataBaseAccess<NotificationGetter>.AddOrUpdate(notificationGetter);
        return Ok(notificationGetter.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteNotificationGetter(long id)
    {
        var notificationGetter = await DataBaseAccess<NotificationGetter>.GetAsync(id);
        if (notificationGetter == null) return NotFound("NotificationGetter is null");

        DataBaseAccess<NotificationGetter>.Delete(id);
        return Ok();
    }
}