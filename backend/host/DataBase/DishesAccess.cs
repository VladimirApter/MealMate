using host.Models;
namespace host.DataBase;

public class DishesAccess : DataBaseAccess
{
    private const string DishQuery = "SELECT price, weight, name, description, cooking_time_minutes FROM dishes WHERE id = @dishId";
    public static Dish? GetDish(int id)
    {
        using var dishReader = ExecuteReader(DishQuery, ("@dishId", id));
        
        if (!dishReader.Read()) return null;

        return new Dish(
            dishReader.GetDouble(0),
            dishReader.GetDouble(1),
            dishReader.GetString(2),
            dishReader.GetString(3),
            dishReader.GetInt32(4)
        );
    }

    
}