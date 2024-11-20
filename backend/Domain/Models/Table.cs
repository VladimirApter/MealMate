using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Domain.Logic;

namespace Domain.Models;

public class Table : ITableDataBase
{
    public int? Id { get; set; }
    [JsonPropertyName("restaurant_id")] public int RestaurantId { get; set; }
    public int Number { get; set; }
    public string? Token { get; set; }
    [JsonPropertyName("qr_code_image_path")] public string? QRCodeImagePath { get; set; }
    
    public Table(){}
    public Table(int? id, int restaurantId, int number)
    {
        Id = id;
        RestaurantId = restaurantId;
        Number = number;
        Token = TokenEncryptor.GenerateToken(Id, RestaurantId);
        QRCodeImagePath = TableQrCode.GenerateAndSaveQrCode(Number, Token);
    }
}