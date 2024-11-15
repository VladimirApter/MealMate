using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace host.Models;

public class Category : ITableDataBase, ITakeRelatedData
{
    [JsonPropertyName("menu_id")]
    public int MenuId { get; set; }
    public int? Id { get; set; }
    public string Name { get; set; }
    [NotMapped] public List<Dish>? Dishes { get; set; }
    public Category(){}
    public Category(string name, int? id, int menuId, List<Dish>? dishes)
    {
        Id = id;
        Name = name;
        Dishes = dishes;
        MenuId = menuId;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        Dishes = await context.Dishes
            .Where(d => d.CategoryId == Id)
            .ToListAsync();
    }
}