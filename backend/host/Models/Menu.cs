using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace host.Models;

public class Menu : ITableDataBase, ITakeRelatedData
{
    public int? Id { get; set; }
    
    [JsonPropertyName("restaurant_id")]
    public int RestaurantId { get; set; }
    [JsonPropertyName("categories")]
    [NotMapped] public List<Category>? Categories { get; set; }

    public Menu(){}
    public Menu(int? id, List<Category>? categories, int restaurantId)
    {
        Id = id;
        Categories = categories;
        RestaurantId = restaurantId;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        Categories = await context.Categories
            .Where(c => c.MenuId == Id)
            .ToListAsync();
           
        foreach (var category in Categories)
        {
            if (category is ITakeRelatedData relatedDataLoader)
            {
                await relatedDataLoader.TakeRelatedData(context);
            }
        }
    }
}