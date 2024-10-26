using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Dish : MenuItem, ITableDataBase
{
    public double Weight { get; set; }
    
    public Dish(int? id, 
        double price, 
        string name, 
        string description, 
        int cookingTimeMinutes, 
        int categoryId, 
        double weight) : base(id, price, name, description, cookingTimeMinutes, categoryId)
    {
        Weight = weight;
    }
}