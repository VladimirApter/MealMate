using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Domain.Logic;

namespace Domain.Models;

public class Table : ITableDataBase
{
    public long? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public long RestaurantId { get; init; }
    public int Number { get; init; }
    public string Token { get; set; }
    [JsonPropertyName("qr_code_image_path")] public string QrCodeImagePath { get; set; }

    public Table(){}
    public Table(long? id, long restaurantId, int number)
    {
        Id = id;
        RestaurantId = restaurantId;
        Number = number;
        GenerateTokenAndQrCode();
    }

    private void GenerateTokenAndQrCode()
    {
        Token = TokenEncryptor.GenerateToken(Id, RestaurantId);
        QrCodeImagePath = TableQrCode.GenerateAndSaveQrCode(Number, Token);
    }
}