# Fifteen minutes - algo traning

Training of the most commom algorithm techniques 

## Instructions to run scripts
### C#

To run scripts in dotnet, install dotnet-script tool:

run it into fifteen_minutes folder:
> dotnet tool install -g dotnet-script --configfile .\nuget.config

After that, execute the script .cs using:
> dotnet script {script-name}
## Algorithms Tehcnics

### Sliding Window

Reaproveitar o cálculo feito na janela anterior (ou na posição anterior da janela);

- Mantém dois ponteiros ou índices
- a cada passo, o ponteiro move e desliza a janela
- a janela pode ter tamanho fixo ou variavel (até satisfazer uma condição)
- durante o deslocamento, a informação é atualizada (soma, máximo, contagem de caracateres e etc), sem precisar recalcular do zero

