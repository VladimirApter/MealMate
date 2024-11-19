using System.Text.Json.Serialization;

namespace host.Models;

public class OrderItem
{
    public int? Id { get; set; } 
    [JsonPropertyName("order_id")] public int OrderId { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }
    [JsonPropertyName("menu_item")] public MenuItem MenuItem { get; set; }
    
    public OrderItem(int? id, int orderId, int count, double price, MenuItem menuItem)
    {
        Id = id;
        OrderId = orderId;
        Count = count;
        Price = price;
        MenuItem = menuItem;
    }
}