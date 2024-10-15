using System.Text.Json.Serialization;

namespace host.Models;

public class Category
{
    public string Name { get; set; }
    public List<Dish?> Dishes { get; set; }

    public Category(string name, List<Dish?> dishes)
    {
        Name = name;
        Dishes = dishes;
    }
}