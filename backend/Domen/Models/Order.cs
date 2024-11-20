using System.Text.Json.Serialization;

namespace Domen.Models;

public class Order
{
    public int? Id { get; set; }
    [JsonPropertyName("client_id")] public int ClientId { get; set; }
    [JsonPropertyName("table_id")] public int TableId { get; set; }
    [JsonPropertyName("cooking_time")] public double CookingTime { get; set; }
    public string? Comment { get; set; }
    [JsonPropertyName("date_time")] public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }
    public Client Client { get; set; }
    [JsonPropertyName("order_items")] public List<OrderItem> OrderItems { get; set; }

    public Order(int? id, int clientId, int tableId, string? comment, DateTime dateTime, Client client, List<OrderItem> orderItems)
    {
        Id = id;
        ClientId = clientId;
        TableId = tableId;
        Comment = comment;
        DateTime = dateTime;
        Status = OrderStatus.InAssembly;
        Client = client;
        CookingTime = orderItems.Max(item => item.MenuItem.CookingTimeMinutes);
        OrderItems = orderItems;
    }
}

public enum OrderStatus
{
    InAssembly,
    Cooking,
    Done,
}