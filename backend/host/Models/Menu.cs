using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace host.Models;

public class Menu : ITableDataBase, ITakeRelatedData, IDeleteRelatedData
{
    public int? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public int RestaurantId { get; set; }
    [JsonPropertyName("categories")] [NotMapped] public List<Category>? Categories { get; set; }

    public Menu(){}
    public Menu(int? id, List<Category>? categories, int restaurantId)
    {
        Id = id;
        RestaurantId = restaurantId;
        Categories = categories;
    } 
    

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        Categories = await context.Categories
            .Where(c => c.MenuId == Id)
            .ToListAsync();

        foreach (var category in Categories.OfType<ITakeRelatedData>())
        {
            await category.TakeRelatedData(context);
        }
    }

    public void DeleteRelatedData(ApplicationDbContext context)
    {
        foreach (var category in context.Categories
                     .Where(c => c.MenuId == Id))
        {
            context.Categories.Remove(category);
            context.SaveChanges();
            category.DeleteRelatedData(context);
        }
    }
}