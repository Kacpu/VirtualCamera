using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public abstract class WorldObject
    {
        protected List<Vector4> Vertices { get; set; }
        protected List<Vector4> PerspectiveVertices { get; set; }
        protected List<Vector2> Pixels { get; set; }


        public void Observe(Matrix? worldTransformationMatrix, Matrix perspectiveTransformationMatrix)
        {
            PerspectiveVertices = new List<Vector4>();
            Pixels = new List<Vector2>();

            for (int i = 0; i < Vertices.Count; i++)
            {
                if(worldTransformationMatrix is Matrix wtm)
                {
                    Vertices[i] = Vertices[i].LeftTransform(wtm);
                }

                PerspectiveVertexTransform(Vertices[i], perspectiveTransformationMatrix);
                TransformVertexToPixel(PerspectiveVertices[i]);
            }
        }

        private void PerspectiveVertexTransform(Vector4 vertex, Matrix ptm)
        {
            var v = vertex.LeftTransform(ptm).PerspectiveDivide();
            PerspectiveVertices.Add(v);
        }

        private void TransformVertexToPixel(Vector4 perspectiveVertex)
        {
            Pixels.Add(new Vector2()
            {
                X = (perspectiveVertex.X + 1) * GraphicsManager.ScreenWidth * 0.5f,
                Y = (1 - perspectiveVertex.Y) * GraphicsManager.ScreenHeight * 0.5f,
            });
        }

        public abstract void Draw();
    }
}
