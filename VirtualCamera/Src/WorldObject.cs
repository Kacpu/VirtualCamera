using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class WorldObject
    {
        public List<Vector4> Vertices { get; set; }
        public List<Vector2> Pixels { get; set; } = new List<Vector2>();

        public void Project(Matrix ptm)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] = Vertices[i].LeftTransform(ptm);
                Vertices[i] = PerspectiveProjection.PerspectiveDivision(Vertices[i]);
            }
        }

        public void TransformToPixels()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Pixels.Add(new Vector2()
                {
                    X = (Vertices[i].X + 1) * 1080f * 0.5f,
                    Y = (1 - Vertices[i].Y) * 720f * 0.5f,
                });
            }
        }
    }
}
