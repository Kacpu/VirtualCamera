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
            Pixels = new Vector2[Vertices.Count];
        }

        public override void Draw()
        {
            if (IsOut())
            {
                return;
            }

            GraphicsManager.spriteBatch.DrawLine(Pixels[0], Pixels[1], Color.Blue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[0], Pixels[2], Color.Blue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[1], Pixels[3], Color.Blue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[2], Pixels[3], Color.Blue);

            GraphicsManager.spriteBatch.DrawLine(Pixels[0], Pixels[4], Color.Red);
            GraphicsManager.spriteBatch.DrawLine(Pixels[1], Pixels[5], Color.Red);
            GraphicsManager.spriteBatch.DrawLine(Pixels[2], Pixels[6], Color.Red);
            GraphicsManager.spriteBatch.DrawLine(Pixels[3], Pixels[7], Color.Red);

            GraphicsManager.spriteBatch.DrawLine(Pixels[4], Pixels[5], Color.Green);
            GraphicsManager.spriteBatch.DrawLine(Pixels[4], Pixels[6], Color.Green);
            GraphicsManager.spriteBatch.DrawLine(Pixels[5], Pixels[7], Color.Green);
            GraphicsManager.spriteBatch.DrawLine(Pixels[6], Pixels[7], Color.Green);
        }
    }
}
