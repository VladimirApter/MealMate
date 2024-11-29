using Domain.DataBaseAccess;

namespace Domain.Models;

public class Drink : MenuItem, ITableDataBase
{
    public double Volume { get; set; }
    
    public Drink(){}
    public Drink(long? id,
        long categoryId, 
        int cookingTimeMinutes,
        double price,
        double volume,
        string name, 
        string description,
        string? imagePath,
        Nutrients? nutrients) : base(id, categoryId, cookingTimeMinutes, price, name, description, imagePath, nutrients)
    {
        Volume = volume;
    }
}