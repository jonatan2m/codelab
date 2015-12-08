define(['soma', 'subtracao', 'divisao', 'multiplicacao'],
	function(soma, subtracao, divisao, multiplicacao){

	var module = {};
	
	module.somar = function (x, y){
		return soma.do(x, y);
	}

	module.somaValor = function (){
		return soma.valor;
	}

	module.subtracao = function (x, y){
		return subtracao.do(x, y);
	}

	module.divisao = function (x, y){
		return divisao.do(x, y);
	}

	module.multiplicacao = function (x, y){
		return multiplicacao.do(x, y);
	}

	return module;
});