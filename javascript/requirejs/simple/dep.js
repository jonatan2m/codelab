define(['hello-world', 'dep'], function(h, dep){
	var module = {};

	console.log(dep.config().year,dep.config().month, dep.config().day);

	module.show = function(){
		h.show('Carregado como dependencia');
		var div = document.createElement('div');
		div.innerHTML = '<b>Dep Carregado</b>';

		var body = document.querySelector('body')
		body.appendChild(div);
	};

	return module;
})