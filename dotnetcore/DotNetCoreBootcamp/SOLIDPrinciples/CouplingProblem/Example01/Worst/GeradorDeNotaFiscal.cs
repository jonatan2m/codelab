using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.CouplingProblem.Example01.Worst
{
    public class GeradorDeNotaFiscal
    {
        private readonly EnviadorDeEmail email;
        private readonly NotaFiscalDao dao;

        public GeradorDeNotaFiscal(EnviadorDeEmail email, NotaFiscalDao dao) =>
            (this.email, this.dao) = (email, dao);

        public NotaFiscal Gera(Fatura fatura)
        {
            double valor = fatura.GetValorMensal();
            var nf = new NotaFiscal(
                valor,
                ImpostoSimplesSobreO(valor));

            email.EnviarEmail(nf);
            dao.Persiste(nf);

            return nf;
        }

        private double ImpostoSimplesSobreO(double valor)
        {
            return valor * 0.06;
        }
    }
}
