using System.Text;
using System.Text.Json;

namespace site;

public class ApiClient<T> where T : class
{
    private readonly object @lock = new();
    private static readonly HttpClient HttpClient = new();
    private static readonly string RequestUri = $"http://localhost:5211/api/{typeof(T).Name.ToLower()}";
    private static readonly JsonSerializerOptions OptionsDeserializer = new() { PropertyNameCaseInsensitive = true };

    public T? Get(int id)
    {
        lock (@lock)
        {
            var response = HttpClient.GetAsync($"{RequestUri}/{id}").Result;

            if (!response.IsSuccessStatusCode) return null;

            var jsonData = response.Content.ReadAsStringAsync().Result;
            var obj = JsonSerializer.Deserialize<T>(jsonData, OptionsDeserializer);
            return obj;
        }
    }

    public void Post(T obj)
    {
        lock (@lock)
        {
            var jsonContent = JsonSerializer.Serialize(obj);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = HttpClient.PostAsync($"{RequestUri}", content).Result;
            if (response.IsSuccessStatusCode) return;

            throw new HttpRequestException($"Error posting data: {response.StatusCode}");
        }
    }
}