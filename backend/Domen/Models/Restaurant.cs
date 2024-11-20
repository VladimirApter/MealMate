using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domen.DataBaseAccess;
using Domen.Logic;
using Microsoft.EntityFrameworkCore;

namespace Domen.Models;

public class Restaurant : ITableDataBase, ITakeRelatedData, IDeleteRelatedData
{
    public int? Id { get; set; }
    [JsonPropertyName("owner_id")] public int OwnerId { get; set; }
    public string Name { get; set; }
    [NotMapped] public GeoCoordinates? Coordinates { get; set; }
    [JsonPropertyName("notification_getter")] [NotMapped] public NotificationGetter? NotificationGetter { get; set; }
    [NotMapped] public Menu? Menu { get; set; }
    [NotMapped] public List<Table>? Tables { get; set; }

    public Restaurant(){}
    public Restaurant(int? id, int ownerId, string name, GeoCoordinates? coordinates, NotificationGetter? notificationGetter,
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
        if (Menu != null)
            await Menu.TakeRelatedData(context);

        NotificationGetter = await context.NotificationGetters.FirstOrDefaultAsync(n => n.RestaurantId == Id);

        Tables = await context.Tables
            .Where(t => t.RestaurantId == Id)
            .ToListAsync();

        Coordinates = await context.GeoCoordinates.FirstOrDefaultAsync(c => c.RestaurantId == Id);
    }

    public void DeleteRelatedData(ApplicationDbContext context)
    {
        var menu = context.Menus.FirstOrDefault(m => m.RestaurantId == Id);
        if (menu != null)
        {
            context.Menus.Remove(menu);
            context.SaveChanges();
            menu.DeleteRelatedData(context);
        }

        var notificationGetter = context.NotificationGetters.FirstOrDefault(n => n.RestaurantId == Id);
        if (notificationGetter != null)
        {
            context.NotificationGetters.Remove(notificationGetter);
            context.SaveChanges();
        }

        foreach (var table in context.Tables.Where(t => t.RestaurantId == Id))
        {
            context.Tables.Remove(table);
            context.SaveChanges();
        }

        var coordinates = context.GeoCoordinates.FirstOrDefault(n => n.RestaurantId == Id);
        if (coordinates != null)
        {
            context.GeoCoordinates.Remove(coordinates);
            context.SaveChanges();
        }
    }
}