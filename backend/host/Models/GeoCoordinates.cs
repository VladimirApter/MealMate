namespace host.Logic;

public class GeoCoordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public GeoCoordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
    
    public override string ToString()
    {
        return $"({Latitude}, {Longitude})";
    }
}