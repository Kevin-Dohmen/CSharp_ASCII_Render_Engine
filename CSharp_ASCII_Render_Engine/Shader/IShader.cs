﻿using CSharp_ASCII_Render_Engine.Types.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_ASCII_Render_Engine.Shader
{
    public interface IShader
    {
        public Vec2 Render(Vec2 uv);
    }
}
