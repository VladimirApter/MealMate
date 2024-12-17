using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;
[JsonConverter(typeof(MenuItemConverter))]
public class MenuItem : ITakeRelatedData, IDeleteRelatedData
{
    public long? Id { get; set; }
    [JsonPropertyName("category_id")] public long CategoryId { get; }
    [JsonPropertyName("cooking_time_minutes")] public double CookingTimeMinutes { get; }
    public double Price { get; }
    public string Name { get; }
    [JsonPropertyName("image_path")] public string? ImagePath { get; }
    private string Description { get; }
    [JsonPropertyName("nutrients_of_100_grams")] [NotMapped] public Nutrients? NutrientsOf100Grams { get; private set; }
    
    public MenuItem(){}
    public MenuItem(long? id, long categoryId, int cookingTimeMinutes, double price, string name, string description,
        string? imagePath, Nutrients? nutrientsOf100Grams)
    {
        Id = id;
        CategoryId = categoryId;
        CookingTimeMinutes = cookingTimeMinutes;
        Price = price;
        Name = name;
        ImagePath = imagePath;
        Description = description;
        NutrientsOf100Grams = nutrientsOf100Grams;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        NutrientsOf100Grams = await context.Nutrients.FirstOrDefaultAsync(n => n.MenuItemId == Id);
    }

    public void DeleteRelatedData(ApplicationDbContext context)
    {
        var nutrient = context.Nutrients.FirstOrDefault(n => n.MenuItemId == Id);
        if (nutrient == null) return;
        context.Nutrients.Remove(nutrient);
        context.SaveChanges();
    }

    public override string ToString()
    {
        var jsonObj = new
        {
            name = Name,
            image = ImagePath,
            price = Price,
            desc = Description,
            nutrients = NutrientsOf100Grams ?? null,
            cooking_time = CookingTimeMinutes
        };
        
        return JsonSerializer.Serialize(jsonObj);
    }
}