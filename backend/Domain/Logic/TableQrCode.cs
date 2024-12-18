using System.Diagnostics.CodeAnalysis;
using SkiaSharp;
using QRCoder;

namespace Domain.Logic;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class TableQrCode
{
    private static readonly string applicationBaseUrl = HostsUrlGetter.ApplicationUrl;
    private static readonly string qrcodeDataBasePath;
    private static readonly string templatePath;

    static TableQrCode()
    {
        var databasePath = DataBasePathGetter.DataBasePath;
        qrcodeDataBasePath = Path.Combine(databasePath, "QRCodeImages");
        templatePath = "qr_code_card_template.png";
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

        var roundedQrCodeImage = CreateRoundedBitmap(convertedQrCodeImage, 55);

        var finalImage = CombineWithTemplate(roundedQrCodeImage);

        return SaveQrCode(finalImage);
    }

    private static SKBitmap CreateRoundedBitmap(SKBitmap bitmap, float cornerRadius)
    {
        var roundedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
        using (var canvas = new SKCanvas(roundedBitmap))
        {
            canvas.Clear(SKColors.Transparent);

            var path = new SKPath();
            var rect = new SKRect(0, 0, bitmap.Width, bitmap.Height);
            path.AddRoundRect(rect, cornerRadius, cornerRadius);

            canvas.ClipPath(path);
            canvas.DrawBitmap(bitmap, 0, 0);
        }

        return roundedBitmap;
    }

    private static SKBitmap CombineWithTemplate(SKBitmap qrCodeImage)
    {
        using var templateStream = File.OpenRead(templatePath);
        var templateImage = SKBitmap.Decode(templateStream);

        var combinedBitmap = new SKBitmap(templateImage.Width, templateImage.Height);
        using (var canvas = new SKCanvas(combinedBitmap))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.DrawBitmap(templateImage, 0, 0);

            var qrCodeSize = Math.Min(templateImage.Width, templateImage.Height) * 0.6f;
            var x = (templateImage.Width - qrCodeSize) / 2;
            var y = (templateImage.Height - qrCodeSize) / 2;
            canvas.DrawBitmap(qrCodeImage, new SKRect(x, y, x + qrCodeSize, y + qrCodeSize));
        }

        return combinedBitmap;
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
