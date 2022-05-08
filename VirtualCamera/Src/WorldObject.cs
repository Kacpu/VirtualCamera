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
        protected Vector2[] Pixels { get; set; } 

        public void Observe(Matrix? worldTransformationMatrix, Matrix perspectiveTransformationMatrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if(worldTransformationMatrix is Matrix wtm)
                {
                    Vertices[i] = Vertices[i].LeftTransform(wtm);
                }

                var vp = Vertices[i].Z >= 0 ? new Vector4(Vertices[i].X, Vertices[i].Y, -0.001f, Vertices[i].W) : Vertices[i];

                //var vp = Vertices[i];
                //if (Vertices[i].Z >= 0)
                //{
                //    continue;
                //}

                PerspectiveVertexTransform(i, vp, perspectiveTransformationMatrix);
                TransformVertexToPixel(i, PerspectiveVertices[i]);
            }
        }

        private void PerspectiveVertexTransform(int id, Vector4 vertex, Matrix ptm)
        {
            var v = vertex.LeftTransform(ptm);
            v = v.PerspectiveDivide();

            if(vertex.Z == -1f)
            {
                //Debug.WriteLine("here");
            }

            //if (v.X < -1 || v.X > 1)
            //{
            //    Debug.WriteLine("here");
            //}

            //if(v.Z < -1 || v.Z > 1)
            //{
            //    v.X = v.X < -1 ? -1 : v.X;
            //    v.X = v.X > 1 ? 1 : v.X;
            //    v.Y = v.Y < -1 ? -1 : v.Y;
            //    v.Y = v.Y > 1 ? 1 : v.Y;
            //}

            //v.X = v.X < -1 || v.X > 1 ? PerspectiveVertices[id].X: v.X;
            //v.Y = v.Y < -1 || v.Y > 1 ? PerspectiveVertices[id].Y : v.Y;

            PerspectiveVertices[id] = v;
        }

        private void TransformVertexToPixel(int id, Vector4 perspectiveVertex)
        {
            Pixels[id] = new Vector2()
            {
                X = (perspectiveVertex.X + 1) * GraphicsManager.ScreenWidth * 0.5f,
                Y = (1 - perspectiveVertex.Y) * GraphicsManager.ScreenHeight * 0.5f,
            };
        }

        protected bool IsOut()
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
