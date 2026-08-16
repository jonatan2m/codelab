# comprimir-videos.sh

Recomprime vídeos reduzindo o tamanho sem perda visível. Envolve o `ffmpeg` com
padrões que foram medidos, não chutados, e com salvaguardas para que um lote de
10 horas não destrua nada se der errado no meio.

---

## Instalação

O script é autocontido. Precisa apenas de `ffmpeg` e `ffprobe` no PATH:

```bash
sudo apt install ffmpeg
chmod +x ~/bin/comprimir-videos.sh
```

Se `~/bin` não estiver no PATH, adicione ao `~/.zshrc`:

```bash
export PATH="$HOME/bin:$PATH"
```

---

## Uso

```
comprimir-videos.sh [opções] <arquivo.mp4 | diretório> ...
```

Aceita quantos alvos você quiser, misturando arquivos e diretórios. Diretórios são
varridos **sem recursão** (só o primeiro nível).

Extensões reconhecidas: `mp4 mkv mov avi m4v webm ts mpg mpeg wmv flv`

---

## Parâmetros

| Opção | Argumento | Padrão | O que faz |
|---|---|---|---|
| `-c`, `--codec` | `x265` \| `x264` \| `nvenc` \| `av1` | `x265` | Escolhe o encoder. Veja a tabela de codecs. |
| `-q`, `--crf` | inteiro | varia por codec | Qualidade. **Menor = melhor qualidade e arquivo maior.** |
| `-p`, `--preset` | nome | varia por codec | Preset do encoder. Mais lento ≠ menor; veja a nota. |
| `-o`, `--out` | diretório | `<origem>/comprimidos` | Onde gravar as saídas. |
| `-j`, `--jobs` | inteiro ≥ 1 | `1` | Quantos arquivos processar em paralelo. |
| `-n`, `--dry-run` | — | desligado | Simula: lista o que faria sem escrever nada. |
| `-f`, `--force` | — | desligado | Reprocessa mesmo se a saída já existir. |
| `--replace` | — | desligado | Move o original para quarentena após verificar a saída. |
| `--audio-max` | kbps | `160` | Reencoda o áudio só se passar disso. `0` nunca reencoda. |
| `-h`, `--help` | — | — | Ajuda resumida. |

### Sobre `-j` (paralelo)

O `x265` e o `x264` já usam todos os núcleos da CPU sozinhos — subir `-j` com eles
divide o mesmo bolo e não acelera nada. Use `-j 2` apenas com `nvenc`, que roda na
GPU e deixa a CPU ociosa.

### Sobre `-p` (preset)

Contraintuitivo, mas medido: **`slow` não produziu arquivos menores que `medium`.**
Com CRF o encoder mira qualidade constante, não tamanho — e nesse tipo de conteúdo
o preset lento decidiu gastar mais bits. Deu 43% de redução contra 50% do `medium`,
gastando 3× mais tempo. Não mexa nisso sem medir.

---

## Codecs

| Codec | Redução | Velocidade | Compatibilidade |
|---|---|---|---|
| `x265` | **~50%** | 1,1× tempo real | Boa: VLC, mpv, celular, TV moderna. **Não** toca em Firefox nem TV antiga. |
| `x264` | ~29% | 0,57× tempo real | **Universal.** Toca em qualquer coisa deste século. |
| `nvenc` | ~35% | **0,19× tempo real** | Igual ao x265 (também é HEVC). Exige GPU NVIDIA. |
| `av1` | ~46% | 3,6× tempo real | Irregular. Navegadores novos sim, TVs e players antigos não. |

*Velocidade em múltiplos da duração do vídeo: 1,1× significa que 1 hora de vídeo
leva 1h06 para encodar.*

**Regra prática:** `x265` por padrão. `nvenc` quando o tempo importa mais que os GB.
`x264` quando o arquivo precisa tocar em qualquer lugar. `av1` não vale a pena —
comprime menos que o x265 e demora 3× mais.

---

## Escala de qualidade

Cada codec tem sua própria escala. `-q 22` no x264 **não** significa o mesmo que
`-q 22` no x265. Os padrões do script já são o valor certo de cada um:

| Codec | Padrão | Faixa útil | Referência medida |
|---|---|---|---|
| `x265` | **22** | 18–28 | 20 → −35% · 22 → −50% |
| `x264` | 20 | 16–26 | 20 → −29% |
| `nvenc` | 26 | 22–32 | 26 → −35% |
| `av1` | 30 | 26–36 | 30 → −46% |

Diminuir 2 pontos de CRF aumenta o arquivo em roughly 25%. Aumentar 2 pontos reduz
outro tanto, com risco crescente de artefato visível em cenas de movimento caótico
(fumaça, fogo, água, confete).

---

## Receitas

