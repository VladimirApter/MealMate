using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Order : ITableDataBase, ITakeRelatedData
{
    private double price;
    public long? Id { get; set; }
    [JsonPropertyName("client_id")] public long? ClientId { get; set; }
    [JsonPropertyName("table_id")] public long TableId { get; init; }
    [JsonPropertyName("cooking_time_minutes")] public double CookingTimeMinutes { get; init; }
    [NotMapped] public double Price { get => Math.Round(price, 2); set => price = value; }
    public string? Comment { get; init; }
    [JsonPropertyName("date_time")] public DateTime DateTime { get; init; }
    public OrderStatus Status { get; set; }
    [NotMapped] public Client? Client { get; set; }
    [JsonPropertyName("order_items")] [NotMapped] public List<OrderItem> OrderItems { get; set; }

    public Order(){}
    public Order(long? id, long tableId, string? comment, DateTime dateTime, Client? client, List<OrderItem> orderItems)
    {
        Id = id;
        TableId = tableId;
        Comment = comment;
        DateTime = dateTime;
        Status = OrderStatus.InAssembly;
        Client = client;
        CookingTimeMinutes = orderItems.Max(item =>
        {
            if (item.MenuItem != null) return item.MenuItem.CookingTimeMinutes;
            return -1;
        });
        OrderItems = orderItems;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        Client = await context.Clients.FirstOrDefaultAsync(c => c.Id == ClientId);
        OrderItems = await context.OrderItems
            .Where(c => c.OrderId == Id)
            .ToListAsync();
        Price = 0;
        foreach (var orderItem in OrderItems)
        {
            await orderItem.TakeRelatedData(context);
            Price += orderItem.Price;
        }
    }
}

public enum OrderStatus
{
    InAssembly,
    Cooking,
    Done,
    Cancelled
}