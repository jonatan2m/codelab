var require = {
			baseUrl: 'scripts/app',			
			deps:['calc','scripts/globals/libStatic.js'],			
			callback: function (calc){
				calc.somar(1,2);
				console.log(Static, calc.somar(1,2), calc.somaValor());
			},
			paths:{
				out: '../',
				path2: '../pasta1/pasta2/',
				fallb: ['http://google.com/cdn/fallback', 'fallback']
			},
			shim:{
				'libStatic' : {
					deps: ['libStatic2','../globals/libStatic3'],
					exports: 'Static'
				}
			}						
		};

