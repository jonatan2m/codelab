define(function(){
	var module = {};

	module.show = function(msg){
		alert(msg || 'Hello World');
	};


	return module;
})