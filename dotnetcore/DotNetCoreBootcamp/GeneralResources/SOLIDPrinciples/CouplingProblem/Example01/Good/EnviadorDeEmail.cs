using System;

namespace SOLIDPrinciples.CouplingProblem.Example01.Good
{
    public class EnviadorDeEmail : IAcaoAposGerarNota
    {
        public void Executar(NotaFiscal nf)
        {
            //Execution
        }
    }
}