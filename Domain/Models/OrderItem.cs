using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class OrderItem : ITableDataBase, ITakeRelatedData
{
    public long? Id { get; set; } 
    [JsonPropertyName("menu_item_id")] public long? MenuItemId { get; set; }
    [JsonPropertyName("order_id")] public long OrderId { get; set; }
    public int Count { get; set; }
    [NotMapped] public double Price { get; set; }
    [JsonPropertyName("menu_item")] [NotMapped] public MenuItem? MenuItem { get; set; }
    
    public OrderItem(){}
    public OrderItem(long? id, long orderId, int count, MenuItem menuItem)
    {
        Id = id;
        OrderId = orderId;
        Count = count;
        Price = menuItem.Price*count;
        MenuItem = menuItem;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        MenuItem = await context.Dishes
            .FirstOrDefaultAsync(d => d.Id == MenuItemId) ?? (MenuItem?)await context.Drinks
            .FirstOrDefaultAsync(d => d.Id == MenuItemId);
        if (MenuItem != null)
        {
            Price = MenuItem.Price*Count;
            await MenuItem.TakeRelatedData(context);
        }
    }
}