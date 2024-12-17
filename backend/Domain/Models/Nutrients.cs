using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Models;

public class Nutrients : ITableDataBase
{
    public long? Id { get; set; }
    [JsonPropertyName("menu_item_id")] public long? MenuItemId { get; set; }
    public int? Kilocalories { get; init; }
    public int? Proteins { get; init; }
    public int? Fats { get; init; }
    public int? Carbohydrates { get; init; }

    public Nutrients(){}
    public Nutrients(long? id, int? kilocalories, int? proteins, int? fats, int? carbohydrates)
    {
        Id = id;
        Kilocalories = kilocalories;
        Proteins = proteins;
        Fats = fats;
        Carbohydrates = carbohydrates;
    }
}