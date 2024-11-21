using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace Domain.Logic;

public static partial class HostsUrlGetter
{
    public static string ApiUrl;
    public static string ApplicationUrl;

    public static void Setup()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var apiPath = Path.GetFullPath(Path.Combine(currentDirectory, "../API/API.http"));
        var applicationPath = Path.GetFullPath(Path.Combine(currentDirectory, "../Application/Application.http"));

        var apiContent = File.ReadAllText(apiPath);
        var apiMatch = MyRegex().Match(apiContent);

        if (apiMatch.Success)
            ApiUrl = apiMatch.Groups[1].Value.Trim();

        var applicationContent = File.ReadAllText(applicationPath);
        var applicationMatch = MyRegex().Match(applicationContent);

        if (applicationMatch.Success)
            ApplicationUrl = applicationMatch.Groups[1].Value.Trim();
    }

    [GeneratedRegex(@"@\w+_HostAddress\s*=\s*(.+)")]
    private static partial Regex MyRegex();
}