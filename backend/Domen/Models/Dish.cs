using System.Text.Json.Serialization;
using Domen.DataBaseAccess;

namespace Domen.Models;

public class Dish : MenuItem, ITableDataBase
{
    public double Weight { get; set; }
    
    public Dish(){}
    public Dish(int? id,
        int categoryId,
        int cookingTimeMinutes,
        double price, 
        double weight,
        string name, 
        string description,
        string? imagePath,
        Nutrients? nutrients) : base(id, categoryId, cookingTimeMinutes,price, name, description, imagePath, nutrients)
    {
        Weight = weight;
    }
}