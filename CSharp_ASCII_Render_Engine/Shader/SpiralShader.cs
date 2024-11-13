﻿using CSharp_ASCII_Render_Engine.Types.Pixels;
using CSharp_ASCII_Render_Engine.Types.Vectors;

namespace CSharp_ASCII_Render_Engine.Shader
{
    public class SpiralShader : IShader
    {
        public string Name { get; } = "Spiral Shader";

        // ShaderSettings
        public double TimeOffset { get; set; }

        public SpiralShader()
        {
            TimeOffset = 0;
        }

        public SpiralShader(double timeOffset)
        {
            TimeOffset = timeOffset;
        }

        // Source: ChatGPT
        public Vec2 Render(ShaderPixel shaderPixel)
        {
            Vec2 col = shaderPixel.Vec2Pool.GetObject().reset();
            Vec2 uv = shaderPixel.UV;
            double frame = (double)shaderPixel.Frame;
            double time = shaderPixel.Time + TimeOffset;

            col.y = 1;

            // Shift uv to center for a spiral effect
            uv.AddInPlace(-0.5, -0.5);

            // Compute angle and radius from center
            double angle = Math.Atan2(uv.y, uv.x);
            double radius = Math.Sqrt(uv.x * uv.x + uv.y * uv.y);

            // Create a rotating spiral effect
            col.x = (Math.Sin(10 * radius - time * 0.2 + angle) + 1) / 2;

            shaderPixel.Vec2Pool.ReturnObject(col);
            return col;
        }

    }
}
