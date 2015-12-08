define(function(){

	var module = {};
	module.valor = 0;
	module.do = function (x, y){
		module.valor++;
		return x / y;
	}

	return module;
});