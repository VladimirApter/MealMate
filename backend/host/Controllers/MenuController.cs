using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using host.Models;

namespace host.Controllers;
    
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMenu()
    {
        var menu = new Menu(new List<Dish>()
        {
            new Dish(150, 200, "Окрошка", "В составе: квас, огурцы, кортошка, яица, колбаса"),
            new Dish(200, 300, "Шашлык", "В составе: свинина, лук, помидоры, черный перец")
        });
        return Ok(menu);
    }
}