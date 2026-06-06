using System;

namespace Projeto01_Vetores
{
    public struct Mat4
    {
        //Elementos da matriz
        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;

        //Matriz indentidade global
        public static Mat4 Identity()
        {
            Mat4 identity = new Mat4();
            identity.m11 = 1.0f;
            identity.m22 = 1.0f;
            identity.m33 = 1.0f;
            identity.m44 = 1.0f;

            return identity;
        }

        //Multiplicação de matriz
        public static Mat4 Multiply(Mat4 a, Mat4 b)
        {
            Mat4 res = new Mat4();

            // Linha 1
            res.m11 = a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31 + a.m14 * b.m41;
            res.m12 = a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32 + a.m14 * b.m42;
            res.m13 = a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33 + a.m14 * b.m43;
            res.m14 = a.m11 * b.m14 + a.m12 * b.m24 + a.m13 * b.m34 + a.m14 * b.m44;

            // Linha 2
            res.m21 = a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31 + a.m24 * b.m41;
            res.m22 = a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32 + a.m24 * b.m42;
            res.m23 = a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33 + a.m24 * b.m43;
            res.m24 = a.m21 * b.m14 + a.m22 * b.m24 + a.m23 * b.m34 + a.m24 * b.m44;

            // Linha 3
            res.m31 = a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31 + a.m34 * b.m41;
            res.m32 = a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32 + a.m34 * b.m42;
            res.m33 = a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33 + a.m34 * b.m43;
            res.m34 = a.m31 * b.m14 + a.m32 * b.m24 + a.m33 * b.m34 + a.m34 * b.m44;

            // Linha 4
            res.m41 = a.m41 * b.m11 + a.m42 * b.m21 + a.m43 * b.m31 + a.m44 * b.m41;
            res.m42 = a.m41 * b.m12 + a.m42 * b.m22 + a.m43 * b.m32 + a.m44 * b.m42;
            res.m43 = a.m41 * b.m13 + a.m42 * b.m23 + a.m43 * b.m33 + a.m44 * b.m43;
            res.m44 = a.m41 * b.m14 + a.m42 * b.m24 + a.m43 * b.m34 + a.m44 * b.m44;

            return res;
        }

        public static Mat4 CreateTranslation(float tx, float ty, float tz)
        {
            Mat4 trans = Identity();
            trans.m41 = tx;
            trans.m42 = ty;
            trans.m43 = tz;
            return trans;
        }

        public static Mat4 CreateScale(float sx, float sy, float sz)
        {
            Mat4 scale = Identity();
            scale.m11 = sx;
            scale.m22 = sy;
            scale.m33 = sz;
            return scale;
        }

        public static Mat4 CreateRotationX(float radians)
        {
            Mat4 rot = Identity();
            float c = MathF.Cos(radians);
            float s = MathF.Sin(radians);

            rot.m22 = c;
            rot.m23 = s;
            rot.m32 = -s;
            rot.m33 = c;
            return rot;
        }

        public static Mat4 CreateRotationY(float radians)
        {
            Mat4 rot = Identity();
            float c = MathF.Cos(radians);
            float s = MathF.Sin(radians);

            rot.m11 = c;
            rot.m13 = s;
            rot.m31 = -s;
            rot.m33 = c;
            return rot;
        }

        public static Mat4 CreateRotationZ(float radians)
        {
            Mat4 rot = Identity();
            float c = MathF.Cos(radians);
            float s = MathF.Sin(radians);

            rot.m11 = c;
            rot.m12 = s;
            rot.m21 = -s;
            rot.m22 = c;
            return rot;
        }


        /// <summary>
        /// Constrói uma Matriz de Visualização (LookAt) para a câmera.
        /// </summary>
        /// <param name="eye">A posição atual da câmera no espaço 3D.</param>
        /// <param name="target">O ponto para onde a câmera está olhando.</param>
        /// <param name="up">O vetor que indica a direção "para cima" no mundo.</param>
        /// <returns>Retorna uma Mat4 de mudança de base.</returns>
        public static Mat4 LookAt(Vec3 eye, Vec3 target, Vec3 up)
        {
            /*
             * right-handed
             * vetor linha (v * M)
             * view matrix estilo DirectX clássico
             */

            // 1. Z: Eixo Forward (da câmera para o alvo)
            Vec3 zAxis = (eye - target).Normalize();

            // 2. X: Eixo Right (produto vetorial entre Up do mundo e Z da câmera)
            Vec3 xAxis = Vec3.Cross(up, zAxis).Normalize();

            // 3. Y: Eixo Up Verdadeiro (ortogonal a Z e X)
            Vec3 yAxis = Vec3.Cross(zAxis, xAxis); // Z e X já são unitários e perpendiculares

            Mat4 result = Identity();

            // Matriz de Rotação (transposta porque estamos mudando do World para o View Space)
            result.m11 = xAxis.X; result.m12 = yAxis.X; result.m13 = zAxis.X;
            result.m21 = xAxis.Y; result.m22 = yAxis.Y; result.m23 = zAxis.Y;
            result.m31 = xAxis.Z; result.m32 = yAxis.Z; result.m33 = zAxis.Z;

            // Translação (Projeção do vetor 'eye' contra os novos eixos locais)
            result.m41 = -Vec3.DotProduct(xAxis, eye);
            result.m42 = -Vec3.DotProduct(yAxis, eye);
            result.m43 = -Vec3.DotProduct(zAxis, eye);

            return result;
        }


        public static Mat4 Perspective(float fovYRadians, float aspect, float zNear, float zFar)
        {
            Mat4 result = new Mat4();

            float tanHalfFov = MathF.Tan(fovYRadians / 2.0f);

            // Eixo X: ajustado pelo Aspect Ratio e Campo de Visão
            result.m11 = 1.0f / (aspect * tanHalfFov);

            // Eixo Y: ajustado pelo Campo de Visão
            result.m22 = 1.0f / tanHalfFov;

            // Eixo Z: remapeamento da profundidade entre o Near e o Far Plane
            result.m33 = (zFar + zNear) / (zNear - zFar);

            // Coloca o -Z no componente 'w' para a divisão de perspectiva no Transform
            result.m34 = -1.0f;

            // Translação de Z baseada no Near e Far Plane
            result.m43 = (2.0f * zFar * zNear) / (zNear - zFar);

            // Note que m44 permanece 0.0f propositalmente.
            return result;
        }
    }
}
