using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Models;

public class GeoCoordinates(long restaurantId, double latitude, double longitude) : ITableDataBase
{
    public long? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public long RestaurantId { get; init; } = restaurantId;
    public double Latitude { get; init; } = latitude;
    public double Longitude { get; init; } = longitude;

    public override string ToString()
    {
        return $"({Latitude}, {Longitude})";
    }
}