﻿using ASCII_Render_Engine.ScreenRelated;
using ASCII_Render_Engine.Shader;
using ASCII_Render_Engine.Types.Pixels;
using ASCII_Render_Engine.Types.Vectors;
using System.Net.NetworkInformation;

namespace ASCII_Render_Engine.Geometry.Primitives
{
    public class Rectangle : IRenderable
    {
        public Vec2 Pos;
        public Vec2 Size;
        public Vec2 Color;
        public bool IsFilled = true;
        public IShader? Shader;

        // object pool
        private ShaderPixel shaderPixel = new();
        private Vec2 shaderPixelScreenRes = new();
        private Vec2 tmpVec = new();

        public Rectangle(Vec2 pos, Vec2 size, Vec2 color)
        {
            Pos = pos;
            Size = size;
            Color = new Vec2(color.x, color.y);
        }
        public Rectangle(Vec2 pos, Vec2 size, Vec2 color, bool filled)
        {
            Pos = pos;
            Size = size;
            Color = new Vec2(color.x, color.y);
            IsFilled = filled;
        }

        public Rectangle(Vec2 pos, Vec2 size, IShader shader, double alpha = 1)
        {
            Pos = pos;
            Size = size;
            Shader = shader;
            Color = new Vec2(1, alpha);
        }

        public void Render(ScreenBuffer buffer, int frame, double runTime)
        {
            int posx = (int)Math.Floor(Pos.x);
            int posy = (int)Math.Floor(Pos.y);
            int sizex = (int)Math.Ceiling(Size.x);
            int sizey = (int)Math.Ceiling(Size.y);

            if (IsFilled)
            {
                ShaderPixel pix = shaderPixel;
                shaderPixelScreenRes.x = sizex; shaderPixelScreenRes.y = sizey;
                pix.ScreenRes = shaderPixelScreenRes;

                pix.Frame = frame;
                pix.Time = runTime;

                for (int y = int.Max(posy, 0); y < int.Min(posy + sizey, buffer.Height); y++)
                {
                    for (int x = int.Max(posx, 0); x < int.Min(posx + sizex, buffer.Width); x++)
                    {
                        if (Shader != null)
                        {
                            pix.ScreenPos.x = x - posx;
                            pix.ScreenPos.y = y - posy;
                            pix.UV.DivideInPlace(pix.ScreenPos, pix.ScreenRes);

                            Vec2 col = Shader.Render(pix);
                            buffer.Buffer[y][x].SetInPlace(RenderFuncs.AlphaTransform(col.MultiplyInPlace(Color), buffer.Buffer[y][x], tmpVec));
                        }
                        else
                        {
                            buffer.Buffer[y][x].SetInPlace(RenderFuncs.AlphaTransform(Color, buffer.Buffer[y][x], tmpVec));
                        }
                    }
                }
            }
            else
            {
                for (int y = int.Max(posy, 0); y < int.Min(posy + sizey, buffer.Height); y++)
                {
                    for (int x = int.Max(posx, 0); x < int.Min(posx + sizex, buffer.Width); x++)
                    {
                        if (x == posx || x == posx + sizex - 1 || y == posy || y == posy + sizey - 1)
                            buffer.Buffer[y][x].SetInPlace(RenderFuncs.AlphaTransform(Color, buffer.Buffer[y][x], tmpVec));
                    }
                }
            }
        }
    }
}