using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;


namespace Domen.Logic;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class TableQrCode
{
    private static readonly string siteBaseUrl;
    private static readonly string qrcodeDataBasePath;
    
    static TableQrCode()
    {
        siteBaseUrl = HostsUrlGetter.GetHostUrl("Application.http");
        
        var databasePath = DataBasePathGetter.GetAbsoluteDataBasePath();
        qrcodeDataBasePath = Path.Combine(databasePath, "QRCodeImages");
    }

    public static string GenerateAndSaveQrCode(int tableNumber, string tableToken)
    {
        var url = $"{siteBaseUrl}/{tableToken}";
        var logo = NumberImageGenerator.Generate(tableNumber);
        
        
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCode.GetGraphic(20);
        
        using var ms = new MemoryStream(qrCodeBytes);
        var qrCodeImage = new Bitmap(ms);
        
        var convertedQrCodeImage = new Bitmap(qrCodeImage.Width, qrCodeImage.Height, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(convertedQrCodeImage))
            graphics.DrawImage(qrCodeImage, 0, 0);
        
        var convertedLogo = new Bitmap(logo.Width, logo.Height, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(convertedLogo))
            graphics.DrawImage(logo, 0, 0);

        using (var graphics = Graphics.FromImage(convertedQrCodeImage))
        {
            var logoSize = convertedQrCodeImage.Width / 5;
            var x = (convertedQrCodeImage.Width - logoSize) / 2;
            var y = (convertedQrCodeImage.Height - logoSize) / 2;
            graphics.DrawImage(convertedLogo, x, y, logoSize, logoSize);
        }

        return SaveQrCode(convertedQrCodeImage);
    }

    private static string SaveQrCode(Bitmap bitmap)
    {
        if (!Directory.Exists(qrcodeDataBasePath))
            Directory.CreateDirectory(qrcodeDataBasePath);

        var uniqueFileName = $"{Guid.NewGuid()}.png";
        var filePath = Path.Combine(qrcodeDataBasePath, uniqueFileName);

        bitmap.Save(filePath, ImageFormat.Png);

        return filePath;
    }
}