using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class Viewport
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float Distance { get; set; }

        public Viewport(float width, float height, float distance)
        {
            Width = width;
            Height = height;
            Distance = distance;
        }
    }
}
