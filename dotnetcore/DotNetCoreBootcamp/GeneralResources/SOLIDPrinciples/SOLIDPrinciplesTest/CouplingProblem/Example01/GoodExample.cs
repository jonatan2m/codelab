using System;
using System.Collections.Generic;
using Ex01 = SOLIDPrinciples.CouplingProblem.Example01;
using Xunit;

namespace SOLIDPrinciplesTest.CouplingProblem.Example01
{
    public class GoodExample
    {
       [Fact]
       public void Test01()
        {
            Ex01.Good.GeradorDeNotaFiscal geradorDeNota = new Ex01.Good.GeradorDeNotaFiscal(
            new List<Ex01.Good.IAcaoAposGerarNota>
            {
                new Ex01.Good.EnviadorDeEmail(),
                new Ex01.Good.NotaFiscalDao()
            });

            geradorDeNota.Gera(new Ex01.Good.Fatura());
        }

        [Fact]
        public void Test02()
        {
            Ex01.GoodEvents.GeradorDeNotaFiscal geradorDeNota =
                new Ex01.GoodEvents.GeradorDeNotaFiscal();

            var notaFiscalDao = new Ex01.GoodEvents.NotaFiscalDao();
            var enviadorDeEmail = new Ex01.GoodEvents.EnviadorDeEmail();

            geradorDeNota.NotaFiscalGerada += notaFiscalDao.Executar;
            geradorDeNota.NotaFiscalGerada += enviadorDeEmail.Executar;

            geradorDeNota.Gera(new Ex01.GoodEvents.Fatura());
        }

        [Fact]
        public void Test03()
        {
            Ex01.GoodAction.GeradorDeNotaFiscal geradorDeNota =
                new Ex01.GoodAction.GeradorDeNotaFiscal();

            var notaFiscalDao = new Ex01.GoodAction.NotaFiscalDao();
            var enviadorDeEmail = new Ex01.GoodAction.EnviadorDeEmail();

            geradorDeNota.NotaFiscalGerada += notaFiscalDao.Executar;
            geradorDeNota.NotaFiscalGerada += enviadorDeEmail.Executar;

            geradorDeNota.Gera(new Ex01.GoodAction.Fatura());
        }
    }
}
