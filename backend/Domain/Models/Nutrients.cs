using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Models;

public class Nutrients : ITableDataBase
{
    public int? Id { get; set; }
    [JsonPropertyName("menu_item_id")] public int MenuItemId { get; set; }
    public int? Kilocalories { get; set; }
    public int? Proteins { get; set; }
    public int? Fats { get; set; }
    public int? Carbohydrates { get; set; }

    public Nutrients(){}
    public Nutrients(int? id, int menuItemId, int? kilocalories, int? proteins, int? fats, int? carbohydrates)
    {
        Id = id;
        MenuItemId = menuItemId;
        Kilocalories = kilocalories;
        Proteins = proteins;
        Fats = fats;
        Carbohydrates = carbohydrates;
    }
}