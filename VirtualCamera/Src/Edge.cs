using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class Edge
    {
        public Vector3 P1 { get; set; }
        public Vector3 P2 { get; set; }
        public Polygon MainPolygon { get;  set; }
        public Polygon Polygon1 { get; set; }
        public Polygon Polygon2 { get; set; }

        public float XMin { get => P1.X <= P2.X ? P1.X : P2.X; }
        public float XMax { get => P1.X >= P2.X ? P1.X : P2.X; }
        public float YMin { get => P1.Y <= P2.Y ? P1.Y : P2.Y; }
        public float YMax { get => P1.Y >= P2.Y ? P1.Y : P2.Y; }

        public float XToMin { get => YMin == P1.Y ? P1.X : P2.X; }

        public Edge(Vector3 p1, Vector3 p2, Polygon polygon1, Polygon polygon2)
        {
            P1 = p1;
            P2 = p2;
            Polygon1 = polygon1;
            Polygon2 = polygon2;
        }
    }
}
