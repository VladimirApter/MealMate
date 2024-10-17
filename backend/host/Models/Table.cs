using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Table : ITableDataBase
{
    public int? Id { get; set; }
    public int RestaurantId { get; set; }
    public int Number { get; set; }
    public Table(int number, int? id, int restaurantId)
    {
        Number = number;
        Id = id;
        RestaurantId = restaurantId;
    }
}