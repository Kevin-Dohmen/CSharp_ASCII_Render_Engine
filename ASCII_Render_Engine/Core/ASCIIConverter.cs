﻿using ASCII_Render_Engine.Types.Vectors;

namespace ASCII_Render_Engine.Core;

public class ASCIIConverter
{
    private static readonly char[] chars = [' ', '.', ':', ';', '+', '=', 'x', 'X', '$'];
    //private static readonly char[] chars = { ' ', '.', ':', '-', '=', '+', '*', '#', '%', '&', '$' };

    private static readonly double[,] ditherMatrix = new double[,]
    {
        { -0.25, 0.25 },
        { 0.5, -0.5 }
    };

    public static Display BufferToFullScreen(ScreenBuffer screenBuffer, Display display, ScreenConfig config)
    {
        int width = screenBuffer.Width;
        int height = screenBuffer.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char tmpChar;
                if (config.Dithering)
                {
                    tmpChar = CharFromColorDither(screenBuffer.Buffer[y][x], x, y);
                }
                else
                {
                    tmpChar = CharFromColor(screenBuffer.Buffer[y][x]);
                }
                display.SetChar(x, y, tmpChar);
            }
        }

        return display;
    }

    public static char CharFromColor(Vec2 color)
    {
        double lum = Math.Clamp(color.x * color.y, 0, 1);
        return chars[(int)Math.Round(lum * (chars.Length - 1))];
    }

    public static char CharFromColorDither(Vec2 color, int x, int y)
    {
        double lum = Math.Clamp(color.x * color.y, 0, 1);

        // Calculate the step size based on the color depth (number of characters)
        double lumStep = 1.0 / (chars.Length - 1);

        // Scale the dither value by a fraction of lumStep for smooth transitions
        double threshold = ditherMatrix[y % 2, x % 2] * (lumStep / 2.0);

        // Apply the scaled threshold for adaptive dithering
        lum = Math.Clamp(lum + threshold, 0, 1);

        // Map adjusted luminance to the closest character
        return chars[(int)Math.Round(lum * (chars.Length - 1))];
    }
}
