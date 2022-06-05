using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class Polygon
    {
        public int Id { get; set; }
        private Vector3 p1;
        private Vector3 p2;
        private Vector3 p3;
        private Vector3 p4;
        private float a;
        private float b;
        private float c;
        private float d;
        private List<Vector3> points;
        private List<Vector4> vertices;
        private List<Vector4> perspectiveVertices;
        private List<Vector2> textiles;
        private List<Vector3> lights;
        public Vector3 Normal { get; set; }

        private PhongModel phong;

        public Texture2D texture;

        public Color Color;
        
        public Color[,] TextureData { get; set; }

        private static int idCounter = 0;
        //private int textureWidth;
        //private int textureHeight;

        public bool IsRead { get; set; }

        public Polygon(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color color, Texture2D texture, Vector4 lightPosition)
        {
            Id = idCounter++;

            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;

            this.texture = texture;

            points = new List<Vector3>()
            {
                p1, p2, p3, p4
            };

            Color = color;
            IsRead = false;
            CalcEquations();

            ReadTexture(texture);

            textiles = new List<Vector2>()
            {
                new Vector2(0, texture.Height - 1), new Vector2(texture.Width - 1, texture.Height - 1),
                new Vector2(texture.Width - 1, 0), new Vector2(0, 0)
            };

            phong = new PhongModel(lightPosition);

            //Normal = new Vector3(a / c, b / c, 1);
        }

        private void ReadTexture(Texture2D texture)
        {
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);

            TextureData = new Color[texture.Width, texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    TextureData[x, y] = pixels[x + y * texture.Width];
                }
            }
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

        public List<(Vector3 p, Color color)> TextureInterpolate(IntersectPoint p1, IntersectPoint p2)
        {
            int n = points.Count;
            Vector3 polyPoint1 = new Vector3();
            Vector3 polyPoint2 = new Vector3();
            Vector3 texturePoint1 = new Vector3();
            Vector3 texturePoint2 = new Vector3();
            Vector3 light1 = new Vector3();
            Vector3 light2 = new Vector3();

            List<(Vector3 p, Color color)> pixels = new();

            int c = 0;

            for (int i = 0; i < n; i++)
            {
                int next = (i + 1) % n;

                if (points[i].Y <= p1.Y == points[next].Y >= p1.Y)
                {
                    var p = CalcIntersect(points[i], points[next], p1.Y);

                    if(p == null)
                    {
                        return pixels;
                    }

                    if (!p.HasValue)
                    {
                        continue;
                    }

                    float edgeLen = CalcEdgeLen(points[i], points[next]);
                    float topToInterLen = CalcEdgeLen(points[i], p.Value);
                    float prop1 = topToInterLen / edgeLen;

                    Vector3 texturePoint = new Vector3();

                    if (textiles[i].X == textiles[next].X)
                    { 
                        texturePoint.X = (int)textiles[i].X;

                        if(textiles[i].Y > textiles[next].Y)
                        {
                            prop1 = 1  -  prop1;
                        }

                        texturePoint.Y = (int) (prop1 * texture.Height);
                    }
                    else if (textiles[i].Y == textiles[next].Y)
                    {
                        if(textiles[i].X > textiles[next].X)
                        {
                            prop1 = 1 - prop1;
                        }
                        texturePoint.X = (int)(prop1 * texture.Width);
                        texturePoint.Y = (int)textiles[i].Y;
                    }

                    c++;
                    if(c == 1)
                    {
                        polyPoint1 = p.Value;
                        texturePoint1 = texturePoint;
                        light1 = InterpolateInterLight(points[i], lights[i], points[next], lights[next], polyPoint1);
                    }
                    else if(c == 2)
                    {
                        polyPoint2 = p.Value;
                        texturePoint2 = texturePoint;
                        light2 = InterpolateInterLight(points[i], lights[i], points[next], lights[next], polyPoint2);
                        break;
                    }
                }
            }

            float polyPointsLen = CalcEdgeLen(polyPoint1, polyPoint2);

            if(polyPointsLen == 0)
            {
                return pixels;
            }

            float texturePointsLen = CalcEdgeLen(texturePoint1, texturePoint2);

            if(texturePointsLen == 0)
            {
                return pixels;
            }

            float propPerPixel = texturePointsLen  /  polyPointsLen;

            if (polyPoint1.X > polyPoint2.X)
            {
                var temp = polyPoint1;
                polyPoint1 = polyPoint2;
                polyPoint2 = temp;

                temp = texturePoint1;
                texturePoint1 = texturePoint2;
                texturePoint2 = temp;

                temp = light1;
                light1 = light2;
                light2 = temp;
            }

            float yLenT = texturePoint2.Y - texturePoint1.Y;
            float xLenT = texturePoint2.X - texturePoint1.X;

            float sinusT = yLenT / texturePointsLen;
            float cosinusT = xLenT / texturePointsLen;

            float dxT = propPerPixel * cosinusT;
            float dyT = propPerPixel * sinusT;

            int startPol = (int)(p1.X - polyPoint1.X);
            int ctr = 0;

            for (int i = startPol; i <= startPol + (int)(p2.X - p1.X); i++)
            {
                Vector3 pT = new Vector3();
                pT.X = texturePoint1.X + dxT * i;
                pT.Y = texturePoint1.Y + dyT * i;

                if(pT.X < 0 || pT.X >= texture.Width || pT.Y < 0 || pT.Y >= texture.Height)
                {
                    continue;
                }

                Color colorT = TextureData[(int)pT.X, (int)pT.Y];

                //cieniowanie koloru
                Vector3 light = InterpolatePixelLight(polyPoint1, light1, polyPoint2, light2, new Vector3(p1.X + ctr, p1.Y, 0));
                Color lightColor = new Color(colorT.ToVector3() * light);

                //if(lightColor.R == 0 && lightColor.G == 0 && lightColor.B == 0)
                //{
                //    if(pixels.Count > 0)
                //    {
                //        lightColor = pixels[^1].color;
                //    }
                //    else
                //    {
                //        lightColor = colorT;
                //    }
                //}
                
                pixels.Add((new Vector3(p1.X + ctr, p1.Y, 0), lightColor));
                
                ctr++;
            }

            return pixels;
        }

        private float CalcEdgeLen(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow((p1.X - p2.X),  2) + Math.Pow((p1.Y - p2.Y),  2));
        }

        private Vector3? CalcIntersect(Vector3 p1, Vector3 p2, float y)
        {
            float A1 = (p1.Y - p2.Y);
            float B1 = p2.X - p1.X;
            float C1 = p1.X * p2.Y - p2.X * p1.Y;

            float A2 = 0;
            float B2 = 1;
            float C2 = -y;

            float? xp = null;

            if ((A1 * B2 - A2 * B1) != 0)
            {
                xp = (B1 * C2 - B2 * C1) / (A1 * B2 - A2 * B1);
            }

            if(xp != null)
            {
                return new Vector3(xp.Value, y, 0);
            }
            else
            {
                return null;
            }
        }

        public void SetPerspectiveVertices(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4)
        {
            perspectiveVertices = new List<Vector4> { p1, p2, p3, p4 };
            //SetNormal();
        }

        public void SetVertices(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4)
        {
            vertices = new List<Vector4> { p1, p2, p3, p4 };
            SetNormal(p1, p2, p3);
        }

        public void SetLights(Vector3 n1, Vector3 n2, Vector3 n3, Vector3 n4)
        {
            //Vector3 l1 = phong.GetPhongModelCalc(p1, n1);
            //Vector3 l2 = phong.GetPhongModelCalc(p2, n2);
            //Vector3 l3 = phong.GetPhongModelCalc(p3, n3);
            //Vector3 l4 = phong.GetPhongModelCalc(p4, n4);

            Vector3 l1 = phong.GetPhongModelCalc(new Vector3(vertices[0].X, vertices[0].Y, vertices[0].Z), n1);
            Vector3 l2 = phong.GetPhongModelCalc(new Vector3(vertices[1].X, vertices[1].Y, vertices[1].Z), n2);
            Vector3 l3 = phong.GetPhongModelCalc(new Vector3(vertices[2].X, vertices[2].Y, vertices[2].Z), n3);
            Vector3 l4 = phong.GetPhongModelCalc(new Vector3(vertices[3].X, vertices[3].Y, vertices[3].Z), n4);

            lights = new() { l1, l2, l3, l4 };
        }

        private Vector3 InterpolateInterLight(Vector3 p1, Vector3 l1, Vector3 p2, Vector3 l2, Vector3 pInter)
        {
            Vector3 l1Part = l1 * (Math.Abs(p1.Y - pInter.Y) / Math.Abs(p1.Y - p2.Y));
            Vector3 l2Part = l2 * (Math.Abs(pInter.Y - p2.Y) / Math.Abs(p1.Y - p2.Y));

            //Vector3 l1Part = l1 * ((p1.Y - pInter.Y) /(p1.Y - p2.Y));
            //Vector3 l2Part = l2 * ((pInter.Y - p2.Y) / (p1.Y - p2.Y));

            return (l1Part + l2Part);

            //float polyPointsLen = CalcEdgeLen(p1, p2);
            //float interPolyLen = CalcEdgeLen(p1, pInter);
            //float lightsLen = CalcEdgeLen(l1, l2);
            //float lightProp = (interPolyLen * lightsLen) / polyPointsLen;
            //float lightInterLen = lightsLen * lightProp;

            //float yLenT = p2.Y - p1.Y;
            //float xLenT = p2.X - p1.X;
            //float zLenT = p2.Z = p1.Z;

            //float sinusT = yLenT / lightsLen;
            //float cosinusT = xLenT / lightsLen;

            ////float dxL = lightInterLen * cosinusT;
            ////float dyL = lightInterLen * sinusT;

            //float dxL = p1.X > p2.X ? lightProp : 1 - lightProp;
            //float dyL = p1.Z > p2.Z ? lightProp : 1 - lightProp;
            //float dzL = p1.Z > p2.Z ? lightProp : 1 - lightProp;

            //return new Vector3(l1.X * lightProp, l1.Y * lightProp, l1.Z * lightProp);
        }

        private Vector3 InterpolatePixelLight(Vector3 p1, Vector3 l1, Vector3 p2, Vector3 l2, Vector3 pixel)
        {
            //Vector3 l1Part = l1 * (Math.Abs(p2.X - pixel.X) / Math.Abs(p2.X - p1.X));
            //Vector3 l2Part = l2 * (Math.Abs(pixel.X - p1.X) / Math.Abs(p2.X - p1.X));

            Vector3 l1Part = l1 * ((p2.X - pixel.X) / (p2.X - p1.X));
            Vector3 l2Part = l2 * ((pixel.X - p1.X) / Math.Abs(p2.X - p1.X));

            return (l1Part + l2Part);
        }

        private void SetNormal(Vector4 p1, Vector4 p2, Vector4 p3)
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

            var a = b1 * c2 - b2 * c1;
            var b = a2 * c1 - a1 * c2;
            var c = a1 * b2 - b1 * a2;
            //var d = (-a * sharedX - b * sharedY - c * sharedZ);

            //var ac = a / c;
            //var bc = b / c;

            //if (!float.IsFinite(ac))
            //{
            //    ac = 0;
            //}

            //if (!float.IsFinite(bc))
            //{
            //    bc = 0;
            //}

            Normal = new Vector3(a, b, c);
        }
    }
}
