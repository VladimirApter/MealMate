using System.Drawing;
using System.Drawing.Drawing2D;

namespace Domen.Logic;

public static class NumberImageGenerator
{
    public static Bitmap Generate(int number)
    {
        var bitmap = new Bitmap(240, 240);

        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Transparent);

        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

        var numberText = number.ToString();

        var fontSize = CalculateFontSize(numberText.Length);
        var font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        var brush = Brushes.Black;

        var textSize = graphics.MeasureString(numberText, font);

        var x = (bitmap.Width - textSize.Width) / 2;
        var y = (bitmap.Height - textSize.Height) / 2 + CalculateExtraVerticalOffset(numberText.Length);

        DrawRoundedRectangle(graphics, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            100, Brushes.White, Pens.Transparent);

        graphics.DrawString(numberText, font, brush, x, y);

        return bitmap;
    }

    private static float CalculateFontSize(int digitCount)
    {
        const float baseFontSize = 150;

        if (digitCount == 1)
            return baseFontSize;
        return baseFontSize / (digitCount * 0.55f);
    }
    
    private static float CalculateExtraVerticalOffset(int digitCount)
    {
        const float baseVerticalOffset = 10;

        return digitCount switch
        {
            1 => baseVerticalOffset,
            2 => baseVerticalOffset * 0.7f,
            _ => baseVerticalOffset * 0.3f
        };
    }

    private static void DrawRoundedRectangle(Graphics graphics, Rectangle rectangle, int cornerRadius, Brush fillBrush, Pen outlinePen)
    {
        using var path = new GraphicsPath();
        path.AddArc(rectangle.X, rectangle.Y, cornerRadius, cornerRadius, 180, 90);
        path.AddArc(rectangle.Right - cornerRadius, rectangle.Y, cornerRadius, cornerRadius, 270, 90);
        path.AddArc(rectangle.Right - cornerRadius, rectangle.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90);
        path.AddArc(rectangle.X, rectangle.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90);
        path.CloseFigure();

        graphics.FillPath(fillBrush, path);
        graphics.DrawPath(outlinePen, path);
    }
}