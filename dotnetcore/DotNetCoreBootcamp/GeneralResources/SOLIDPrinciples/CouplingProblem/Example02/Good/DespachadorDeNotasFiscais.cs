using System;

namespace SOLIDPrinciples.CouplingProblem.Example02.Good
{
    public class DespachadorDeNotasFiscais
    {
        private NFDao dao;
        private CalculadorDeImposto impostos;
        private EntregadoresDeNFs entregadoresDeNFs;


        public DespachadorDeNotasFiscais(
        NFDao dao,
        CalculadorDeImposto impostos,
        EntregadoresDeNFs entregadoresDeNFs) =>
        (this.dao, this.impostos, this.entregadoresDeNFs) =
            (dao, impostos, entregadoresDeNFs);

        public void Processar(NotaFiscal nf)
        {
            double imposto = impostos.Aplicar(nf);
            nf.SetImposto(imposto);

            entregadoresDeNFs.DeterminarFormaDeEnvio(nf);

            dao.Persiste(nf);
        }
    }
}
