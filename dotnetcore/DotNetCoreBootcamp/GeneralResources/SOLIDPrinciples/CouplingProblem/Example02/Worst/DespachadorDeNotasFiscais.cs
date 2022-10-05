using System;

namespace SOLIDPrinciples.CouplingProblem.Example02.Worst
{
    public class DespachadorDeNotasFiscais
    {
        private NFDao dao;
        private CalculadorDeImposto impostos;
        private LeiDeEntrega lei;
        private Correios correios;

        public DespachadorDeNotasFiscais(
        NFDao dao,
        CalculadorDeImposto impostos,
        LeiDeEntrega lei,
        Correios correios) =>
        (this.dao, this.impostos, this.lei, this.correios) =
            (dao, impostos, lei, correios);

        public void Processar(NotaFiscal nf)
        {
            double imposto = impostos.Aplicar(nf);
            nf.SetImposto(imposto);

            if (lei.DeveEntregarUrgente(nf))
            {
                correios.EnviarPorSedex10(nf);
            }
            else
            {
                correios.EnviarPorSedexComUm(nf);
            }

            dao.Persiste(nf);
        }
    }
}
