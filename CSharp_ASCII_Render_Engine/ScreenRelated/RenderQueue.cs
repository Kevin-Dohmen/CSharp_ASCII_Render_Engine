﻿namespace CSharp_ASCII_Render_Engine.ScreenRelated
{
    public class RenderQueue
    {
        public List<IRenderable> Queue { get; set; }

        public RenderQueue()
        {
            Queue = new List<IRenderable>();
        }

        public void Add(IRenderable item)
        {
            Queue.Add(item);
        }

        public void Remove(IRenderable item)
        {
            Queue.Remove(item);
        }

        public void Clear()
        {
            Queue.Clear();
        }

        public bool Contains(IRenderable item)
        {
            return Queue.Contains(item);
        }
    }

    public interface IRenderable
    {
        void Render(ScreenBuffer buffer, int frame);
    }
}
