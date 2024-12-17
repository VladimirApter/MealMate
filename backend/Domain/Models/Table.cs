using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Domain.Logic;

namespace Domain.Models;

public class Table : ITableDataBase
{
    public long? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public long RestaurantId { get; set; }
    public int Number { get; set; }
    public string Token { get; set; }
    [JsonPropertyName("qr_code_image_path")] public string QRCodeImagePath { get; set; }

    public Table(){}
    public Table(long? id, long restaurantId, int number)
    {
        Id = id;
        RestaurantId = restaurantId;
        Number = number;
        GenerateTokenAndQRCode();
    }

    private void GenerateTokenAndQRCode()
    {
        Token = TokenEncryptor.GenerateToken(Id, RestaurantId);
        QRCodeImagePath = TableQrCode.GenerateAndSaveQrCode(Number, Token);
    }
}