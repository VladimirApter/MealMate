using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Category : ITableDataBase, ITakeRelatedData, IDeleteRelatedData
{
    public long? Id { get; set; }
    [JsonPropertyName("menu_id")] public long MenuId { get; init; }
    public string Name { get; init; }
    [JsonPropertyName("menu_items")] [NotMapped] public List<MenuItem>? MenuItems { get; set; }
    
    public Category() {}
    public Category(long? id, long menuId, string name, List<MenuItem>? menuItems)
    {
        Id = id;
        MenuId = menuId;
        Name = name;
        MenuItems = menuItems;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        MenuItems = new List<MenuItem>();

        foreach (var dish in await context.Dishes
                     .Where(d => d.CategoryId == Id).ToListAsync())
        {
            await dish.TakeRelatedData(context);
            MenuItems.Add(dish);
        }

        foreach (var drink in await context.Drinks
                     .Where(d => d.CategoryId == Id).ToListAsync())
        {
            await drink.TakeRelatedData(context);
            MenuItems.Add(drink);
        }
    }

    public void DeleteRelatedData(ApplicationDbContext context)
    {
        foreach (var dish in context.Dishes
                     .Where(d => d.CategoryId == Id))
        {
            context.Dishes.Remove(dish);
            context.SaveChanges();
            dish.DeleteRelatedData(context);
        }
        foreach (var drink in context.Drinks
                     .Where(d => d.CategoryId == Id))
        {
            context.Drinks.Remove(drink);
            context.SaveChanges();
            drink.DeleteRelatedData(context);
        }
    }
}