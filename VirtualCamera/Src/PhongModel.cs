
using Microsoft.Xna.Framework;
using System;

namespace VirtualCamera.Src
{
    public class PhongModel
    {
        //observer position
        private Vector3 ObserverPosition = new Vector3(1.0f,  1.0f,  0.0f);

        //Light position
        private Vector3 LightPosition = new Vector3(1280.0f,  720.0f,  1.0f);

        //Ambient
        private float K_a = 0.7f; //weight
        private Vector3 I_a = new Vector3(0.8f, 0.8f, 0); //color

        //Diffuse
        private float K_d = 0.6f;
        private Vector3 I_d = new Vector3(0.7f, 0.7f, 0);

        //Specular
        private float K_s = 0.7f;
        private Vector3 I_s = new Vector3(1.0f,  1.0f,  1.0f);
        private float Alpha = 10;


        public Vector3 GetPhongModelCalc(Vector3 point, Vector3 normal)
        {
            //point surface normal - normalize
            Vector3 N = Vector3.Normalize(normal);

            //light normal
            Vector3 L = Vector3.Normalize(LightPosition - point);

            Vector3 Ambient = GetAmbient();
            Vector3 Diffusion = GetDiffusion(N, L);
            Vector3 Specular = GetSpecular(N,L, point);

            return (Ambient + Diffusion + Specular);
        }

        private Vector3 GetAmbient()
        {
            return (I_a * K_a);
        }

        private Vector3 GetDiffusion(Vector3 N, Vector3 L)
        {
            float VecDot = Vector3.Dot(N,  L);
            Vector3 D = I_d * (K_d * VecDot);

            return D;
        }

        private Vector3 GetSpecular(Vector3 N, Vector3 L, Vector3 Point)
        {

            Vector3 R = 2  *  Vector3.Dot(L  , N)  *  N - L;
            Vector3 V = Vector3.Normalize(ObserverPosition - Point);
            float dotRV = Vector3.Dot(R,V);
            Vector3 S =  I_s * K_s * (float) Math.Pow(dotRV, Alpha);

            return S;
        }

        
    }
}