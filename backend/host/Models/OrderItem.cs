using System.Text.Json.Serialization;

namespace host.Models;

public class OrderItem
{
    public int Id { get; set; } 
        
    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }
    
    [JsonPropertyName("dish_id")]
    public int DishId { get; set; }
    public Dish Dish { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }

    public OrderItem(int id, int orderId, int dishId, Dish dish, int count, double price)
    {
        Id = id;
        OrderId = orderId;
        DishId = dishId;
        Dish = dish;
        Count = count;
        Price = price;
    }
}