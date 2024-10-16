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
        var (insertOrUpdateCommand, connection) = OpenDataBase(dish.Id == null ? InsertCommand : UpdateCommand);

        if (dish.Id != null)
            insertOrUpdateCommand.Parameters.AddWithValue("@id", dish.Id);

        insertOrUpdateCommand.Parameters.AddWithValue("@categoryId", dish.CategoryId);
        insertOrUpdateCommand.Parameters.AddWithValue("@price", dish.Price);
        insertOrUpdateCommand.Parameters.AddWithValue("@weight", dish.Weight);
        insertOrUpdateCommand.Parameters.AddWithValue("@name", dish.Name);
        insertOrUpdateCommand.Parameters.AddWithValue("@description", dish.Description);
        insertOrUpdateCommand.Parameters.AddWithValue("@cookingTimeMinutes", dish.CookingTimeMinutes);
        insertOrUpdateCommand.ExecuteNonQuery();

        if (dish.Id != null) return;

        dish.Id = (int)(long)new SQLiteCommand("SELECT last_insert_rowid()", connection).ExecuteScalar();
    }
}