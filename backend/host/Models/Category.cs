using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Category : ITableDataBase
{
    [JsonPropertyName("menu_id")]
    public int MenuId { get; set; }
    public int? Id { get; set; }
    public string Name { get; set; }
    
    [JsonPropertyName("menu_items")]
    public List<MenuItem>? MenuItems { get; set; }

    public Category(string name, int? id, int menuId, List<MenuItem>? menuItems)
    {
        Id = id;
        Name = name;
        MenuItems = menuItems;
        MenuId = menuId;
    }
}