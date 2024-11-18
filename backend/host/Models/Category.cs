using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace host.Models;

public class Category : ITableDataBase, ITakeRelatedData
{
    public int? Id { get; set; }
    [JsonPropertyName("menu_id")]
    public int MenuId { get; set; }
    public string Name { get; set; }
    
    [JsonPropertyName("menu_items")]
    [NotMapped] public List<MenuItem>? MenuItems { get; set; }

    public Category(int? id, int menuId, string name, List<MenuItem>? menuItems)
    {
        Id = id;
        MenuId = menuId;
        Name = name;
        MenuItems = menuItems;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        MenuItems = await context.Dishes
            .Where(d => d.CategoryId == Id)
            .ToListAsync();
        
    }
}