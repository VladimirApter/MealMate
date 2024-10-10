using System.Text.Json.Serialization;

namespace host.Models;

public class Order
{
    public int Id { get; set; }
    
    [JsonPropertyName("client_id")]
    public int ClientId { get; set; }
    
    [JsonPropertyName("restaurant_id")]
    public int RestaurantId { get; set; }
    
    [JsonPropertyName("order_items")]
    public List<OrderItem> OrderItems { get; set; }
    public string Comment { get; set; }
    
    [JsonPropertyName("datetime")]
    public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }

    public Order(int id, int clientId, int restaurantId, List<OrderItem> orderItems, string comment, DateTime dateTime)
    {
        Id = id;
        ClientId = clientId;
        RestaurantId = restaurantId;
        OrderItems = orderItems;
        Comment = comment;
        DateTime = dateTime;
        Status = OrderStatus.InAssembly;
    }
}

public enum OrderStatus
{
    InAssembly,
    Cooking,
    Done,
}