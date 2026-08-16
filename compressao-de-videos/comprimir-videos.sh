#!/usr/bin/env bash
#
# comprimir-videos.sh — recomprime vídeos reduzindo tamanho sem perda visível.
#
# Uso:   comprimir-videos.sh [opções] <arquivo.mp4 | diretório> ...
# Ajuda: comprimir-videos.sh --help
#
# Requisitos: ffmpeg e ffprobe no PATH.
# NUNCA apaga o original. Com --replace, move o original para uma pasta de
# quarentena só depois de verificar que a saída está íntegra.

set -uo pipefail

# ─── Padrões ──────────────────────────────────────────────────────────────────
CODEC=x265
CRF=""
PRESET=""
OUTDIR=""
JOBS=1
DRYRUN=0
FORCE=0
REPLACE=0
AUDIO_MAX_K=160     # reencoda áudio só se passar disso; 0 desliga
MIN_GANHO=5         # % mínimo de redução para valer a pena manter a saída

msg()  { printf '%s\n' "$*" >&2; }
erro() { printf 'erro: %s\n' "$*" >&2; }

ajuda() {
  cat >&2 <<'EOF'
comprimir-videos.sh — recomprime vídeos reduzindo tamanho sem perda visível.

USO
  comprimir-videos.sh [opções] <arquivo.mp4 | diretório> ...

OPÇÕES
  -c, --codec  NOME   x265 (padrão) | x264 | nvenc | av1
  -q, --crf    N      qualidade; menor = melhor e maior. Padrão por codec:
                      x265=22  x264=20  nvenc=26  av1=30
  -p, --preset NOME   preset do encoder (padrão: medium / p6 / 6)
  -o, --out    DIR    pasta de saída (padrão: <origem>/comprimidos)
  -j, --jobs   N      arquivos em paralelo (padrão 1; use 2 com nvenc)
  -n, --dry-run       só mostra o que faria
  -f, --force         reprocessa mesmo se a saída já existir
      --replace       após verificar, move o original para 'originais-substituidos/'
      --audio-max K   reencoda áudio acima de K kbps (padrão 160; 0 = nunca)
  -h, --help          esta ajuda

CODECS
  x265   melhor equilíbrio. ~50% menor. Não toca em Firefox nem TV antiga.
  x264   compatível com tudo. ~30% menor. Escolha segura.
  nvenc  HEVC na GPU NVIDIA. ~5x mais rápido, ~15% maior que x265.
  av1    compressão máxima, encode lento, suporte ainda irregular.

EXEMPLOS
  comprimir-videos.sh ~/videos                    # x265 CRF 22, pasta inteira
  comprimir-videos.sh -c nvenc -j 2 ~/videos      # rápido, na GPU
  comprimir-videos.sh -c x264 aula-1.mp4          # compatibilidade máxima
  comprimir-videos.sh -n ~/videos                 # simular antes
  comprimir-videos.sh --replace ~/videos          # substitui após verificar

SAÍDA
  Cada arquivo vira <nome>.<codec>.mp4 na pasta de saída. O original só sai do
  lugar com --replace, e mesmo assim vai para quarentena, nunca para o lixo.
EOF
}

# ─── Argumentos ───────────────────────────────────────────────────────────────
ALVOS=()
while [[ $# -gt 0 ]]; do
  case "$1" in
    -c|--codec)     CODEC="${2:-}"; shift 2 ;;
    -q|--crf)       CRF="${2:-}"; shift 2 ;;
    -p|--preset)    PRESET="${2:-}"; shift 2 ;;
    -o|--out)       OUTDIR="${2:-}"; shift 2 ;;
    -j|--jobs)      JOBS="${2:-}"; shift 2 ;;
    --audio-max)    AUDIO_MAX_K="${2:-}"; shift 2 ;;
    -n|--dry-run)   DRYRUN=1; shift ;;
    -f|--force)     FORCE=1; shift ;;
    --replace)      REPLACE=1; shift ;;
    -h|--help)      ajuda; exit 0 ;;
    -*)             erro "opção desconhecida: $1"; ajuda; exit 2 ;;
    *)              ALVOS+=("$1"); shift ;;
  esac
done

