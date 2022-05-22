using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VirtualCamera.Src
{
    public class Camera
    {
        private readonly float viewportWidth;
        private readonly float viewportHeight;
        private readonly float aspectRatio;
        private readonly float zNear;
        private readonly float zFar;
        private float viewportDistance;
        private float tgHalfFov;
        private Matrix perspectiveTransformationMatrix;
        private readonly float speed = 2f;
        private readonly float angleSpeed = 0.5f;

        public enum Action
        {
            Right,
            Left,
            Up,
            Down,
            Forward,
            Backward,
            PosRotateX,
            NegRotateX,
            PosRotateY,
            NegRotateY,
            PosRotateZ,
            NegRotateZ,
            ZoomIn,
            ZoomOut,
            TakePhoto,
            Reset,
            None
        }

        public Camera(float viewportWidth, float viewportHeight, float viewportDistance)
        {
            this.viewportWidth = viewportWidth;
            this.viewportHeight = viewportHeight;
            this.viewportDistance = viewportDistance;

            aspectRatio = this.viewportWidth / this.viewportHeight;
            tgHalfFov = CountTgHalfFov();

            zNear = 0.1f;
            zFar = 1000f;

            perspectiveTransformationMatrix = new Matrix();
            SetPerspectiveMatrix();
        }

        public Action TakeAction()
        {
            KeyboardState kstate = Keyboard.GetState();

            var key = kstate.GetPressedKeyCount() > 0 ? kstate.GetPressedKeys()[0] : Keys.None;

            Action action = key switch
            {
                Keys.Right => Action.Right,
                Keys.Left => Action.Left,
                Keys.Up => Action.Up,
                Keys.Down => Action.Down,

                Keys.W => Action.Forward,
                Keys.S => Action.Backward,

                Keys.D => Action.PosRotateY,
                Keys.A => Action.NegRotateY,

                Keys.I => Action.NegRotateX,
                Keys.K => Action.PosRotateX,

                Keys.J => Action.PosRotateZ,
                Keys.L => Action.NegRotateZ,

                Keys.OemPlus => Action.ZoomIn,
                Keys.OemMinus => Action.ZoomOut,

                Keys.Enter => Action.TakePhoto,

                Keys.R => Action.Reset,

                _ => Action.None
            };

            return action;
        }

        public void Observe(World world, Action action)
        {
            Matrix? transformationMatrix = null;

            switch (action)
            {
                case Action.Right:
                    transformationMatrix = Transformations.GetTranslationMatrix(x: -speed); break;
                case Action.Left:
                    transformationMatrix = Transformations.GetTranslationMatrix(x: speed); break;
                case Action.Up:
                    transformationMatrix = Transformations.GetTranslationMatrix(y: -speed); break;
                case Action.Down:
                    transformationMatrix = Transformations.GetTranslationMatrix(y: speed); break;

                case Action.Forward:
                    transformationMatrix = Transformations.GetTranslationMatrix(z: speed); break;
                case Action.Backward:
                    transformationMatrix = Transformations.GetTranslationMatrix(z: -speed); break;

                case Action.PosRotateX:
                    transformationMatrix = Transformations.GetRotationXMatrix(angleSpeed * (float)Math.PI / 180f); break;
                case Action.NegRotateX:
                    transformationMatrix = Transformations.GetRotationXMatrix(-angleSpeed * (float)Math.PI / 180f); break;

                case Action.PosRotateY:
                    transformationMatrix = Transformations.GetRotationYMatrix(angleSpeed * (float)Math.PI / 180f); break;
                case Action.NegRotateY:
                    transformationMatrix = Transformations.GetRotationYMatrix(-angleSpeed * (float)Math.PI / 180f); break;

                case Action.PosRotateZ:
                    transformationMatrix = Transformations.GetRotationZMatrix(angleSpeed * (float)Math.PI / 180f); break;
                case Action.NegRotateZ:
                    transformationMatrix = Transformations.GetRotationZMatrix(-angleSpeed * (float)Math.PI / 180f); break;

                case Action.ZoomIn:
                    ZoomIn(); break;
                case Action.ZoomOut:
                    ZoomOut(); break;

                case Action.TakePhoto:
                    Photo.TakePhoto(); return;

                default:
                    break;
            }

            world.Observe(transformationMatrix, perspectiveTransformationMatrix);

            world.ScanLine();
        }

        private void ZoomIn()
        {
            viewportDistance -= speed;
            tgHalfFov = CountTgHalfFov();
            SetPerspectiveMatrix();
        }

        private void ZoomOut()
        {
            viewportDistance = viewportDistance + speed < 0 ? viewportDistance + speed : viewportDistance;
            tgHalfFov = CountTgHalfFov();
            SetPerspectiveMatrix();
        }

        private float CountTgHalfFov()
        {
            Debug.WriteLine("d: " + viewportDistance);
            Debug.WriteLine(2*Math.Atan2(viewportHeight, (2 * Math.Abs(viewportDistance))) * 180/Math.PI);
            return viewportHeight / (2 * Math.Abs(viewportDistance));
        }

        private void SetPerspectiveMatrix()
        {
            perspectiveTransformationMatrix.M11 = 1f / (aspectRatio * tgHalfFov);
            perspectiveTransformationMatrix.M22 = 1f / tgHalfFov;
            perspectiveTransformationMatrix.M33 = (zNear + zFar) / (zNear - zFar);
            perspectiveTransformationMatrix.M34 = 2 * zNear * zFar / (zNear - zFar);
            perspectiveTransformationMatrix.M43 = -1f;
        }
    }
}
