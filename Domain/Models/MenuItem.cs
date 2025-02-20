using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;
[JsonConverter(typeof(MenuItemConverter))]
public class MenuItem : ITakeRelatedData, IDeleteRelatedData
{
    public long? Id { get; set; }
    [JsonPropertyName("category_id")] public long CategoryId { get; set; }
    [JsonPropertyName("cooking_time_minutes")] public double CookingTimeMinutes { get; set; }
    public double Price { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("image_path")] public string? ImagePath { get; set; }
    public string Description { get; set; }
    [JsonPropertyName("nutrients_of_100_grams")] [NotMapped] public Nutrients? NutrientsOf100grams { get; set; }
    
    public MenuItem(){}
    public MenuItem(long? id, long categoryId, int cookingTimeMinutes, double price, string name, string description,
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

    public override string ToString()
    {
        var jsonObj = new
        {
            name = Name,
            image = ImagePath,
            price = Price,
            desc = Description,
            nutrients = NutrientsOf100grams ?? null,
            cooking_time = CookingTimeMinutes,
            volume = this is Dish ? ((Dish)this).Weight.ToString() + "г" : ((Drink)this).Volume.ToString() + "мл",
        };
        
        return JsonSerializer.Serialize(jsonObj);
    }
}