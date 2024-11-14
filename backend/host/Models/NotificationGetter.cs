using System.Text.Json;
using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class NotificationGetter : TgAccount, ITableDataBase
{
    [JsonPropertyName("restaurant_id")]
    public int RestaurantId { get; set; }
    public NotificationGetter(string username, int? id, int restaurantId) : base(id, username)
    {
        RestaurantId = restaurantId;
    }
}