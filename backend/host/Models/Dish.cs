using System.Text.Json.Serialization;

namespace host.Models;

public class Dish
{
    public double Price { get; set; }
    public double Weight { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    [JsonPropertyName("cooking_time_minutes")]
    public int CookingTimeMinutes { get; set; }

    public Dish(double price, double weight, string name, string description, int cookingTimeMinutes)
    {
        Price = price;
        Weight = weight;
        Name = name;
        Description = description;
        CookingTimeMinutes = cookingTimeMinutes;
    }
}