using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Category : ITableDataBase
{
    public int? Id { get; set; }
    [JsonPropertyName("menu_id")]
    public int MenuId { get; set; }
    public string Name { get; set; }
    
    [JsonPropertyName("menu_items")]
    public List<MenuItem>? MenuItems { get; set; }

    public Category(int? id, int menuId, string name, List<MenuItem>? menuItems)
    {
        Id = id;
        MenuId = menuId;
        Name = name;
        MenuItems = menuItems;
    }
}