using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace host.Models;

public class Restaurant : ITableDataBase, ITakeRelatedData
{
    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }
    public int? Id { get; set; }

    [JsonPropertyName("notification_getter")]
    [NotMapped] public NotificationGetter? NotificationGetter { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
    [NotMapped] public Menu? Menu { get; set; }
    [NotMapped] public List<Table>? Tables { get; set; }
    public Restaurant(){}

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