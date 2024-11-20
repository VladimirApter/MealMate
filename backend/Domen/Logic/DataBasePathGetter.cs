using System.Runtime.CompilerServices;

namespace Domen.Logic;

public static class DataBasePathGetter
{
    public static string GetAbsoluteDataBasePath([CallerFilePath] string callerFilePath = "")
    {
        var currentDirectory = Path.GetDirectoryName(callerFilePath);
        var baseDirectory = Directory.GetParent(currentDirectory).FullName;
        var dataBasePath = Path.Combine(baseDirectory, "DataBase");

        return dataBasePath;
    }
}