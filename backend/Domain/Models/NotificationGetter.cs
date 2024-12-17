using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Models;

public class NotificationGetter(string username, long? id, long restaurantId, bool isBlocked)
    : TgAccount(id, username), ITableDataBase
{
    [JsonPropertyName("restaurant_id")] public long RestaurantId { get; init; } = restaurantId;

    [JsonPropertyName("is_blocked")] public bool IsBlocked { get; init; } = isBlocked;
}