using System.Data.SQLite;

namespace host.DataBaseAccess;

public class DataBaseAccess
{
    public const string PathDataBase = "../DataBase/DataBase.bd3";
    private const string ConnectionString = $"Data Source={PathDataBase}";

    protected static SQLiteDataReader ExecuteReader(string itemQuery, (string, int) parameter)
    {
        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        var command = new SQLiteCommand(itemQuery, connection);
        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
        return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection); 
    }
}