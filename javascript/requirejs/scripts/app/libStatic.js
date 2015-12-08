var libStatic = libStatic || {};
(function (s) {
	s.valor = 0;

	s.total = function(){
		console.log('libStatic carregando...');
		s.valor = libStatic2.valor + libStatic3.valor;
	};
	
	return s;

}(libStatic));