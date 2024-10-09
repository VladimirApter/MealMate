using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using host.Models;

namespace host.Controllers;
    

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    //Пример
    private static Restaurant restaurant = new Restaurant(
        new Owner(1, "@Yuchik55"),
        new NotificationGetter(2, "@Stasyamba52"),
        "Клод моне",
        "ул. Спиридоновка, 25/20с1, Москва",
        new Menu(new List<Category>()
            {
                new Category("Супы", new List<Dish>()
                {
                    new Dish(500, 200, "Буйабес", "Суп от шефа", 15)
                }),
                new Category("Горячее", new List<Dish>()
                {
                    new Dish(140, 1200, "Пюре", "Огузок навалил жиденькой пюрехи", 55)
                })
            }),
        new List<Table>()
        {
            new Table(1), new Table(2), new Table(3)
        });
        
    [HttpGet]
    public IActionResult GetMenu()
    {
        return Ok(restaurant);
    }
}