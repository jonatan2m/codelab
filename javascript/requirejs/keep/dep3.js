define(function(){
	var module = {};

	var index = 0;

	module.show = function(){		
		index++;
		var div = document.createElement('div');
		div.innerHTML = '<b>Dep 3</b>';
		var body = document.querySelector('body')
		body.appendChild(div);
	};

	module.getIndex = function (){
		console.log(index);
	};

	return module;
})