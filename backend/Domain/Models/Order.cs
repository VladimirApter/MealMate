using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Order : ITableDataBase, ITakeRelatedData, IDeleteRelatedData
{
    public int? Id { get; set; }
    [JsonPropertyName("client_id")] public int? ClientId { get; set; }
    [JsonPropertyName("table_id")] public int TableId { get; set; }
    [JsonPropertyName("cooking_time")] public double CookingTime { get; set; }
    public string? Comment { get; set; }
    [JsonPropertyName("date_time")] public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }
    [NotMapped] public Client Client { get; set; }
    [JsonPropertyName("order_items")] [NotMapped] public List<OrderItem> OrderItems { get; set; }

    public Order(){}
    public Order(int? id, int tableId, string? comment, DateTime dateTime, Client client, List<OrderItem> orderItems)
    {
        Id = id;
        TableId = tableId;
        Comment = comment;
        DateTime = dateTime;
        Status = OrderStatus.InAssembly;
        Client = client;
        CookingTime = orderItems.Max(item => item.MenuItem.CookingTimeMinutes);
        OrderItems = orderItems;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        Client = await context.Clients.FirstOrDefaultAsync(c => c.Id == ClientId);
        OrderItems = await context.OrderItems.Where(oi => oi.OrderId == Id).ToListAsync();
    }

    public void DeleteRelatedData(ApplicationDbContext context)
    {
        var client = context.Clients.FirstOrDefault(c => c.Id == ClientId);
        if (client != null)
        {
            context.Clients.Remove(client);
            context.SaveChanges();
        }
        var orderItems = context.OrderItems.Where(oi => oi.OrderId == Id);
        foreach (var orderItem in orderItems)
        {
            context.OrderItems.Remove(orderItem);
            context.SaveChanges();
            orderItem.DeleteRelatedData(context);
        }
    }
}

public enum OrderStatus
{
    InAssembly,
    Cooking,
    Done,
}