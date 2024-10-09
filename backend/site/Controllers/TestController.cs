using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace host.Controllers;
    

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMenu()
    {
        return Ok("Эти странные глаза\nСмотрят на тебя, скрббяяя\nЧто это за кринж?\nЧто за Джигурда?\nАааа ага ага ага ага\nЭто Джига Дрыга\nДжига Джигурда!\nЭй Джига-Дрыга!\nОджибудда Джига\nДжига джига дрыга\nДжига Джигурда упс-а!\nДжига джига дрыга\nОджибудда буда\nДжик аджи аджика\nОджибудда Джиган!\nЭй Джига-Дрыга!\nОджибудда Джига\nДжига джига дрыга\nДжига Джигурда упс-а!\nДжига джига дрыга\nОджибудда буда\nДжик аджи аджика\nОджибудда Джиган!");
    }
}