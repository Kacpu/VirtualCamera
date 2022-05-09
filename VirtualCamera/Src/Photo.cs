using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VirtualCamera.Src
{
    public static class Photo
    {
        private static int photoCounter = 0;

        public static void TakePhoto()
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VirtualCamera");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, "photo" + (photoCounter++) + ".png");

            while (File.Exists(filePath))
            {
                filePath = Path.Combine(directoryPath, "photo" + (photoCounter++) + ".png");
            }

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                int[] backBuffer = new int[GraphicsManager.ScreenWidth * GraphicsManager.ScreenHeight];
                GraphicsManager.graphics.GraphicsDevice.GetBackBufferData(backBuffer);

                Texture2D texture = new Texture2D(GraphicsManager.graphics.GraphicsDevice,
                    GraphicsManager.ScreenWidth, GraphicsManager.ScreenHeight, false,
                    GraphicsManager.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat);

                texture.SetData(backBuffer);
                texture.SaveAsPng(fs, GraphicsManager.ScreenWidth, GraphicsManager.ScreenHeight);
                texture.Dispose();
            }
        }
    }
}
