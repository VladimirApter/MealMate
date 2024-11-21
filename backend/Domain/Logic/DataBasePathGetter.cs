using System.Runtime.CompilerServices;

namespace Domain.Logic;

public static class DataBasePathGetter
{
    public static string DataBasePath;

    public static void Setup()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var dataBasePath = Path.GetFullPath(Path.Combine(currentDirectory, "../DataBase"));
        
        DataBasePath = dataBasePath;
    }
}