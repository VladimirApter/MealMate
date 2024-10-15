using host.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/notification_getter")]
public class NotificationGetterController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetNotificationGetter(int id)
    {
        var notificationGetter = NotificationGetterAccess.GetNotificationGetter(id);
        if (notificationGetter == null) return NotFound();

        return Ok(notificationGetter);
    }
}