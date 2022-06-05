using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VirtualCamera.Src
{
    public abstract class WorldObject
    {
        protected List<Vector4> Vertices { get; set; }
        protected Vector4[] PerspectiveVertices { get; set; } 
        protected Vector3[] Pixels { get; set; }
        public abstract Edge[] Edges { get; set; }
        public abstract Polygon[] Polygons { get; set; }

        public void Observe(Matrix? worldTransformationMatrix, Matrix perspectiveTransformationMatrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if(worldTransformationMatrix is Matrix wtm)
                {
                    Vertices[i] = Vertices[i].LeftTransform(wtm);
                }

                var vp = Vertices[i].Z >= 0 ? new Vector4(Vertices[i].X, Vertices[i].Y, -0.001f, Vertices[i].W) : Vertices[i];

                PerspectiveVertexTransform(i, vp, perspectiveTransformationMatrix);
                TransformVertexToPixel(i, PerspectiveVertices[i]);
            }
        }

        private void PerspectiveVertexTransform(int id, Vector4 vertex, Matrix ptm)
        {
            var v = vertex.LeftTransform(ptm);
            v = v.PerspectiveDivide();

            PerspectiveVertices[id] = v;
        }

        private void TransformVertexToPixel(int id, Vector4 perspectiveVertex)
        {
            Pixels[id] = new Vector3()
            {
                X = (perspectiveVertex.X + 1) * GraphicsManager.ScreenWidth * 0.5f,
                Y = (1 - perspectiveVertex.Y) * GraphicsManager.ScreenHeight * 0.5f,
                Z = perspectiveVertex.Z
            };
        }

        public abstract void GenerateEdgesAndPolygons(Vector4 lightPosition);

        public bool IsOut()
        {
            foreach(var p in Pixels)
            {
                if(p.X >= 0 && p.X <= GraphicsManager.ScreenWidth && p.Y >= 0 && p.Y <= GraphicsManager.ScreenHeight)
                {
                    return false;
                }
            }
            return true;
        }

        public abstract void Draw();
    }
}
