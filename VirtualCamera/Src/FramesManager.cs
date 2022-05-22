using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VirtualCamera.Src
{
    class FramesManager
    {
        private int frames;
        private int frames_temp = 0;
        private double frames_Time = 0;
        private SpriteFont _robotoFont;
        private string _framesName;

        public FramesManager(string framesName)
        {
            //_robotoFont = GameManager.content.Load<SpriteFont>("Fonts/Roboto16");

            _framesName = framesName;
        }

        public void CountFrames(GameTime gameTime)
        {
            frames_temp++;
            frames_Time += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frames_Time >= 1000)
            {
                frames = frames_temp;
                Debug.WriteLine(_framesName + ": " + frames);
                frames_temp = 0;
                frames_Time = 0;
                //Thread.Sleep(20);
            }
            //Thread.Sleep(5);
        }

        public void Draw(int posY)
        {
            //GraphicsManager.spriteBatch.DrawString(_robotoFont, $"{_framesName}: {frames}", new Vector2(20, posY), Color.Black);
        }
    }
}
