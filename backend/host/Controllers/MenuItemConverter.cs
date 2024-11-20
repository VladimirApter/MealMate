using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using host.Models;

public class MenuItemConverter : JsonConverter<MenuItem>
{
    public override MenuItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Читаем весь JSON в JsonDocument для анализа
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        // Определяем, какой тип использовать, проверяя поля
        if (root.TryGetProperty("Weight", out _) || root.TryGetProperty("weight", out _))
            return JsonSerializer.Deserialize<Dish>(root.GetRawText(), options);

        if (root.TryGetProperty("Volume", out _) || root.TryGetProperty("volume", out _))
            return JsonSerializer.Deserialize<Drink>(root.GetRawText(), options);

        throw new JsonException("Не удалось определить тип MenuItem.");
    }

    public override void Write(Utf8JsonWriter writer, MenuItem value, JsonSerializerOptions options)
    {
        // Сериализуем с учетом реального типа
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}