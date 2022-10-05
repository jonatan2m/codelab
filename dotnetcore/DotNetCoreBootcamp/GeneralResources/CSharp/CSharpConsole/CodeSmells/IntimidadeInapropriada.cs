using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpConsole.CodeSmells
{
    class IntimidadeInapropriada
    {
        /*
         * É quando uma classe sabe demais sobre a outra.
         * O código faz perguntas para o objeto e, dada a resposta, ele decide 
         * marca-la como importante.
         * Essa decisão deveria estar dentro de NotaFiscal.
         * Veja que é fácil esquecer de marca-la como importante, além de ter
         * essa regra de negócio vazada por inúmeros lugares
         * 
         //Metodo da classe Gerenciador
         public	void processa(NotaFiscal	nf) {
            if(nf.isEncerrada()	&&	nf.getValor()	>	5000)	{
				nf.marcaComoImportante();
			}	
        }
         */
    }
}
