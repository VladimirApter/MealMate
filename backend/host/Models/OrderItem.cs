using System.Text.Json.Serialization;

namespace host.Models;

public class OrderItem
{
    public int Id { get; set; } 
        
    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }
    
    [JsonPropertyName("menu_item")]
    public MenuItem MenuItem { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }
    
    public OrderItem(int id, int orderId, MenuItem menuItem, int count, double price)
    {
        Id = id;
        OrderId = orderId;
        MenuItem = menuItem;
        Count = count;
        Price = price;
    }
}