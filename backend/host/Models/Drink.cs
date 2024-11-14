using host.DataBaseAccess;

namespace host.Models;

public class Drink : MenuItem, ITableDataBase
{
    public double Volume { get; set; }
    
    public Drink(int? id,
        int categoryId, 
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