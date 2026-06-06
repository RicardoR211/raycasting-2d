using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto01_Vetores
{
    // Input.cs
    static class Input
    {
        public static float Eixo(KeyboardKey positivo, KeyboardKey negativo)
        {
            float valor = 0f;
            if (Raylib.IsKeyDown(positivo)) valor += 1f;
            if (Raylib.IsKeyDown(negativo)) valor -= 1f;
            return valor;
        }

        public static bool CliqueNovo(MouseButton botao)
        {
            return Raylib.IsMouseButtonPressed(botao);
        }

        public static bool SegurandoClique(MouseButton botao)
        {
            return Raylib.IsMouseButtonDown(botao);
        }

        public static bool SoltouClique(MouseButton botao)
        {
            return Raylib.IsMouseButtonReleased(botao);
        }

        public static bool ClicouTecla(KeyboardKey botao)
        {
            return Raylib.IsKeyPressed(botao);
        }
    }
}
