using System;
namespace SOLIDPrinciples.CouplingProblem.Example02.Good
{
    public class EntregadoresDeNFs
    {
        LeiDeEntrega lei;
        Correios correios;
        public EntregadoresDeNFs(LeiDeEntrega lei,
        Correios correios) =>
            (this.lei, this.correios) = (lei, correios);

        public void DeterminarFormaDeEnvio(NotaFiscal nf)
        {
            //throw new NotImplementedException();
        }
    }
}
