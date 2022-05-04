using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VirtualCamera.Src
{
    public class PerspectiveProjection
    {
        private readonly float aspectRatio;
        private readonly float fov;
        private readonly float zNear;
        private readonly float zFar;
        public Matrix ptm;

        public PerspectiveProjection(Viewport viewport)
        {
            aspectRatio = viewport.Width / viewport.Height;
            fov = (float)Math.Atan2(viewport.Height, Math.Abs(viewport.Distance));
            zNear = -0.1f;
            zFar = -1000f;
            float tga2 = viewport.Height / (2 * Math.Abs(viewport.Distance));

            Debug.WriteLine((float)Math.Tan(fov * 0.5f));
            Debug.WriteLine(fov * 0.5f);
            Debug.WriteLine(tga2);
            Debug.WriteLine(Math.Atan(tga2));

            Vector4 mr1 = new Vector4(1f / (aspectRatio * tga2), 0, 0, 0);
            Vector4 mr2 = new Vector4(0, 1f / tga2, 0, 0);
            Vector4 mr3 = new Vector4(0, 0, (zNear + zFar) / (zNear - zFar), 2 * zNear * zFar / (zNear - zFar));
            Vector4 mr4 = new Vector4(0, 0, -1f, 0);

            ptm = new Matrix(mr1, mr2, mr3, mr4);
        }

        public static Vector4 PerspectiveDivision(Vector4 vector)
        {
            return new Vector4()
            {
                X = vector.X / vector.W,
                Y = vector.Y / vector.W,
                Z = vector.Z / vector.W,
                W = vector.W / vector.W
            };
        }
    }
}
