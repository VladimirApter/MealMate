using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class OrderItem : ITableDataBase, ITakeRelatedData, IDeleteRelatedData
{
    public int? Id { get; set; } 
    [JsonPropertyName("menu_item_id")] public int? MenuItemId { get; set; }
    [JsonPropertyName("order_id")] public int OrderId { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }
    [JsonPropertyName("menu_item")] [NotMapped] public MenuItem? MenuItem { get; set; }
    
    public OrderItem(){}
    public OrderItem(int? id, int orderId, int count, double price, MenuItem menuItem)
    {
        Id = id;
        OrderId = orderId;
        Count = count;
        Price = price;
        MenuItem = menuItem;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        MenuItem = await context.Dishes
            .FirstOrDefaultAsync(d => d.Id == MenuItemId) ?? (MenuItem?)await context.Drinks
            .FirstOrDefaultAsync(d => d.Id == MenuItemId);

        if (MenuItem != null)
            await MenuItem.TakeRelatedData(context);
    }

    public void DeleteRelatedData(ApplicationDbContext context)
    {
        foreach (var dish in context.Dishes
                     .Where(d => d.Id == MenuItemId))
        {
            context.Dishes.Remove(dish);
            context.SaveChanges();
            dish.DeleteRelatedData(context);
        }
        foreach (var drink in context.Drinks
                     .Where(d => d.Id == MenuItemId))
        {
            context.Drinks.Remove(drink);
            context.SaveChanges();
            drink.DeleteRelatedData(context);
        }
    }
}