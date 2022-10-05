using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpConsole.CodeSmells
{
    class RefusedBequest
    {
        /*
         * 
         class Matematica	{
            public	int	quadrado(int	a,	int	b) {}
            public	int	raiz(int	a) {}
         }
         class NotaFiscal extends Matematica	{
                public	double	calculaImposto() {
                //	alguma	conta	qualquer
                    return	quadrado(this.valor,	this.qtd);
				}
         }
         
        //	uma	classe	cliente	qualquer
        NotaFiscal	nf	=	new	NotaFiscal();
        algumComportamento(nf);
            
        public	void	algumComportamento(Matematica	m) {
            //	aqui	pode	vir	uma	nota	fiscal,	mas
            //	não	queremos	uma	nota	fiscal...
            //	queremos	matematica!
        }
         */
    }
}
