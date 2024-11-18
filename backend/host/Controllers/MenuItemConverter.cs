using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using host.Models;

public class MenuItemConverter : JsonConverter<MenuItem>
{
    public override MenuItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Читаем весь JSON в JsonDocument для анализа
        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = document.RootElement;

            // Определяем, какой тип использовать, проверяя поля
            if (root.TryGetProperty("Weight", out JsonElement weightProperty))
            {
                // Это объект Dish, десериализуем его
                return JsonSerializer.Deserialize<Dish>(root.GetRawText(), options);
            }
            else if (root.TryGetProperty("Volume", out JsonElement volumeProperty))
            {
                // Это объект Drink, десериализуем его
                return JsonSerializer.Deserialize<Drink>(root.GetRawText(), options);
            }
            else
            {
                throw new JsonException("Не удалось определить тип MenuItem.");
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, MenuItem value, JsonSerializerOptions options)
    {
        // Сериализуем с учетом реального типа
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}