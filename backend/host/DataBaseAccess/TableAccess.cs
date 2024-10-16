using host.Models;

namespace host.DataBaseAccess;

public class TableAccess : DataBaseAccess
{
    private const string TableQuery = "SELECT number FROM tables WHERE id = @tableId";
    public static Table? GetTable(int id)
    {
        using var tableReader = ExecuteReader(TableQuery, ("@tableId", id));
        
        if (!tableReader.Read()) return null;

        return new Table(
            tableReader.GetInt32(0)
        );
    }
}