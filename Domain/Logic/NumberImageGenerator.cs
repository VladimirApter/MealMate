using SkiaSharp;
using System;
using System.Runtime.InteropServices;

namespace Domain.Logic;

public static class NumberImageGenerator
{
    public static SKBitmap Generate(int number)
    {
        var bitmap = new SKBitmap(240, 240);

        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.Transparent);

        var numberText = number.ToString();

        var fontSize = CalculateFontSize(numberText.Length);
        using var font = new SKFont(SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright), fontSize);
        using var paint = new SKPaint();
        paint.Color = SKColors.Black;
        paint.IsAntialias = true;

        var textWidth = font.MeasureText(MemoryMarshal.Cast<char, ushort>(numberText.AsSpan()));
        var textHeight = font.Metrics.Descent - font.Metrics.Ascent;

        var x = (bitmap.Width - textWidth) / 2 + 10 * textWidth / 115;
        var y = (bitmap.Height - textHeight) / 2;

        const float borderThickness = 7;
        DrawRoundedRectangle(canvas, new SKRect(0, 0, bitmap.Width, bitmap.Height), 50, SKColors.Black);
        DrawRoundedRectangle(canvas, new SKRect(borderThickness, borderThickness, bitmap.Width - borderThickness, bitmap.Height - borderThickness), 43, SKColors.White);
        canvas.DrawText(numberText, x, y - font.Metrics.Ascent, font, paint);

        return bitmap;
    }

    private static float CalculateFontSize(int digitCount)
    {
        const float baseFontSize = 150;

        if (digitCount == 1)
            return baseFontSize;
        return baseFontSize / (digitCount * 0.55f);
    }

    private static void DrawRoundedRectangle(SKCanvas canvas, SKRect rect, float cornerRadius, SKColor fillColor)
    {
        using var path = new SKPath();
        path.AddRoundRect(rect, cornerRadius, cornerRadius);

        using var paint = new SKPaint();
        paint.Color = fillColor;
        paint.Style = SKPaintStyle.Fill;

        canvas.DrawPath(path, paint);
    }
}