using System.Data.SQLite;
namespace host.DataBase;

public class DataBaseAccess
{
    private const string ConnectionString =
        @"Data Source=C:\Users\yurar\RiderProjects\MealMate\backend\host\DataBase\DataBase.bd3;";

    protected static SQLiteDataReader ExecuteReader(string itemQuery, (string, int) parameter)
    {
        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        var command = new SQLiteCommand(itemQuery, connection);
        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
        return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection); 
    }
}