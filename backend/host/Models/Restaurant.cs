using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Restaurant : ITableDataBase
{
    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }
    public int? Id { get; set; }

    [JsonPropertyName("notification_getter")]
    public NotificationGetter? NotificationGetter { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
    public Menu? Menu { get; set; }
    public List<Table>? Tables { get; set; }

    public Restaurant(NotificationGetter? notificationGetter, string name, string address, Menu? menu,
        List<Table>? tables, int? id, int ownerId)
    {
        NotificationGetter = notificationGetter;
        Name = name;
        Address = address;
        Menu = menu;
        Tables = tables;
        Id = id;
        OwnerId = ownerId;
    }
}