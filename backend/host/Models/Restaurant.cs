using System.Text.Json.Serialization;
using host.DataBaseAccess;
using host.Logic;

namespace host.Models;

public class Restaurant : ITableDataBase
{
    public int? Id { get; set; }
    
    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public GeoCoordinates Coordinates { get; set; }
    
    [JsonPropertyName("notification_getter")]
    public NotificationGetter? NotificationGetter { get; set; }
    public Menu? Menu { get; set; }
    public List<Table>? Tables { get; set; }

    public Restaurant(int? id, int ownerId, string name, GeoCoordinates coordinates, NotificationGetter? notificationGetter,
        Menu? menu, List<Table>? tables)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        Coordinates = coordinates;
        NotificationGetter = notificationGetter;
        Menu = menu;
        Tables = tables;
    }
}