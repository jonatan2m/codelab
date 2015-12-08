define(function(){
	var module = {};

	module.show = function(){		
		var div = document.createElement('div');
		div.innerHTML = '<b>Fora da pasta script/app - Usando Path</b>';

		var body = document.querySelector('body')
		body.appendChild(div);
	};


	return module;
})