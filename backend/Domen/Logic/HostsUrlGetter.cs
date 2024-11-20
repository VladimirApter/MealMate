using System.Text.RegularExpressions;

namespace Domen.Logic;

public static partial class HostsUrlGetter
{
    public static string GetHostUrl(string hostDataFileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var hostDataFilePath = hostDataFileName switch
        {
            "API.http" => Path.GetFullPath(Path.Combine(currentDirectory, hostDataFileName)),
            "Application.http" => Path.GetFullPath(Path.Combine(currentDirectory, $"../Application/{hostDataFileName}")),
            _ => throw new ArgumentException($"Unknown API data file name: {hostDataFileName}")
        };

        if (!File.Exists(hostDataFilePath))
            throw new FileNotFoundException($"File not found: {hostDataFilePath}");

        var content = File.ReadAllText(hostDataFilePath);
        var match = MyRegex().Match(content);

        if (match.Success)
            return match.Groups[1].Value.Trim();

        throw new InvalidOperationException($"No valid API address found in the file {hostDataFilePath}");
    }

    [GeneratedRegex(@"@\w+_HostAddress\s*=\s*(.+)")]
    private static partial Regex MyRegex();
}