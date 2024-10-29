using host.DataBaseAccess;

namespace host.Models;

public class Drink : MenuItem, ITableDataBase
{
    public double Volume;
    
    public Drink(int? id, 
        double price,
        string name, 
        string description, 
        int cookingTimeMinutes, 
        int categoryId, 
        double volume) : base(id, price, name, description, cookingTimeMinutes, categoryId)
    {
        Volume = volume;
    }
}