using System.Text.Json.Serialization;

namespace host.Models;

public class Owner : TgAccount
{
    [JsonPropertyName("restaurant_ids")]
    public List<int> RestaurantIds { get; set; }
    public Owner(string username, int id, List<int> restaurantIds) : base(username, id)
    {
        Id = id;
        RestaurantIds = restaurantIds;
    }
}