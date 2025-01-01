using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Menu : ITableDataBase, ITakeRelatedData, IDeleteRelatedData
{
    public long? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public long RestaurantId { get; set; }
    [JsonPropertyName("excel_table_path")] public string? ExcelTablePath { get; set; }
    [JsonPropertyName("categories")] [NotMapped] public List<Category>? Categories { get; set; }

    public Menu(){}
    public Menu(long? id, List<Category>? categories, long restaurantId, string? excelTablePath)
    {
        Id = id;
        RestaurantId = restaurantId;
        Categories = categories;
        ExcelTablePath = excelTablePath;
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