var Static = Static || {};
(function (s) {
	s.valor = 0;

	s.total = function(){
		console.log('libStatic carregando...');
		s.valor = libStatic2.valor + libStatic3.valor;
		console.log(s.valor);
	};
	
	return s;

}(Static));