using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using host.Models;

namespace host.Controllers;
    

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    //дефолтное меню
    private static Menu menu = new(new List<Dish>()
    {
        new(150, 200, "Окрошка", "В составе: квас, огурцы, кортошка, яица, колбаса"),
        new(200, 300, "Шашлык", "В составе: свинина, лук, помидоры, черный перец")
    });
    
    [HttpGet]
    public IActionResult GetMenu()
    {
        return Ok(menu);
    }

    [HttpPost]
    public IActionResult CreateMenu([FromBody] Menu newMenu)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid model object");

        menu = newMenu;
        return Ok(menu);
    }
}