[[ ${#ALVOS[@]} -gt 0 ]] || { erro "nenhum arquivo ou diretório informado."; ajuda; exit 2; }

for cmd in ffmpeg ffprobe; do
  command -v "$cmd" >/dev/null || { erro "'$cmd' não encontrado. Instale com: sudo apt install ffmpeg"; exit 1; }
done

[[ "$JOBS" =~ ^[0-9]+$ && "$JOBS" -ge 1 ]] || { erro "--jobs precisa ser inteiro >= 1"; exit 2; }

# ─── Perfil de encode ─────────────────────────────────────────────────────────
case "$CODEC" in
  x265)  ENCODER=libx265;    : "${CRF:=22}"; : "${PRESET:=medium}"
         VARGS=(-c:v "$ENCODER" -tag:v hvc1 -crf "$CRF" -preset "$PRESET") ;;
  x264)  ENCODER=libx264;    : "${CRF:=20}"; : "${PRESET:=medium}"
         VARGS=(-c:v "$ENCODER" -crf "$CRF" -preset "$PRESET" -profile:v high) ;;
  nvenc) ENCODER=hevc_nvenc; : "${CRF:=26}"; : "${PRESET:=p6}"
         VARGS=(-c:v "$ENCODER" -tag:v hvc1 -preset "$PRESET" -tune hq
                -rc vbr -cq "$CRF" -b:v 0 -bf 3) ;;
  av1)   ENCODER=libsvtav1;  : "${CRF:=30}"; : "${PRESET:=6}"
         VARGS=(-c:v "$ENCODER" -crf "$CRF" -preset "$PRESET") ;;
  *)     erro "codec inválido: '$CODEC' (use x265, x264, nvenc ou av1)"; exit 2 ;;
esac

# Capturado numa variável de propósito: com 'pipefail', o grep -q fecharia o pipe
# e o SIGPIPE resultante no ffmpeg faria o pipeline inteiro parecer ter falhado.
ENCODERS_DISPONIVEIS="$(ffmpeg -hide_banner -encoders 2>/dev/null)"
grep -qE "^ *V\S* +${ENCODER}( |$)" <<<"$ENCODERS_DISPONIVEIS" \
  || { erro "encoder '$ENCODER' não existe neste ffmpeg. Veja: ffmpeg -encoders"; exit 1; }

# ─── Coleta de arquivos ───────────────────────────────────────────────────────
EXTS='mp4|mkv|mov|avi|m4v|webm|ts|mpg|mpeg|wmv|flv'
ARQUIVOS=()
for alvo in "${ALVOS[@]}"; do
  if [[ -d "$alvo" ]]; then
    while IFS= read -r -d '' f; do ARQUIVOS+=("$f"); done < <(
      find "$alvo" -maxdepth 1 -type f -regextype posix-extended \
           -iregex ".*\.($EXTS)$" -not -name "*.$CODEC.mp4" -print0 | sort -z)
  elif [[ -f "$alvo" ]]; then
    ARQUIVOS+=("$alvo")
  else
    erro "não encontrado, ignorando: $alvo"
  fi
done

