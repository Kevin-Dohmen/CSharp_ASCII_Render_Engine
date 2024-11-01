﻿using CSharp_ASCII_Render_Engine.Types.Pixels;
using CSharp_ASCII_Render_Engine.Types.Vectors;

namespace CSharp_ASCII_Render_Engine.Shader
{
    public interface IShader
    {
        public Vec2 Render(ShaderPixel shaderPixel);
    }
}
