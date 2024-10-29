using host.Models;
using Microsoft.EntityFrameworkCore;

namespace host.DataBaseAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=./DataBase/DataBase.bd3");
    }
    
}
