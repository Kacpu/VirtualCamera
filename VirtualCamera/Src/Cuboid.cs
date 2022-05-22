using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.VectorDraw;

namespace VirtualCamera.Src
{
    public class Cuboid : WorldObject
    {
        public override Edge[] Edges { get; set; }
        public override Polygon[] Polygons { get; set; }

        public Cuboid(float x, float y, float z, float width, float length, float height)
        {
            Vertices = new List<Vector4>()
            {
                new Vector4(x, y, z, 1),
                new Vector4(x + width, y, z, 1),
                new Vector4(x, y, z - length, 1),
                new Vector4(x + width, y, z - length, 1),
                new Vector4(x, y + height, z, 1),
                new Vector4(x + width, y + height, z, 1),
                new Vector4(x, y + height, z - length, 1),
                new Vector4(x + width, y + height, z - length, 1)
        };

            PerspectiveVertices = new Vector4[Vertices.Count];
            Pixels = new Vector3[Vertices.Count];
        }

        public override void GenerateEdgesAndPolygons()
        {
            Polygons = new Polygon[]
            {
                new Polygon(Pixels[0], Pixels[1], Pixels[3], Pixels[2], Color.Orange), //0 - dol
                new Polygon(Pixels[0], Pixels[1], Pixels[5], Pixels[4], Color.Green), //1 - front
                new Polygon(Pixels[0], Pixels[2], Pixels[6], Pixels[4], Color.Red), //2 - lewo
                new Polygon(Pixels[1], Pixels[3], Pixels[7], Pixels[5], Color.Yellow), //3 - prawo
                new Polygon(Pixels[2], Pixels[3], Pixels[7], Pixels[6], Color.Blue), //4 - tyl
                new Polygon(Pixels[4], Pixels[5], Pixels[7], Pixels[6], Color.Violet) //5 - gora
            };

            Edges = new Edge[] 
            {
                new Edge(Pixels[0], Pixels[1], Polygons[0], Polygons[1]),
                new Edge(Pixels[0], Pixels[2], Polygons[0], Polygons[2]),
                new Edge(Pixels[1], Pixels[3], Polygons[0], Polygons[3]),
                new Edge(Pixels[2], Pixels[3], Polygons[0], Polygons[4]),

                new Edge(Pixels[0], Pixels[4], Polygons[1], Polygons[2]),
                new Edge(Pixels[1], Pixels[5], Polygons[1], Polygons[3]),
                new Edge(Pixels[2], Pixels[6], Polygons[2], Polygons[4]),
                new Edge(Pixels[3], Pixels[7], Polygons[3], Polygons[4]),

                new Edge(Pixels[4], Pixels[5], Polygons[5], Polygons[1]),
                new Edge(Pixels[4], Pixels[6], Polygons[5], Polygons[2]),
                new Edge(Pixels[5], Pixels[7], Polygons[5], Polygons[3]),
                new Edge(Pixels[6], Pixels[7], Polygons[5], Polygons[4]),
            };
        }

        public override void Draw()
        {
            if (IsOut())
            {
                return;
            }

            GraphicsManager.spriteBatch.DrawLine(Pixels[0].X, Pixels[0].Y, Pixels[1].X, Pixels[1].Y, Color.Blue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[0].X, Pixels[0].Y, Pixels[2].X, Pixels[2].Y, Color.Blue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[1].X, Pixels[1].Y, Pixels[3].X, Pixels[3].Y, Color.Blue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[2].X, Pixels[2].Y, Pixels[3].X, Pixels[3].Y, Color.Blue);

            GraphicsManager.spriteBatch.DrawLine(Pixels[0].X, Pixels[0].Y, Pixels[4].X, Pixels[4].Y, Color.Red);
            GraphicsManager.spriteBatch.DrawLine(Pixels[1].X, Pixels[1].Y, Pixels[5].X, Pixels[5].Y, Color.Red);
            GraphicsManager.spriteBatch.DrawLine(Pixels[2].X, Pixels[2].Y, Pixels[6].X, Pixels[6].Y, Color.Red);
            GraphicsManager.spriteBatch.DrawLine(Pixels[3].X, Pixels[3].Y, Pixels[7].X, Pixels[7].Y, Color.Red);

            GraphicsManager.spriteBatch.DrawLine(Pixels[4].X, Pixels[4].Y, Pixels[5].X, Pixels[5].Y, Color.Green);
            GraphicsManager.spriteBatch.DrawLine(Pixels[4].X, Pixels[4].Y, Pixels[6].X, Pixels[6].Y, Color.Green);
            GraphicsManager.spriteBatch.DrawLine(Pixels[5].X, Pixels[5].Y, Pixels[7].X, Pixels[7].Y, Color.Green);
            GraphicsManager.spriteBatch.DrawLine(Pixels[6].X, Pixels[6].Y, Pixels[7].X, Pixels[7].Y, Color.Green);
        }
    }
}
