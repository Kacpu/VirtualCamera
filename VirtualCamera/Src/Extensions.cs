using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public static class Extensions
    {
        public static Vector4 LeftTransform(this Vector4 vector, Matrix matrix)
        {
            Vector4 res = new Vector4();
            res.X = (vector.X * matrix.M11) + (vector.Y * matrix.M12) + (vector.Z * matrix.M13) + (vector.W * matrix.M14);
            res.Y = (vector.X * matrix.M21) + (vector.Y * matrix.M22) + (vector.Z * matrix.M23) + (vector.W * matrix.M24);
            res.Z = (vector.X * matrix.M31) + (vector.Y * matrix.M32) + (vector.Z * matrix.M33) + (vector.W * matrix.M34);
            res.W = (vector.X * matrix.M41) + (vector.Y * matrix.M42) + (vector.Z * matrix.M43) + (vector.W * matrix.M44);
            return res;
        }
    }
}
