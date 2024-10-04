using System.Text.Json.Serialization;

namespace host.Models;

public class Menu
{
    [JsonPropertyName("dish_list")]
    public List<Dish> DishList { get; set; }

    public Menu(List<Dish> dishList)
    {
        DishList = dishList;
    } 
}