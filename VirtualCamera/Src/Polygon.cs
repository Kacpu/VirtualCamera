using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class Polygon
    {
        private Vector3 p1;
        private Vector3 p2;
        private Vector3 p3;
        private Vector3 p4;
        private float a;
        private float b;
        private float c;
        private float d;
        private List<Vector3> points;

        public Color Color;

        public bool IsRead { get; set; }

        public Polygon(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color color)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;

            points = new List<Vector3>()
            {
                p1, p2, p3, p4
            };

            Color = color;
            IsRead = false;
            CalcEquations();
        }

        private void CalcEquations()
        {
            float sharedX = p2.X;
            float sharedY = p2.Y;
            float sharedZ = p2.Z;

            float x1 = p1.X;
            float y1 = p1.Y;
            float z1 = p1.Z;

            float x2 = p3.X;
            float y2 = p3.Y;
            float z2 = p3.Z;

            float a1 = x1 - sharedX;
            float b1 = y1 - sharedY;
            float c1 = z1 - sharedZ;
            float a2 = x2 - sharedX;
            float b2 = y2 - sharedY;
            float c2 = z2 - sharedZ;

            a = b1 * c2 - b2 * c1;
            b = a2 * c1 - a1 * c2;
            c = a1 * b2 - b1 * a2;
            d = (-a * sharedX - b * sharedY - c * sharedZ);
        }

        public float CalculateDepth(float x, float y)
        {
            float z = (-d - a * x - b * y) / c;
            return z;
        }

        public bool IsInPolygon((float x, float y) checkPoint)
        {
            bool side = false;
            bool flag = false;
            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                int next = (i + 1) % n;
                double d = GetVectorProduct(checkPoint, points[i], points[next]);
                if (d != 0.0)
                {
                    if (!flag)
                    {
                        flag = true;
                        side = d > 0.0;
                    }
                    else if (side != (d > 0.0))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private float GetVectorProduct((float x, float y) sharedPoint, Vector3 point1, Vector3 point2)
        {
            return (point1.X - sharedPoint.x) * (point2.Y - sharedPoint.y) -
                    (point2.X - sharedPoint.x) * (point1.Y - sharedPoint.y);
        }
    }
}
