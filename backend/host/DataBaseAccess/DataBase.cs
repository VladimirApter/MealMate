using host.Logic;
using host.Models;
using Microsoft.EntityFrameworkCore;

namespace host.DataBaseAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<NotificationGetter> NotificationGetters { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<GeoCoordinates> GeoCoordinates { get; set; }
    public DbSet<Nutrients> Nutrients { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=./DataBase/DataBase.bd3");
    }
    
}
