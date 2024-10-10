using System.Text.Json.Serialization;

namespace host.Models;

public class Table
{
    public int Id { get; set; }
    
    [JsonPropertyName("restaurant_id")]
    public int RestaurantId { get; set; }
    public int Number { get; set; }
    public Table(int id, int restaurantId, int number)
    {
        Id = id;
        RestaurantId = restaurantId;
        Number = number;
    }
}