using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera
{
    public class GraphicsManager
    {
        public static readonly int ScreenWidth = 1280;
        public static readonly int ScreenHeight = 720;
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
    }
}
