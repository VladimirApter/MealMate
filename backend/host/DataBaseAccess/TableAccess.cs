using System.Data.SQLite;
using host.Models;

namespace host.DataBaseAccess;

public class TableAccess : DataBaseAccess
{
    private const string TableQuery = "SELECT number, id, restaurant_id FROM tables WHERE id = @tableId";

    private const string InsertCommand =
        @"INSERT INTO [tables] (restaurant_id, number) 
             VALUES (@restaurantId, @number);";

    private const string UpdateCommand = @"UPDATE [tables] 
             SET restaurant_id = @restaurantId, number = @number 
             WHERE Id = @id;";

    public static Table? GetTable(int id)
    {
        using var tableReader = ExecuteReader(TableQuery, ("@tableId", id));

        if (!tableReader.Read()) return null;

        return new Table(
            tableReader.GetInt32(0),
            tableReader.GetInt32(1),
            tableReader.GetInt32(2)
        );
    }

    public static void AddOrUpdateTable(Table table)
    {
        AddOrUpdateObject(table, InsertCommand, UpdateCommand, (tableObj, insertOrUpdateCommand) =>
        {
            insertOrUpdateCommand.Parameters.AddWithValue("@restaurantId", tableObj.RestaurantId);
            insertOrUpdateCommand.Parameters.AddWithValue("@number", tableObj.Number);
        });
    }
}