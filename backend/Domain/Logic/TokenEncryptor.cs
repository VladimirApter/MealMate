using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Domain.Logic;
public static class TokenEncryptor
{
    private static readonly byte[] key; // 32 байта для AES-256
    private static readonly byte[] iv;  // 16 байт для AES

    static TokenEncryptor()
    {
        // Генерируем ключ и IV нужной длины
        using var aes = Aes.Create();
        aes.KeySize = 256; // Используем AES-256 для максимальной безопасности
        aes.GenerateKey();
        aes.GenerateIV();
        key = aes.Key;
        iv = aes.IV;
    }

    // Метод для генерации токена для стола (возвращает URL-friendly строку)
    public static string GenerateToken(int? tableId, int restaurantId)
    {
        var data = $"{tableId}:{restaurantId}";
        return ToUrlSafeBase64(Encrypt(data));
    }

    // Метод для дешифрования URL-friendly токена
    public static (int TableId, int RestaurantId) DecryptToken(string token)
    {
        return Decrypt(FromUrlSafeBase64(token));
    }

    // Вспомогательный метод шифрования
    private static byte[] Encrypt(string data)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        using (var swEncrypt = new StreamWriter(csEncrypt))
            swEncrypt.Write(data);
        return msEncrypt.ToArray();
    }

    // Вспомогательный метод дешифрования
    private static (int TableId, int RestaurantId) Decrypt(byte[] encryptedData)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(encryptedData);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        var decrypted = srDecrypt.ReadToEnd();
        var parts = decrypted.Split(':');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out var restaurantId) &&
            int.TryParse(parts[1], out var tableId))
            return (restaurantId, tableId);
        throw new FormatException("Invalid token format.");
    }

    // Преобразует байты в URL-безопасный Base64
    private static string ToUrlSafeBase64(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    // Преобразует URL-безопасный Base64 обратно в байты
    private static byte[] FromUrlSafeBase64(string urlSafeBase64)
    {
        var base64 = urlSafeBase64
            .Replace("-", "+")
            .Replace("_", "/");
        var padding = 4 - base64.Length % 4;
        if (padding < 4) base64 = base64.PadRight(base64.Length + padding, '=');
        return Convert.FromBase64String(base64);
    }
}