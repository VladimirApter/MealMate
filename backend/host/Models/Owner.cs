using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Owner : TgAccount, ITableDataBase
{
    [JsonPropertyName("restaurant_ids")]
    public List<int>? RestaurantIds { get; set; }
    public Owner(string username, int? id, List<int>? restaurantIds) : base(username, id)
    {
        RestaurantIds = restaurantIds;
    }
}