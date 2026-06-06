using System;

namespace Projeto01_Vetores
{
    public struct Vec3
    {
        public float X, Y, Z;

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly float Magnitude()
        {
            return MathF.Sqrt(X * X + Y * Y + Z * Z);
        }

        public readonly Vec3 Normalize()
        {
            float mag = this.Magnitude();
            if (mag == 0) return new Vec3(0, 0, 0);
            return new Vec3(X / mag, Y / mag, Z / mag);
        }

        public static float DotProduct(Vec3 a, Vec3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                (a.Y * b.Z) - (a.Z * b.Y),
                (a.Z * b.X) - (a.X * b.Z),
                (a.X * b.Y) - (a.Y * b.X)
            );
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator *(Vec3 a, float b)
        {
            return new Vec3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3 operator *(float b, Vec3 a)
        {
            return new Vec3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3 Transform(Mat4 m, Vec3 v)
        {
            float xFinal = v.X * m.m11 + v.Y * m.m21 + v.Z * m.m31 + 1.0f * m.m41;
            float yFinal = v.X * m.m12 + v.Y * m.m22 + v.Z * m.m32 + 1.0f * m.m42;
            float zFinal = v.X * m.m13 + v.Y * m.m23 + v.Z * m.m33 + 1.0f * m.m43;
            float wFinal = v.X * m.m14 + v.Y * m.m24 + v.Z * m.m34 + 1.0f * m.m44;


            //Divisão de perspectiva
            if (wFinal != 1.0f && wFinal != 0.0f)
            {
                xFinal /= wFinal;
                yFinal /= wFinal;
                zFinal /= wFinal;
            }

            return new Vec3(xFinal, yFinal, zFinal);
        }

        // Retorna o tamanho ao quadrado (muito mais rápido, sem raiz quadrada)
        public readonly float SqrMagnitude()
        {
            return X * X + Y * Y + Z * Z;
        }

        // Calcula a distância exata entre dois pontos (usa raiz quadrada)
        public static float Distance(Vec3 a, Vec3 b)
        {
            Vec3 diff = new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            return diff.Magnitude();
        }

        // Calcula a distância ao quadrado para testes de colisão de alta performance
        public static float DistanceSquared(Vec3 a, Vec3 b)
        {
            Vec3 diff = new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            return diff.SqrMagnitude();
        }
    }
}