using host.DataBaseAccess;
using host.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationGetterController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetNotificationGetter(int id)
    {
        var notificationGetter = NotificationGetterAccess.GetNotificationGetter(id);
        if (notificationGetter == null) return NotFound();

        return Ok(notificationGetter);
    }
    [HttpPost]
    public IActionResult PostNotificationGetter([FromBody] NotificationGetter? notificationGetter)
    {
        if (notificationGetter == null) return BadRequest("NotificationGetter is null.");
        NotificationGetterAccess.AddOrUpdateNotificationGetter(notificationGetter);
        
        return Ok(notificationGetter.Id);
    }
}