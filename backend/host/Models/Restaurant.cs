using System.Text.Json.Serialization;

namespace host.Models;

public class Restaurant
{
    public int Id { get; set; }
    
    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }
    
    [JsonPropertyName("notification_getter")]
    public NotificationGetter NotificationGetter { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public Menu Menu { get; set; }
    public List<Table> Tables { get; }
    
    public Restaurant(int id, int ownerId, NotificationGetter notificationGetter, string name, string address, Menu menu, List<Table> tables)
    {
        Id = id;
        OwnerId = ownerId;
        NotificationGetter = notificationGetter;
        Name = name;
        Address = address;
        Menu = menu;
        Tables = tables;
    }
}