using System;

namespace SOLIDPrinciples.CouplingProblem.Example02.Good
{
    public class LeiDeEntrega
    {
        public bool DeveEntregarUrgente(NotaFiscal nf)
        {
            return true;
        }
    }
}