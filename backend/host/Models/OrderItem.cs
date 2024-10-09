namespace host.Models;

public class OrderItem
{
    public Dish Dish { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }

    public OrderItem(Dish dish, int count, double price)
    {
        Dish = dish;
        Count = count;
        Price = price;
    }
}