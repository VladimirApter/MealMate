using System.Data.SQLite;
using host.Models;

namespace host.DataBaseAccess;

public class DishesAccess : DataBaseAccess
{
    private const string DishQuery =
        "SELECT price, weight, name, description, cooking_time_minutes, id, category_id FROM dishes WHERE id = @dishId";

    private const string InsertCommand =
        @"INSERT INTO [dishes] (category_id, price, weight, name, description, cooking_time_minutes) 
             VALUES (@categoryId, @price, @weight, @name, @description, @cookingTimeMinutes);";

    private const string UpdateCommand = @"UPDATE [dishes] 
             SET category_id = @categoryId, price = @price, weight = @weight, 
                 name = @name, description = @description, cooking_time_minutes = @cookingTimeMinutes 
             WHERE Id = @id;";

    public static Dish? GetDish(int id)
    {
        using var dishReader = ExecuteReader(DishQuery, ("@dishId", id));

        if (!dishReader.Read()) return null;

        return new Dish(
            dishReader.GetInt32(5),
            dishReader.GetDouble(0),
            dishReader.GetDouble(1),
            dishReader.GetString(2),
            dishReader.GetString(3),
            dishReader.GetInt32(4),
            dishReader.GetInt32(6)
        );
    }

    public static void AddOrUpdateDish(Dish dish)
    {
        AddOrUpdateObject(dish, InsertCommand, UpdateCommand, (dishObj, insertOrUpdateCommand) =>
        {
            insertOrUpdateCommand.Parameters.AddWithValue("@categoryId", dishObj.CategoryId);
            insertOrUpdateCommand.Parameters.AddWithValue("@price", dishObj.Price);
            insertOrUpdateCommand.Parameters.AddWithValue("@weight", dishObj.Weight);
            insertOrUpdateCommand.Parameters.AddWithValue("@name", dishObj.Name);
            insertOrUpdateCommand.Parameters.AddWithValue("@description", dishObj.Description);
            insertOrUpdateCommand.Parameters.AddWithValue("@cookingTimeMinutes", dishObj.CookingTimeMinutes);
        });
    }
}