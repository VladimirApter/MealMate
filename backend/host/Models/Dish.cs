using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Dish : ITableDataBase
{
    public int? Id { get; set; }
    
    [JsonPropertyName("category_id")] public int CategoryId { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [JsonPropertyName("cooking_time_minutes")]
    public int CookingTimeMinutes { get; set; }

    public Dish(int? id, double price, double weight, string name, string description, int cookingTimeMinutes,
        int categoryId)
    {
        Id = id;
        Price = price;
        Weight = weight;
        Name = name;
        Description = description;
        CookingTimeMinutes = cookingTimeMinutes;
        CategoryId = categoryId;
    }
}