using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace VirtualCamera.Src
{
    public class Cuboid : WorldObject
    {
        public float width = 40f;
        public float length = 60f;
        public float height1 = 0f;
        public float height2 = 60f;

        public Vector4 V1 { get; set; }
        public Vector4 V2 { get; set; }
        public Vector4 V3 { get; set; }
        public Vector4 V4 { get; set; }
        public Vector4 V5 { get; set; }
        public Vector4 V6 { get; set; }
        public Vector4 V7 { get; set; }
        public Vector4 V8 { get; set; }

        public Cuboid(float startX, float startZ)
        {
            V1 = new Vector4(startX, height1, startZ, 1);
            V2 = new Vector4(startX + width, height1, startZ, 1);
            V3 = new Vector4(startX, height1, startZ - length, 1);
            V4 = new Vector4(startX + width, height1, startZ - length, 1);

            V5 = new Vector4(startX, height2, startZ, 1);
            V6 = new Vector4(startX + width, height2, startZ, 1);
            V7 = new Vector4(startX, height2, startZ - length, 1);
            V8 = new Vector4(startX + width, height2, startZ - length, 1);

            Vertices = new List<Vector4>()
            {
                V1, V2, V3, V4, V5, V6, V7, V8
            };
        }

        public void Draw()
        {
            GraphicsManager.spriteBatch.DrawLine(Pixels[0], Pixels[1], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[0], Pixels[2], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[1], Pixels[3], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[2], Pixels[3], Color.AliceBlue);

            GraphicsManager.spriteBatch.DrawLine(Pixels[0], Pixels[4], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[1], Pixels[5], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[2], Pixels[6], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[3], Pixels[7], Color.AliceBlue);

            GraphicsManager.spriteBatch.DrawLine(Pixels[4], Pixels[5], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[4], Pixels[6], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[5], Pixels[7], Color.AliceBlue);
            GraphicsManager.spriteBatch.DrawLine(Pixels[6], Pixels[7], Color.AliceBlue);
        }
    }
}
