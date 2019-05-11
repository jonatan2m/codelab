using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.CouplingProblem.Example01.Good
{
    public partial class GeradorDeNotaFiscal
    {
        private readonly List<IAcaoAposGerarNota> acoes;

        public GeradorDeNotaFiscal(List<IAcaoAposGerarNota> acoes) =>
            (this.acoes) = (acoes);

        public NotaFiscal Gera(Fatura fatura)
        {
            double valor = fatura.GetValorMensal();
            var nf = new NotaFiscal(
                valor,
                impostoSimplesSobreO(valor));

            foreach (var acao in acoes)
            {
                acao.Executar(nf);
            }

            return nf;
        }

        private double impostoSimplesSobreO(double valor)
        {
            return valor * 0.06;
        }
    }
}
