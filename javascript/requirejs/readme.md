Fundamentos do RequireJS
- Web Module
- É uma implementação para construir aplicações usando o padrão Asynchronous Module Definition (AMD)
Problemas que resolve
	- Web sites são se tornando Web apps
	- Complexidade do código aumenta conforme o site vai aumentando
	- Trabalhar com assemblies é dificil (Module.Custom.Implementation)
	- Queremos separar bem as responsabilidades
	- Otimizar o deploy do código fonte em uma ou poucas chamadas HTTP

- Apenas um módulo definido por arquivo.
[EXEMPLO SIMPLE]
criação de módulo com e sem dependência.

Quem sabe como carregar um script na página?

#http://requirejs.org/docs/why.html
#http://requirejs.org/docs/whyamd.html


- Retirando as dependências do Módulo do RequireJS
[Exemplo Sugar]
Com esse exemplo já ajuda bastante para escrever os módulos que possuem muitas dependências.

- Os arquivos JS são carregados com atributo async (tag script), o que impossibilita definir uma ordem.
data-main

Separar o arquivo de configuração.
Colocar antes da declaração do require


REQUIREJS OPTIMIZER
requer o node instalado
Baixar o r.js
Rodar o comando abaixo
node r.js -o baseUrl=.. name=hello-world out=teste.js

é possivel especificar um arquivo com essas opções de build.
node r.js -o build.js
é aconselhavel utilizar esse arquivo caso exista algum módulo que possua ponto (.) no nome. jquery.tabs, por exemplo.

DEBUGAR UM MÓDULO
require('nome_do_modulo').funcao_ou_valor()

ANALISAR AS DEPENDENCIAS JÁ CARREGAS
//lista de todos os módulos já carregados
var all=[];
require.onResourceLoad = function (context, map, depArray) {
	all.push(map.name);    
};


CONFIGURAÇÃO
baseUrl - pode apontar para outro dominio. A única restrição é parada text!plugin resource
path fallbacks - ter opções para pegar o mesmo arquivo. vai na CDN, dá erro, vai local.
map: configurar diferentes versões do mesmo módulo.
config: define configurações para os módulos. acessivel por, nome_do_modulo.config().algo (não funcionou nos exemplos, melhor criar um módulo nomeado e invoca-lo pelo requirejs)
context: permite carregar multiplos módulos
deps: dependênncias
callbacks: função executada depois que o deps for carregado
enforceDefine: se for true, um erro é disparado ao carregar um script sem define ou ter um valor no shim.
urlArgs: extra na query string. ajuda a controlar a versão do arquivo (só usar em dev)

RequireJS, carrega plugins como text, i18n
RequireJS usa a propriedade navigator.languages do browser para definir a sua configuração. (idioma)

