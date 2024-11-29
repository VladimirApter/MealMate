using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Logic;

public class GeoCoordinates : ITableDataBase
{
    public long? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public long RestaurantId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public GeoCoordinates(long restaurantId, double latitude, double longitude)
    {
        RestaurantId = restaurantId;
        Latitude = latitude;
        Longitude = longitude;
    }
    
    public override string ToString()
    {
        return $"({Latitude}, {Longitude})";
    }
}