using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class Transformations
    {
        public static Matrix GetTranslationMatrix(float x = 0, float y = 0, float z = 0)
        {
            Matrix translationMatrix = Matrix.Identity;

            translationMatrix.M14 = x;
            translationMatrix.M24 = y;
            translationMatrix.M34 = z;

            return translationMatrix;
        }

        public static Matrix GetRotationXMatrix(float angle)
        {
            Matrix rotationMatrix = Matrix.Identity;

            rotationMatrix.M22 = (float)Math.Cos(angle);
            rotationMatrix.M23 = -(float)Math.Sin(angle);
            rotationMatrix.M32 = (float)Math.Sin(angle);
            rotationMatrix.M33 = (float)Math.Cos(angle);

            return rotationMatrix;
        }

        public static Matrix GetRotationYMatrix(float angle)
        {
            Matrix rotationMatrix = Matrix.Identity;

            rotationMatrix.M11 = (float)Math.Cos(angle);
            rotationMatrix.M13 = (float)Math.Sin(angle);
            rotationMatrix.M31 = -(float)Math.Sin(angle);
            rotationMatrix.M33 = (float)Math.Cos(angle);

            return rotationMatrix;
        }

        public static Matrix GetRotationZMatrix(float angle)
        {
            Matrix rotationMatrix = Matrix.Identity;

            rotationMatrix.M11 = (float)Math.Cos(angle);
            rotationMatrix.M12 = -(float)Math.Sin(angle);
            rotationMatrix.M21 = (float)Math.Sin(angle);
            rotationMatrix.M22 = (float)Math.Cos(angle);

            return rotationMatrix;
        }
    }
}
