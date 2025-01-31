using System.Text;
using System.Text.Json;

namespace host;

public static class ForwardToPythonServer
{
    private static readonly JsonSerializerOptions OptionsSerializer = new()
        { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    public static async Task ForwardObject<T>(T? obj, string url)
    {
        if (obj == null) return;

        var jsonContent = JsonSerializer.Serialize(obj, OptionsSerializer);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to forward {nameof(T)} to Python: {response.StatusCode}");
        }
    }
}