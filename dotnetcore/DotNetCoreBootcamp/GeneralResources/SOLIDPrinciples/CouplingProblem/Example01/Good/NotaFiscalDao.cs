using System;

namespace SOLIDPrinciples.CouplingProblem.Example01.Good
{
    public class NotaFiscalDao : IAcaoAposGerarNota
    {
        public void Executar(NotaFiscal nf)
        {
            //Execution
        }
    }
}