using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Category : ITableDataBase
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
}