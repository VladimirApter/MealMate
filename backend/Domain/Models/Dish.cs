using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Models;

public class Dish : MenuItem, ITableDataBase
{
    public double Weight { get; init; }
    
    public Dish(){}
    public Dish(long? id,
        long categoryId,
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