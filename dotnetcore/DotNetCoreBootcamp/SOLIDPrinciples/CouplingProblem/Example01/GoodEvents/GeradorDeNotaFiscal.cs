using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.CouplingProblem.Example01.GoodEvents
{
    public class GeradorDeNotaFiscal
    {
        public event EventHandler<NotaFiscal> NotaFiscalGerada;
        public NotaFiscal Gera(Fatura fatura)
        {
            double valor = fatura.GetValorMensal();
            var nf = new NotaFiscal(
                valor,
                ImpostoSimplesSobreO(valor));
            
            NotaFiscalGerada?.Invoke(this, nf);
         
            return nf;
        }

        private double ImpostoSimplesSobreO(double valor)
        {
            return valor * 0.06;
        }
    }
}
