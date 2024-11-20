using System.Text.Json.Serialization;
using Domen.DataBaseAccess;

namespace Domen.Logic;

public class GeoCoordinates : ITableDataBase
{
    public int? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public int RestaurantId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public GeoCoordinates(int restaurantId, double latitude, double longitude)
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