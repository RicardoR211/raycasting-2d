# Projeto 04 — Raycasting 2D com Campo de Visão

Personagem que lança raios em tempo real, detectando paredes e obstáculos usando interseção raio-segmento implementada do zero.

---

## O que foi implementado

- `Segmento.cs` — struct com dois pontos `A` e `B` representando paredes e obstáculos
- Sala fechada definida como lista de segmentos
- Movimento do personagem com WASD, rotação com Q/E
- Clique do mouse para adicionar caixas (obstáculos) dinamicamente
- Dois modos de lançamento de raios, alternados com `Tab`

---

## Matemática utilizada

**Interseção raio-segmento** via produto vetorial 2D:

```
t = Cross(A - origem, segDir) / Cross(rayDir, segDir)
u = Cross(A - origem, rayDir) / Cross(rayDir, segDir)
```

Interseção válida quando `t >= 0` e `0 <= u <= 1`.

**Câmera angular** via `Atan2` para calcular o ângulo exato de cada vértice em relação ao personagem.

---

## Modos de raycasting

### LancarLeque
Lança N raios distribuídos uniformemente em um arco de ângulo fixo (120° por padrão).

### LancarParaCantos
Lança 3 raios para cada vértice presente na cena — um no ângulo exato, um com offset de -0.0001 rad e um com +0.0001 rad. Os pontos de impacto são ordenados por ângulo para formar o polígono de visão corretamente.

---

## Comparativo de performance

| Vértices na cena | LancarLeque (raios) | LancarParaCantos (raios) |
|---|---|---|
| 1 | 720 | 3 |
| 10 | 720 | 30 |
| 45 | 720 | 135 |
| ~240 | 720 | 720 (ponto de cruzamento) |
| 552 | 720 | 1656 |
| 1000+ | 720 | 3000+ |

> Teste com 552 vértices: `LancarLeque` manteve 60fps, `LancarParaCantos` caiu para ~22fps.  
> Teste com 1000+ vértices: `LancarLeque` manteve 60fps sem quedas perceptíveis.

**Conclusão:** `LancarLeque` tem custo determinístico — não importa quantos obstáculos existam na cena, o número de raios nunca muda. `LancarParaCantos` é mais preciso (nunca erra uma quina) mas escala linearmente com o número de vértices, tornando-se inviável em cenas densas.

---

## Como rodar

```
dotnet run
```

**Controles:**
- `WASD` — mover personagem
- `Q / E` — rotacionar
- `Clique esquerdo` — adicionar caixa na posição do mouse
- `Tab` — alternar entre LancarLeque e LancarParaCantos
