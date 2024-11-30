using System.Runtime.CompilerServices;

namespace Domain.Logic;

public static class DataBasePathGetter
{
    public static readonly string DataBasePath;
    
    static DataBasePathGetter()
    {
        DataBasePath = Environment.GetEnvironmentVariable("DATABASE_PATH");
        if (DataBasePath != null)
            return;
        
        var currentDirectory = Directory.GetCurrentDirectory();
        var dataBasePath = Path.Combine(currentDirectory, "../DataBase");
        DataBasePath = dataBasePath;
    }
}