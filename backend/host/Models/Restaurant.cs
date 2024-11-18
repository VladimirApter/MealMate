using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;
using host.Logic;
using Microsoft.EntityFrameworkCore;

namespace host.Models;

public class Restaurant : ITableDataBase, ITakeRelatedData
{
    public int? Id { get; set; }
    
    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public GeoCoordinates Coordinates { get; set; }
    
    [JsonPropertyName("notification_getter")]
    [NotMapped] public NotificationGetter? NotificationGetter { get; set; }
    [NotMapped] public Menu? Menu { get; set; }
    [NotMapped] public List<Table>? Tables { get; set; }
    
    public Restaurant(){}
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

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        Menu = await context.Menus
            .FirstOrDefaultAsync(m => m.RestaurantId == Id);

        if (Menu == null)
            throw new Exception("Menu is null");
        

        if (Menu is ITakeRelatedData relatedDataLoaderMenu)
            await relatedDataLoaderMenu.TakeRelatedData(context);
        
        NotificationGetter = await context.NotificationGetters.FirstOrDefaultAsync(n => n.RestaurantId == Id);

        Tables = await context.Tables
            .Where(t => t.RestaurantId == Id)
            .ToListAsync();
        
    }
}