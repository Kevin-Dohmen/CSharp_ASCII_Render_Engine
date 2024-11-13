﻿using CSharp_ASCII_Render_Engine.Types.Pixels;
using CSharp_ASCII_Render_Engine.Types.Vectors;

namespace CSharp_ASCII_Render_Engine.Shader
{
    public class ModShader : IShader
    {
        public string Name { get; } = "Modulo Shader";

        // Shader Settings
        public double TimeOffset { get; set; }

        public ModShader()
        {
            TimeOffset = 0;
        }

        public ModShader(double timeOffset)
        {
            TimeOffset = timeOffset;
        }

        public Vec2 Render(ShaderPixel shaderPixel)
        {
            Vec2 col = shaderPixel.Vec2Pool.GetObject().reset();
            Vec2 uv = shaderPixel.UV;
            double frame = (double)shaderPixel.Frame;
            double time = shaderPixel.Time + TimeOffset;

            col.y = 1;
            
            // shader
            col.x = ((uv.x+time*0.1)%0.25)*4;

            shaderPixel.Vec2Pool.ReturnObject(col);
            return col;
        }
    }
}
