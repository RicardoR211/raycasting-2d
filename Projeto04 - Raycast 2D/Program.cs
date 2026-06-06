using Projeto01_Vetores;
using Raylib_cs;
using System;
using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        Raylib.InitWindow(800, 600, "Projeto 04 - Raycast 2D");
        Raylib.SetTargetFPS(60);

        Vec3 pontoTopInicial = new Vec3(1f, 2f, 0f);
        Vec3 pontoTopFinal = new Vec3(799f, 2f, 0f);
        Vec3 pontoDownInicial = new Vec3(799f, 599f, 0f);
        Vec3 pontoDownFinal = new Vec3(0f, 599f, 0f);

        //Dados do player
        float velocidade = 200f;

        //Dados do raycast
        int numRaios = 720;
        //Aq vai de -60 até 60 graus.
        float campoVisao = 361;
        //Alcance maximo do raio
        float alcanceMaximo = 400f;

        //Dados matemática
        float deg2Rad = MathF.PI / 180f;


        //Listas de Segmentos
        List<Segmento> paredesSegs = new List<Segmento>();
        paredesSegs.Add(new Segmento(pontoTopInicial, pontoTopFinal));
        paredesSegs.Add(new Segmento(pontoTopFinal, pontoDownInicial));
        paredesSegs.Add(new Segmento(pontoDownInicial, pontoDownFinal));
        paredesSegs.Add(new Segmento(pontoDownFinal, pontoTopInicial));

        List<Segmento> blocosSegs = new List<Segmento>();

        //Controller
        bool modoCantos = false;


        Vec3 personagem = new Vec3(400f, 300f, 0f);
        float angulo = 0f;

        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();

            //Pegando movimento
            float movX = Input.Eixo(KeyboardKey.D, KeyboardKey.A);
            float movY = Input.Eixo(KeyboardKey.S, KeyboardKey.W);
            float movAngX = Input.Eixo(KeyboardKey.E, KeyboardKey.Q);

            //Adicionando movimentos
            personagem.X += movX * velocidade * dt;
            personagem.Y += movY * velocidade * dt;

            angulo += movAngX * 90f * dt;

            //Verificando click do mouse
            if (Input.SoltouClique(MouseButton.Left))
            {
                CliqueParaCaixa(50, 50, blocosSegs);
            }

            //Verificando o aperto da tecla
            if (Input.ClicouTecla(KeyboardKey.Tab))
            {
                modoCantos = !modoCantos;
            }

            //Gerando a lista final
            List<Segmento> todosSegmentos = ObterTodosSegmentos(paredesSegs, blocosSegs);

            if (modoCantos) Console.WriteLine("Modo Vertices");
            else Console.WriteLine("Modo Leque");

            Console.WriteLine("Total de vertices na tela: " + todosSegmentos.Count);
            int totalVertices = todosSegmentos.Count * 2;

            

            //Funções que desenham
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.Black);
            


            //Raios sendo retornados a depender da função ativa
            List<Vec3> pontosImpactos;

            if (modoCantos)
            {
                // A lanterna perfeita 360 graus que ignora o campoVisao e usa as quinas
                pontosImpactos = LancarParaCantos(personagem, todosSegmentos, alcanceMaximo);
            }
            else
            {
                // O leque clássico como estava antes
                pontosImpactos = LancarLeque(personagem, angulo, todosSegmentos, numRaios, campoVisao, alcanceMaximo, deg2Rad);
            }

            //Origem q é no player
            Vector2 origem = new Vector2(personagem.X, personagem.Y);
            //Desenhando os triângulos
            for (int i = 0; i < pontosImpactos.Count - 1; i++)
            {
                Vector2 p1 = new Vector2(pontosImpactos[i].X, pontosImpactos[i].Y);
                Vector2 p2 = new Vector2(pontosImpactos[i + 1].X, pontosImpactos[i + 1].Y);

                Vector2 v1 = p2 - origem;
                Vector2 v2 = p1 - origem;
                float cross = v1.X * v2.Y - v1.Y * v2.X;

                if (cross > 0)
                    Raylib.DrawTriangle(origem, p1, p2, Color.Blue);
                else
                    Raylib.DrawTriangle(origem, p2, p1, Color.Blue);
            }

            //Desenhando paredes
            foreach (Segmento seg in paredesSegs)
            {
                Raylib.DrawLine(
                    (int)seg.A.X,
                    (int)seg.A.Y,
                    (int)seg.B.X,
                    (int)seg.B.Y,
                    Color.Red
                );
            }

            foreach (Segmento seg in blocosSegs)
            {
                Raylib.DrawLine(
                    (int)seg.A.X,
                    (int)seg.A.Y,
                    (int)seg.B.X,
                    (int)seg.B.Y,
                    Color.Red
                );
            }

            //Desenhando o player kkkk
            Raylib.DrawCircle((int) personagem.X, (int) personagem.Y, 5f, Color.Green);


            //Desenhando informações
            Raylib.DrawFPS(10, 10);
            Raylib.DrawText($"Total de vertices na tela: {totalVertices}", 10, 35, 20, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }


    //Ao clicar cria uma caixa e joga na lista de segmentos passados
    public static void CliqueParaCaixa(int compCaixa, int alturaCaixa, List<Segmento> segments)
    {
        float posMouseX = Raylib.GetMouseX();
        float posMouseY = Raylib.GetMouseY();

        float metadeCompCaixa = compCaixa / 2f;

        float metadeAltCaixa = alturaCaixa / 2f;

        Vec3 topoEsq = new Vec3(posMouseX - metadeCompCaixa, posMouseY - metadeAltCaixa, 0f);
        Vec3 topoDir = new Vec3(posMouseX + metadeCompCaixa, posMouseY - metadeAltCaixa, 0f);
        Vec3 baixoDir = new Vec3(posMouseX + metadeCompCaixa, posMouseY + metadeAltCaixa, 0f);
        Vec3 baixoEsq = new Vec3(posMouseX - metadeCompCaixa, posMouseY + metadeAltCaixa, 0f);

        segments.Add(new Segmento(topoEsq, topoDir));  
        segments.Add(new Segmento(topoDir, baixoDir));
        segments.Add(new Segmento(baixoDir, baixoEsq));
        segments.Add(new Segmento(baixoEsq, topoEsq));
    }

    //Concatena listas
    public static List<Segmento> ObterTodosSegmentos(List<Segmento> paredes, List<Segmento> blocos)
    {
        List<Segmento> todosSegmentos = new List<Segmento>(paredes.Count + blocos.Count);

        todosSegmentos.AddRange(paredes);
        todosSegmentos.AddRange(blocos);

        return todosSegmentos;

    }

    //Lancar leques de raios
    public static List<Vec3> LancarLeque(Vec3 personagem, float angulo, List<Segmento> todosSegmentos, 
        int numRaios, float campoVisao, float alcanceMaximo, float deg2Rad)
    {
        List<Vec3> pontosImpactos = new List<Vec3>();
        Vec3 direcao = new Vec3(1f, 0, 0);

        //Mudando para raios definidos na variaveis
        for (var i = -campoVisao / 2; i < campoVisao / 2; i += campoVisao / numRaios)
        {
            float anguloRaioGraus = angulo + i;

            float anguloRaioRad = anguloRaioGraus * deg2Rad;

            float dirX = MathF.Cos(anguloRaioRad);
            float dirY = MathF.Sin(anguloRaioRad);

            direcao = new Vec3(dirX, dirY, 0f);

            float menorT = float.MaxValue;
            bool bateuEmAlgo = false;

            foreach (Segmento seg in todosSegmentos)
            {
                if (Segmento.Intersecta(personagem, direcao, seg, out float t))
                {
                    if (t < menorT)
                    {
                        menorT = t;
                        bateuEmAlgo = true;
                    }

                }

            }
            if (bateuEmAlgo && menorT <= alcanceMaximo)
            {
                int posXInicial = (int)personagem.X;
                int posYInicial = (int)personagem.Y;
                int posXFinal = (int)(personagem.X + menorT * direcao.X);
                int posYFinal = (int)(personagem.Y + menorT * direcao.Y);

                //Ao invez de desenhar somente a linha, vamos guardar os pontos para um triângulo
                //Raylib.DrawLine(posXInicial, posYInicial, posXFinal, posYFinal, Color.Blue);

                Vec3 pontoDaBatida = new Vec3(posXFinal, posYFinal, 0f);
                pontosImpactos.Add(pontoDaBatida);
            }
            else
            {
                // Joga o raio lá longe na direção em que ele estava olhando
                Vec3 pontoDaBatida = new Vec3(
                    personagem.X + direcao.X * alcanceMaximo,
                    personagem.Y + direcao.Y * alcanceMaximo,
                    0f
                );
                pontosImpactos.Add(pontoDaBatida);
            }
        }

        return pontosImpactos;
    }

    //Lancar somente para os vertices.
    public static List<Vec3> LancarParaCantos(Vec3 personagem, List<Segmento> todosSegmentos, float alcanceMaximo)
    {
        //Pegando todos os vértices dos segmentos
        List<Vec3> vertices = new List<Vec3>();

        foreach (Segmento seg in todosSegmentos)
        {
            vertices.Add(seg.A);
            vertices.Add(seg.B);
        }

        //Guardando o ponto e o angulo dele
        List<(Vec3 Ponto, float Angulo)> impactosComAngulo = new List<(Vec3 ponto, float Angulo)>();

        //Lançando 3 raios para cada vertice
        //1 nele e 2 de - e + 0.00001
        foreach (Vec3 vertice in vertices)
        {
            //Atan traz o angulo em radianos
            //Tg = co/ca -> co = y e ca = x
            float anguloBase = MathF.Atan2(vertice.Y - personagem.Y, vertice.X - personagem.X);

            //Aq os 3 fundamentais (antes, exato, depois)
            float[] angulosTeste = { anguloBase - 0.0001f, anguloBase, anguloBase + 0.0001f };

            foreach (float angulo in angulosTeste)
            {
                float dirX = MathF.Cos(angulo);
                float dirY = MathF.Sin(angulo);
                Vec3 direcao = new Vec3(dirX, dirY, 0f);

                float menorT = float.MaxValue;
                bool bateuEmAlgo = false;

                // Testando colisões exatamente como no método anterior
                foreach (Segmento seg in todosSegmentos)
                {
                    if (Segmento.Intersecta(personagem, direcao, seg, out float t))
                    {
                        if (t < menorT)
                        {
                            menorT = t;
                            bateuEmAlgo = true;
                        }
                    }
                }

                //Calculando onde o raio bateu
                Vec3 pontoDaBatida;
                if (bateuEmAlgo && menorT <= alcanceMaximo)
                {
                    pontoDaBatida = new Vec3(
                        personagem.X + menorT * direcao.X,
                        personagem.Y + menorT * direcao.Y,
                        0f
                    );
                }
                else
                {
                    pontoDaBatida = new Vec3(
                        personagem.X + alcanceMaximo * direcao.X,
                        personagem.Y + alcanceMaximo * direcao.Y,
                        0f
                    );
                }

                // Adicionamos o impacto E o ângulo em que ele foi disparado
                impactosComAngulo.Add((pontoDaBatida, angulo));
            }
        }

        impactosComAngulo.Sort((a, b) => a.Angulo.CompareTo(b.Angulo));

        List<Vec3> pontosImpactos = new List<Vec3>();
        foreach (var impacto in impactosComAngulo)
        {
            pontosImpactos.Add(impacto.Ponto);
        }

        //Adiciona o primeiro ponto no final da lista para fechar tudo
        if (pontosImpactos.Count > 0)
        {
            pontosImpactos.Add(pontosImpactos[0]);
        }


        return pontosImpactos;
    }
}