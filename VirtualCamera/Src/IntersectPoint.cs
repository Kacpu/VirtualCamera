using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class IntersectPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Edge Edge { get; set; }

        public IntersectPoint(float x, float y, Edge edge)
        {
            X = x;
            Y = y;
            Edge = edge;
        }
    }
}
