using System.Text.Json.Serialization;

namespace host.Models;
[JsonConverter(typeof(MenuItemConverter))]
public class MenuItem
{
    public int? Id { get; set; }
    
    [JsonPropertyName("category_id")] 
    public int CategoryId { get; set; }
    
    [JsonPropertyName("cooking_time_minutes")]
    public double CookingTimeMinutes { get; set; }
    public double Price { get; set; }
    public string Name { get; set; }
    
    [JsonPropertyName("image_path")]
    public string? ImagePath { get; set; }
    public string Description { get; set; }
    
    [JsonPropertyName("nutrients_of_100_grams")]
    public Nutrients? NutrientsOf100grams { get; set; }

    public MenuItem(int? id, int categoryId, int cookingTimeMinutes, double price, string name, string description,
        string? imagePath, Nutrients? nutrientsOf100grams)
    {
        Id = id;
        CategoryId = categoryId;
        CookingTimeMinutes = cookingTimeMinutes;
        Price = price;
        Name = name;
        ImagePath = imagePath;
        Description = description;
        NutrientsOf100grams = nutrientsOf100grams;
    }
}