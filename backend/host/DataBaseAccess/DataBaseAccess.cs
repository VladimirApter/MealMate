using System.Data.SQLite;
using host.Models;

namespace host.DataBaseAccess;

public interface ITableDataBase
{
    public int? Id { get; set; }
}

public class DataBaseAccess
{
    public const string PathDataBase = "./DataBase/DataBase.bd3";
    private const string ConnectionString = $"Data Source={PathDataBase}";

    protected static (SQLiteCommand, SQLiteConnection) OpenDataBase(string itemQuery)
    {
        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        return (new SQLiteCommand(itemQuery, connection), connection);
    }

    protected static SQLiteDataReader ExecuteReader(string itemQuery, (string, int) parameter)
    {
        var command = OpenDataBase(itemQuery).Item1;
        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
        return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
    }

    protected static void AddOrUpdateObject<T>(T obj, string insertCommand, string updateCommand,
        Action<T, SQLiteCommand> addParameters) where T : ITableDataBase
    {
        var (insertOrUpdateCommand, connection) = OpenDataBase(obj.Id == null ? insertCommand : updateCommand);

        if (obj is TgAccount)
        {
            var checkCommand = new SQLiteCommand();
            if (obj is Owner)
            {
                checkCommand = new SQLiteCommand($"SELECT COUNT({obj.Id}) FROM owners WHERE id = @id", connection);
            }

            if (obj is NotificationGetter)
            {
                checkCommand = new SQLiteCommand($"SELECT COUNT({obj.Id}) FROM notification_getters WHERE id = @id", connection);
            }
            
            checkCommand.Parameters.AddWithValue("@id", obj.Id);

            var exists = (long)checkCommand.ExecuteScalar() > 0; 

            if (exists)
            {
                insertOrUpdateCommand.CommandText = updateCommand;
            }
            else
            {
                insertOrUpdateCommand.CommandText = insertCommand;
            }
        }


        if (obj.Id != null)
            insertOrUpdateCommand.Parameters.AddWithValue("@id", obj.Id);
        addParameters(obj, insertOrUpdateCommand);
        insertOrUpdateCommand.ExecuteNonQuery();

        if (obj.Id != null) return;

        obj.Id = (int)(long)new SQLiteCommand("SELECT last_insert_rowid()", connection).ExecuteScalar();
    }
}