using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto01_Vetores
{
    internal struct Segmento
    {
        public Vec3 A;
        public Vec3 B;

        public Segmento(Vec3 A, Vec3 B)
        {
            this.A = A;
            this.B = B;

        }

        //Aq eu retorno bool, mas também posso retornar um float representado por t.
        public static bool Intersecta(Vec3 origem, Vec3 direcao, Segmento seg, out float t)
        {
            t = 0f;

            //Segmento dir
            float segDirX = seg.B.X - seg.A.X;
            float segDirY = seg.B.Y - seg.A.Y;

            // Q - P = A = origem
            float qpX = seg.A.X - origem.X;
            float qpY = seg.A.Y - origem.Y;

            float den = (direcao.X * segDirY) - (direcao.Y * segDirX);

            //Se o denominador for 0 (ou proximo), a retas são paralelas
            if(Math.Abs(den) < 0.00001f)
            {
                return false;
            }

            //Calculando t: Cross(Q - P, segDir) / den
            float crossQpSeg = (qpX * segDirY) - (qpY * segDirX);
            t = crossQpSeg / den;

            //Calculando u: Cross(Q - P, rayDir) / den
            float crossQpRay = (qpX * direcao.Y) - (qpY * direcao.X);
            float u = crossQpRay / den;

            //Condição de intersecção:
            // t >= 0 (à frente da origem do raio)
            // 0 <= u <= 1 (dentro dos limites do segmento A -> B)
            if(t >= 0f && u >= 0f && u <= 1f)
            {
                return true;
            }

            return false;
        }
    }
}
