using System.Text.Json.Serialization;

namespace host.Models;

public class Owner : TgAccount
{
    [JsonPropertyName("restaurant_ids")]
    public List<int> RestaurantIds { get; set; }
    public Owner(string username, List<int> restaurantIds) : base(username)
    {
        RestaurantIds = restaurantIds;
    }
}