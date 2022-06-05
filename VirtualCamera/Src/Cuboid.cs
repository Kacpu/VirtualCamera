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
        private readonly Texture2D crazy_cat;
        private readonly Texture2D colour;
        private readonly Texture2D marble;
        private readonly Texture2D sweet;
        private Texture2D oneText;

        public List<Vector3> VerticeNormals { get; set; }

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

            crazy_cat = GraphicsManager.content.Load<Texture2D>("cat3_256");
            colour = GraphicsManager.content.Load<Texture2D>("wood_256");
            marble = GraphicsManager.content.Load<Texture2D>("gk_256");
            sweet = GraphicsManager.content.Load<Texture2D>("sweet");

            oneText = sweet;
        }

        public override void GenerateEdgesAndPolygons(Vector4 lightPosition)
        {
            Polygons = new Polygon[]
            {
                new Polygon(Pixels[0], Pixels[1], Pixels[3], Pixels[2], Color.Orange, colour, lightPosition), //0 - dol
                new Polygon(Pixels[0], Pixels[1], Pixels[5], Pixels[4], Color.Green, sweet, lightPosition), //1 - front
                new Polygon(Pixels[0], Pixels[2], Pixels[6], Pixels[4], Color.Red, colour, lightPosition), //2 - lewo
                new Polygon(Pixels[1], Pixels[3], Pixels[7], Pixels[5], Color.Yellow, crazy_cat, lightPosition), //3 - prawo
                new Polygon(Pixels[2], Pixels[3], Pixels[7], Pixels[6], Color.Blue, colour, lightPosition), //4 - tyl
                new Polygon(Pixels[4], Pixels[5], Pixels[7], Pixels[6], Color.Violet, marble, lightPosition) //5 - gora
            };

            Polygons[0].SetVertices(Vertices[0], Vertices[1], Vertices[3], Vertices[2]);
            Polygons[1].SetVertices(Vertices[0], Vertices[1], Vertices[5], Vertices[4]);
            Polygons[2].SetVertices(Vertices[0], Vertices[2], Vertices[6], Vertices[4]);
            Polygons[3].SetVertices(Vertices[1], Vertices[3], Vertices[7], Vertices[5]);
            Polygons[4].SetVertices(Vertices[2], Vertices[3], Vertices[7], Vertices[6]);
            Polygons[5].SetVertices(Vertices[4], Vertices[5], Vertices[7], Vertices[6]);

            //Polygons[0].SetPerspectiveVertices(PerspectiveVertices[0], PerspectiveVertices[1], PerspectiveVertices[3], PerspectiveVertices[2]);
            //Polygons[1].SetPerspectiveVertices(PerspectiveVertices[0], PerspectiveVertices[1], PerspectiveVertices[5], PerspectiveVertices[4]);
            //Polygons[2].SetPerspectiveVertices(PerspectiveVertices[0], PerspectiveVertices[2], PerspectiveVertices[6], PerspectiveVertices[4]);
            //Polygons[3].SetPerspectiveVertices(PerspectiveVertices[1], PerspectiveVertices[3], PerspectiveVertices[7], PerspectiveVertices[5]);
            //Polygons[4].SetPerspectiveVertices(PerspectiveVertices[2], PerspectiveVertices[3], PerspectiveVertices[7], PerspectiveVertices[6]);
            //Polygons[5].SetPerspectiveVertices(PerspectiveVertices[4], PerspectiveVertices[5], PerspectiveVertices[7], PerspectiveVertices[6]);

            VerticeNormals = new();
            VerticeNormals.Add(MeanNormal(Polygons[0].Normal, Polygons[1].Normal, Polygons[2].Normal));
            VerticeNormals.Add(MeanNormal(Polygons[0].Normal, Polygons[1].Normal, Polygons[3].Normal));
            VerticeNormals.Add(MeanNormal(Polygons[0].Normal, Polygons[2].Normal, Polygons[4].Normal));
            VerticeNormals.Add(MeanNormal(Polygons[0].Normal, Polygons[3].Normal, Polygons[4].Normal));
            VerticeNormals.Add(MeanNormal(Polygons[1].Normal, Polygons[2].Normal, Polygons[5].Normal));
            VerticeNormals.Add(MeanNormal(Polygons[1].Normal, Polygons[3].Normal, Polygons[5].Normal));
            VerticeNormals.Add(MeanNormal(Polygons[2].Normal, Polygons[4].Normal, Polygons[5].Normal));
            VerticeNormals.Add(MeanNormal(Polygons[3].Normal, Polygons[4].Normal, Polygons[5].Normal));

            Polygons[0].SetLights(VerticeNormals[0], VerticeNormals[1], VerticeNormals[3], VerticeNormals[2]);
            Polygons[1].SetLights(VerticeNormals[0], VerticeNormals[1], VerticeNormals[5], VerticeNormals[4]);
            Polygons[2].SetLights(VerticeNormals[0], VerticeNormals[2], VerticeNormals[6], VerticeNormals[4]);
            Polygons[3].SetLights(VerticeNormals[1], VerticeNormals[3], VerticeNormals[7], VerticeNormals[5]);
            Polygons[4].SetLights(VerticeNormals[2], VerticeNormals[3], VerticeNormals[7], VerticeNormals[6]);
            Polygons[5].SetLights(VerticeNormals[4], VerticeNormals[5], VerticeNormals[7], VerticeNormals[6]);

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

        private Vector3 MeanNormal(Vector3 n1, Vector3 n2, Vector3 n3)
        {
            return new Vector3((n1.X + n2.X + n3.X) / 3, (n1.Y + n2.Y + n3.Y) / 3, 1);
        }

        public override void Draw()
        {
            //GraphicsManager.spriteBatch.Draw(crazy_cat, new Rectangle(100, 100, 256, 256), Color.White);

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
