using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using host.Models;

namespace host.Controllers;
    

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    //Пример
    private static Restaurant restaurant = new Restaurant(1, 1,
        new NotificationGetter(2, "@Stasyamba52"),
        "Клод моне",
        "ул. Спиридоновка, 25/20с1, Москва",
        new Menu(1, 1,new List<Category>()
            {
                new Category(1, 1, "Супы", new List<Dish>()
                {
                    new Dish(1, 1, 500, 200, "Буйабес", "Суп от шефа", 15)
                }),
                new Category(2,1,"Горячее", new List<Dish>()
                {
                    new Dish(2, 2, 140, 1200, "Пюре", "Огузок навалил жиденькой пюрехи", 55)
                })
            }),
        new List<Table>()
        {
            new Table(1, 1, 1), new Table(2, 1, 17), new Table(3, 1, 3)
        });
    
    [HttpGet]
    public IActionResult GetRestaurant()
    {
        return Ok(restaurant);
    }
}