[[ ${#ARQUIVOS[@]} -gt 0 ]] || { erro "nenhum vídeo encontrado."; exit 1; }

# ─── Utilidades ───────────────────────────────────────────────────────────────
tam()      { stat -c%s "$1" 2>/dev/null || echo 0; }
duracao()  { ffprobe -v error -show_entries format=duration -of csv=p=0 "$1" 2>/dev/null; }
abitrate() { ffprobe -v error -select_streams a:0 -show_entries stream=bit_rate -of csv=p=0 "$1" 2>/dev/null; }
humano()   { numfmt --to=iec --suffix=B --format='%.2f' "${1:-0}" 2>/dev/null || echo "${1}B"; }

# Processa um arquivo. Ecoa "status|origem|bytes_orig|bytes_saida" no fd 3.
processar() {
  local src="$1"
  local base dir out qdir
  base="$(basename "$src")"; base="${base%.*}"
  dir="$(dirname "$src")"
  local destdir="${OUTDIR:-$dir/comprimidos}"
  out="$destdir/$base.$CODEC.mp4"

  local sz_orig; sz_orig=$(tam "$src")

  if [[ -f "$out" && $FORCE -eq 0 ]]; then
    printf 'PULADO|%s|%s|%s\n' "$src" "$sz_orig" "$(tam "$out")" >&3
    msg "  pulado (já existe): $base"
    return 0
  fi

  # Áudio: copia por padrão; reencoda só se estiver acima do teto.
  local aargs=(-c:a copy) abr
  abr=$(abitrate "$src")
  if [[ $AUDIO_MAX_K -gt 0 && "$abr" =~ ^[0-9]+$ && $((abr / 1000)) -gt $AUDIO_MAX_K ]]; then
    aargs=(-c:a aac -b:a "${AUDIO_MAX_K}k")
  fi

  if [[ $DRYRUN -eq 1 ]]; then
    msg "  [simulação] $base  ($(humano "$sz_orig"))  ->  $out"
    printf 'SIMULADO|%s|%s|0\n' "$src" "$sz_orig" >&3
    return 0
  fi

  mkdir -p "$destdir" || { printf 'ERRO|%s|%s|0\n' "$src" "$sz_orig" >&3; return 1; }

  # Escreve em arquivo temporário: uma interrupção nunca deixa saída parcial
  # ocupando o nome final e sendo confundida com trabalho concluído.
  local tmp="$destdir/.$base.$CODEC.parcial.mp4"
  local log; log="$(mktemp)"
  msg "  encodando: $base ($(humano "$sz_orig"))"

  if ! ffmpeg -nostdin -v error -y -i "$src" \
        "${VARGS[@]}" "${aargs[@]}" -movflags +faststart "$tmp" 2>"$log"; then
    erro "falhou: $base — $(tail -3 "$log" | tr '\n' ' ')"
    rm -f "$tmp" "$log"
    printf 'ERRO|%s|%s|0\n' "$src" "$sz_orig" >&3
    return 1
  fi
  rm -f "$log"

  # Verificação: a saída precisa ter a mesma duração do original (tolerância 1s).
  local d1 d2 delta
  d1=$(duracao "$src"); d2=$(duracao "$tmp")
  if [[ -z "$d2" ]]; then
    erro "saída ilegível, descartando: $base"
    rm -f "$tmp"; printf 'ERRO|%s|%s|0\n' "$src" "$sz_orig" >&3; return 1
  fi
  delta=$(awk -v a="${d1:-0}" -v b="$d2" 'BEGIN{d=a-b; print (d<0?-d:d)}')
  if awk -v x="$delta" 'BEGIN{exit !(x > 1.0)}'; then
    erro "duração divergente (${d1}s -> ${d2}s), descartando: $base"
    rm -f "$tmp"; printf 'ERRO|%s|%s|0\n' "$src" "$sz_orig" >&3; return 1
  fi

  # Se o ganho for irrelevante, recomprimir só perdeu qualidade à toa.
  local sz_novo; sz_novo=$(tam "$tmp")
  local ganho; ganho=$(awk -v o="$sz_orig" -v n="$sz_novo" 'BEGIN{printf "%.1f", o?(1-n/o)*100:0}')
  if awk -v g="$ganho" -v m="$MIN_GANHO" 'BEGIN{exit !(g < m)}'; then
    msg "  ganho de apenas ${ganho}%, descartando saída: $base"
    rm -f "$tmp"
    printf 'MAGRO|%s|%s|%s\n' "$src" "$sz_orig" "$sz_orig" >&3
    return 0
  fi

  mv -f "$tmp" "$out"
  msg "  ok: $base  $(humano "$sz_orig") -> $(humano "$sz_novo")  (-${ganho}%)"

  if [[ $REPLACE -eq 1 ]]; then
    qdir="$dir/originais-substituidos"
    mkdir -p "$qdir" && mv -f "$src" "$qdir/" \
      && msg "     original movido para $qdir/" \
      || erro "não consegui mover o original: $src"
  fi

  printf 'OK|%s|%s|%s\n' "$src" "$sz_orig" "$sz_novo" >&3
  return 0
}
export -f processar tam duracao abitrate humano msg erro
export CODEC OUTDIR DRYRUN FORCE REPLACE AUDIO_MAX_K MIN_GANHO

# ─── Execução ─────────────────────────────────────────────────────────────────
RELATO="$(mktemp)"
trap 'rm -f "$RELATO"' EXIT

msg "Codec: $CODEC | CRF/CQ: $CRF | preset: $PRESET | paralelo: $JOBS"
msg "Arquivos: ${#ARQUIVOS[@]}"
[[ $DRYRUN -eq 1 ]] && msg "*** SIMULAÇÃO — nada será escrito ***"
msg ""

if [[ $JOBS -gt 1 ]]; then
  # xargs distribui os arquivos entre N shells; cada um herda 'processar'.
  printf '%s\0' "${ARQUIVOS[@]}" \
    | VARGS_STR="${VARGS[*]}" xargs -0 -P "$JOBS" -I{} \
        bash -c 'read -ra VARGS <<< "$VARGS_STR"; processar "$1" 3>>"$0"' "$RELATO" {}
else
  exec 3>>"$RELATO"
  for f in "${ARQUIVOS[@]}"; do processar "$f"; done
  exec 3>&-
fi

# ─── Resumo ───────────────────────────────────────────────────────────────────
msg ""
awk -F'|' -v OFS='' '
  { st[$1]++; if ($1=="OK") { orig+=$3; novo+=$4 } }
  END {
    print "───────────────────────────────────────────"
    printf "convertidos: %d   pulados: %d   sem ganho: %d   erros: %d\n",
           st["OK"]+0, st["PULADO"]+0, st["MAGRO"]+0, st["ERRO"]+0
    if (orig > 0) {
      printf "antes:  %.2f GB\ndepois: %.2f GB\nliberado: %.2f GB (-%.1f%%)\n",
             orig/2^30, novo/2^30, (orig-novo)/2^30, (1-novo/orig)*100
    }
    print "───────────────────────────────────────────"
  }' "$RELATO" >&2

if grep -q '^ERRO|' "$RELATO"; then
  msg ""
  msg "Falharam:"
  awk -F'|' '$1=="ERRO"{print "  " $2}' "$RELATO" >&2
fi

if [[ $REPLACE -eq 0 && $DRYRUN -eq 0 ]] && grep -q '^OK|' "$RELATO"; then
  msg ""
  msg "Os originais continuam onde estavam. Confira as saídas antes de apagar."
fi

grep -q '^ERRO|' "$RELATO" && exit 1 || exit 0
