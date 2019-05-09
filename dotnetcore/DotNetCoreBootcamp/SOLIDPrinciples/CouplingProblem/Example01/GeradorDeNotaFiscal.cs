using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.CouplingProblem.Example01
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
                impostoSimplesSobreO(valor));

            email.EnviarEmail(nf);
            dao.Persiste(nf);

            return nf;
        }

        private double impostoSimplesSobreO(double valor)
        {
            return valor * 0.06;
        }

        public class Fatura
        {
            public double GetValorMensal()
            {
                throw new NotImplementedException();
            }
        }
    }
}
