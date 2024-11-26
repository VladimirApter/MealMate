using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Models;

public class NotificationGetter : TgAccount, ITableDataBase
{
    [JsonPropertyName("restaurant_id")] public int RestaurantId { get; set; }
    
    [JsonPropertyName("is_blocked")] public bool IsBlocked { get; set; }
    
    public NotificationGetter(string username, int? id, int restaurantId, bool isBlocked) : base(id, username)
    {
        RestaurantId = restaurantId;
        IsBlocked = isBlocked;
    }
}