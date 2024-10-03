namespace host.Models;

public class Menu
{
    public List<Dish> DishList { get; set; }

    public Menu(List<Dish> dishList)
    {
        DishList = dishList;
    } 
}