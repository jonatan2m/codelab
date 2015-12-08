//carregamento asincrono utilizado hoje
//define(['dep', 'dep2', 'dep3'], function(dep, dep2, dep3){

//carregamento asincrono mas sem declarar as variaveis;
//define(function(require){

define(['require', 'dep', 'dep2', 'dep3'], function(require){
	var dep = require('dep'),
		dep2 = require('dep2'),
		dep3 = require('dep3');

	var module = {};

	module.showDep = function(){
		dep.show();
	};
	module.showDep2 = function(){
		dep2.show();
	};
	module.showDep3 = function(){
		require('dep3').show();
	};

	return module;
})