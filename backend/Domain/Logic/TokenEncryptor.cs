using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Logic;
public static class TokenEncryptor
{
    private const string SecretKey = "HBIkdLEL7MFGvvVwteis9n3gkcabBpFJ";
    
    //метод для генерации токена
    public static string GenerateToken(long? tableId, long restaurantId)
    {
        return Encrypt(tableId, restaurantId);
    }
    
    //метод для шифровки
    public static string Encrypt(long? tableId, long restaurantId)
    {
        var data = $"{tableId}:{restaurantId}";
        using var aes = Aes.Create();

        aes.Key = Encoding.UTF8.GetBytes(SecretKey.PadRight(32).Substring(0, 32));
        aes.IV = new byte[16];

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(data);
        }

        return Convert.ToBase64String(ms.ToArray())
                        .Replace("+", "-")
                        .Replace("/", "_")
                        .TrimEnd('=');
    }
    
    //метод для дешифровки
    public static (long TableId, long RestaurantId) DecryptToken(string token)
    {
        var fixedToken = token.Replace("-", "+").Replace("_", "/");
        while (fixedToken.Length % 4 != 0) 
            fixedToken += "=";

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(SecretKey.PadRight(32).Substring(0, 32));
        aes.IV = new byte[16];

        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream(Convert.FromBase64String(fixedToken)))
        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        using (var reader = new StreamReader(cs))
        {
            string decryptedData = reader.ReadToEnd();
            string[] parts = decryptedData.Split(':');
            if (parts.Length != 2)
                throw new FormatException("Invalid token format.");

            var restaurantId = long.Parse(parts[1]);
            var tableId = long.Parse(parts[0]);

            return (tableId, restaurantId);
        }
    }
}