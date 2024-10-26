using System.Text.Json.Serialization;

namespace host.Models;

public class MenuItem
{
    public int? Id { get; set; }
    
    [JsonPropertyName("category_id")] public int CategoryId { get; set; }
    public double Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [JsonPropertyName("cooking_time_minutes")]
    public int CookingTimeMinutes { get; set; }

    public MenuItem(int? id, double price, string name, string description, int cookingTimeMinutes,
        int categoryId)
    {
        Id = id;
        Price = price;
        Name = name;
        Description = description;
        CookingTimeMinutes = cookingTimeMinutes;
        CategoryId = categoryId;
    }
}