```bash
# Simular antes — sempre comece por aqui
comprimir-videos.sh -n ~/videos

# Padrão: x265 CRF 22, pasta inteira
comprimir-videos.sh ~/videos

# Rápido, na GPU
comprimir-videos.sh -c nvenc -j 2 ~/videos

# Compatibilidade máxima
comprimir-videos.sh -c x264 ~/videos

# Um arquivo só, para conferir antes de soltar o lote
comprimir-videos.sh ~/videos/aula-14.mp4

# Comparar dois CRF sem um sobrescrever o outro
comprimir-videos.sh -q 20 -o ~/cmp20 ~/videos/aula-14.mp4
comprimir-videos.sh -q 24 -o ~/cmp24 ~/videos/aula-14.mp4

# Lote longo, destacado do terminal
nohup comprimir-videos.sh ~/videos > ~/compressao.log 2>&1 &
tail -f ~/compressao.log

# Substituir os originais (só depois de conferir!)
comprimir-videos.sh --replace ~/videos
```

---

## O que ele nunca faz

- **Nunca apaga o original.** Sem `--replace`, o original nem é tocado. Com
  `--replace`, ele é *movido* para `originais-substituidos/` ao lado — e só depois
  da saída passar na verificação. Nada vai para o lixo, nunca.
- **Nunca aceita saída corrompida.** Compara a duração da saída com a do original
  (tolerância de 1 s). Divergiu, descarta e reporta erro.
- **Nunca deixa arquivo parcial no lugar do final.** Encoda para
  `.nome.parcial.mp4` e só renomeia ao terminar. Ctrl-C no meio não deixa lixo que
  seria confundido com trabalho pronto na próxima execução.
- **Nunca recomprime à toa.** Se a redução ficar abaixo de 5%, descarta a saída —
  trocar 3% de espaço por perda de qualidade é mau negócio.
- **Nunca refaz trabalho.** Saída existente é pulada. Rodar de novo após uma
  interrupção retoma de onde parou. Use `-f` para forçar.

---

## Saída

```
~/videos/
├── aula-1.mp4                      ← original, intacto
├── aula-2.mp4
├── comprimidos/
│   ├── aula-1.x265.mp4             ← saída
│   └── aula-2.x265.mp4
└── originais-substituidos/         ← só existe se usar --replace
    └── aula-1.mp4
```

O sufixo carrega o codec, então `x265` e `x264` do mesmo arquivo convivem na mesma
pasta sem conflito.

Ao final, um resumo:

```
───────────────────────────────────────────
convertidos: 15   pulados: 0   sem ganho: 0   erros: 0
antes:  16.09 GB
depois: 8.05 GB
liberado: 8.04 GB (-50.0%)
───────────────────────────────────────────
```

**Códigos de saída:** `0` tudo certo · `1` algum arquivo falhou · `2` erro de uso.

---

## De onde vieram os padrões

Os valores default não são chute. Em 2026-08-15 rodei 6 perfis contra 2 trechos de
90 s de conteúdo real — um de detalhe fino (close de faca) e um de pior caso
(brasa e fumaça) — medindo tamanho, tempo e SSIM contra o original.

| Perfil | Redução | SSIM (pior) | Velocidade |
|---|---|---|---|
| **x265 CRF 22 medium** | **−50%** | 0,9822 | 1,1× |
| av1 CRF 30 p6 | −46% | 0,9845 | 3,6× |
| x265 CRF 22 slow | −43% | 0,9833 | 3,0× |
| x265 CRF 20 medium | −35% | 0,9843 | 1,2× |
| nvenc HEVC CQ 26 | −35% | 0,9860 | 0,19× |
| x264 CRF 20 medium | −29% | 0,9853 | 0,57× |

Todos ficaram acima de SSIM 0,98 — a faixa considerada imperceptível. O intervalo
inteiro entre o melhor e o pior é de 1,2 ponto de SSIM, enquanto a diferença de
tamanho vai de 29% a 50%. Você compra 21 pontos de redução por 0,8 ponto de SSIM.

**Ressalva:** SSIM não é VMAF. Serve para descartar perfis ruins, não para arbitrar
diferenças de 0,001 entre os bons. Assista a uma amostra antes de comprometer um
lote grande.

---

## Problemas

**`encoder 'X' não existe neste ffmpeg`**
Seu build não tem esse encoder. Confira com `ffmpeg -encoders | grep X`. O
`hevc_nvenc` exige GPU NVIDIA com driver instalado; o `libsvtav1` exige ffmpeg 6+.

**`duração divergente, descartando`**
O arquivo de origem provavelmente tem defeito (timestamps quebrados, gravação
interrompida). Tente remuxar antes: `ffmpeg -i entrada.mp4 -c copy corrigido.mp4`

**`ganho de apenas N%, descartando saída`**
O arquivo já estava bem comprimido. Não é erro — é o script recusando um mau
negócio. Se quiser forçar assim mesmo, baixe o `MIN_GANHO` no topo do script.

**Encode lento demais**
Troque para `-c nvenc -j 2`. Cai de ~1,1× para ~0,19× do tempo real.

**O vídeo não toca depois de comprimido**
Você usou HEVC (`x265`/`nvenc`) num player que não suporta. Refaça com `-c x264`.
