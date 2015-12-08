define(function(){
	var module = {};

	module.index = 0;

	module.show = function(){		
		module.index++;
		var div = document.createElement('div');
		div.innerHTML = '<b>Dep 2 ('+ module.index +')</b>';
		var body = document.querySelector('body')
		body.appendChild(div);
	};

	return module;
})