using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace host.Models;
[JsonConverter(typeof(MenuItemConverter))]
public class MenuItem : ITakeRelatedData, IDeleteRelatedData
{
    public int? Id { get; set; }
    [JsonPropertyName("category_id")] public int CategoryId { get; set; }
    [JsonPropertyName("cooking_time_minutes")] public double CookingTimeMinutes { get; set; }
    public double Price { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("image_path")] public string? ImagePath { get; set; }
    public string Description { get; set; }
    [JsonPropertyName("nutrients_of_100_grams")] [NotMapped] public Nutrients? NutrientsOf100grams { get; set; }
    
    public MenuItem(){}
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

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        NutrientsOf100grams = await context.Nutrients.FirstOrDefaultAsync(n => n.MenuItemId == Id);
    }

    public void DeleteRelatedData(ApplicationDbContext context)
    {
        var nutrient = context.Nutrients.FirstOrDefault(n => n.MenuItemId == Id);
        if (nutrient == null) return;
        context.Nutrients.Remove(nutrient);
        context.SaveChanges();
    }
}