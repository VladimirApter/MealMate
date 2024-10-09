namespace host.Models;

public class Order
{
    public int Id { get; }
    public int ClientId { get; set; }
    public int RestaurantId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public string Comment { get; set; }
    public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }

    public Order(int clientId, int restaurantId, List<OrderItem> orderItems, string comment, DateTime dateTime)
    {
        Id = 1;
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