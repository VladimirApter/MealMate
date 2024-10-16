using System.Text.Json.Serialization;

namespace host.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Dish?> Dishes { get; set; }

    public Category(string name, int id, List<Dish?> dishes)
    {
        Id = id;
        Name = name;
        Dishes = dishes;
    }
}