using System.Diagnostics.CodeAnalysis;
using SkiaSharp;
using QRCoder;

namespace Domain.Logic;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class TableQrCode
{
    private static readonly string applicationBaseUrl = HostsUrlGetter.ApplicationUrl;
    private static readonly string qrcodeDataBasePath;

    static TableQrCode()
    {
        var databasePath = DataBasePathGetter.DataBasePath;
        qrcodeDataBasePath = Path.Combine(databasePath, "QRCodeImages");
    }

    public static string GenerateAndSaveQrCode(int tableNumber, string tableToken)
    {
        var url = $"{applicationBaseUrl}/order/{tableToken}";
        var logo = NumberImageGenerator.Generate(tableNumber);

        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCode.GetGraphic(20);

        using var ms = new MemoryStream(qrCodeBytes);
        var qrCodeImage = SKBitmap.Decode(ms);

        var convertedQrCodeImage = new SKBitmap(qrCodeImage.Width, qrCodeImage.Height);
        using (var canvas = new SKCanvas(convertedQrCodeImage))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.DrawBitmap(qrCodeImage, 0, 0);
        }

        var convertedLogo = new SKBitmap(logo.Width, logo.Height);
        using (var canvas = new SKCanvas(convertedLogo))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.DrawBitmap(logo, 0, 0);
        }

        using (var canvas = new SKCanvas(convertedQrCodeImage))
        {
            var logoSize = convertedQrCodeImage.Width / 5;
            var x = (convertedQrCodeImage.Width - logoSize) / 2;
            var y = (convertedQrCodeImage.Height - logoSize) / 2;
            canvas.DrawBitmap(convertedLogo, new SKRect(x, y, x + logoSize, y + logoSize));
        }

        return SaveQrCode(convertedQrCodeImage);
    }

    private static string SaveQrCode(SKBitmap bitmap)
    {
        if (!Directory.Exists(qrcodeDataBasePath))
            Directory.CreateDirectory(qrcodeDataBasePath);

        var uniqueFileName = $"{Guid.NewGuid()}.png";
        var filePath = Path.Combine(qrcodeDataBasePath, uniqueFileName);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(filePath);
        data.SaveTo(stream);

        return uniqueFileName;
    }
}