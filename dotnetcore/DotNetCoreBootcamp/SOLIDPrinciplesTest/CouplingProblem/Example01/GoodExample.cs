using System;
using System.Collections.Generic;
using SOLIDPrinciples.CouplingProblem.Example01.Good;
using Xunit;

namespace SOLIDPrinciplesTest.CouplingProblem.Example01
{
    public class GoodExample
    {
       [Fact]
       public void Test01()
        {
            GeradorDeNotaFiscal geradorDeNota = new GeradorDeNotaFiscal(
            new List<IAcaoAposGerarNota>
            {
                new EnviadorDeEmail(),
                new NotaFiscalDao()
            });

            geradorDeNota.Gera(new Fatura());
        }
    }
}
