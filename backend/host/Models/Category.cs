using System.Text.Json.Serialization;

namespace host.Models;

public class Category
{
    public int Id { get; set; }
    
    [JsonPropertyName("menu_id")]
    public int MenuId { get; set; }
    public string Name { get; set; }
    public List<Dish> Dishes { get; set; }

    public Category(int id, int menuId, string name, List<Dish> dishes)
    {
        Id = id;
        MenuId = menuId;
        Name = name;
        Dishes = dishes;
    }
}