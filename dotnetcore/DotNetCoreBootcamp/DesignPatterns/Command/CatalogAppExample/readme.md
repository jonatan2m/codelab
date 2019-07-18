Command
Lógica condicional é usada para enviar requisições e executar ações.

Crie um comando para cada ação. Armazene os comandos em uma coleção e substitua a lógica
condicional por código para obter e executar comandos.

- Extrair a lógica de dentro de cada if/else para um método separado.
- Criar classes concretas para cada método que foi criado.
  Como esses métodos estão usando atributos da classe que foram declarados,
  passar a instancia da classe no construtor do Comando
- Caso exista muita lógica dentro desse método, aplicar Compor Método (153) para
  agrupar melhor esse código
- Agora crio uma interface comum a todos esses Comandos (algo como execute() ou run()).
  Nesse momento todas as classes de comandos devem possuir métodos com nome diferente,
  mas que no fundo estão executando algo, a ação que foi definida.
  Pode ser uma interface ou uma classe abstrata.
  Concentrar-se em qual será a melhor assinatura pra esse método em comum.
- Todos os comandos devem ser alterados para implementar esse contrato
- Nesse momento já devemos ter algo como um mapa de comandos.
  if(a) new a(), if(b) new b e etc...
  crie uma coleção para eliminar todas as condicionais.
  ["a", new a()]
  ["b", new b()]
  Faça essa criação no construtor da classe que faz a invocação dos comandos
  Caso haja muitos comandos, será melhor transformar isso num Plugin,
  como descrito em [Flower, PEAA]
- Nesse momento já deve ser possivel que a classe invocadora só utilize os comandos.