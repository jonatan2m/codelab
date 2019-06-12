using SOLIDPrinciples.OCP.Example1;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NotSoGood = SOLIDPrinciples.OCP.Example1.NotSoGood;
using Good = SOLIDPrinciples.OCP.Example1.Good;
namespace SOLIDPrinciplesTest.OCP.Examples
{
    public class OCPTests
    {
        [Fact]
        public void OCP_Example1()
        {
            NotSoGood.CalculadoraDePrecos calculadoraDePrecos = new NotSoGood.CalculadoraDePrecos();
            var result = calculadoraDePrecos.Calcular(new NotSoGood.Compra
            {
                Cidade = "RIO DE JANEIRO",
                Valor = 100
            });

            Assert.Equal(130, result);
        }

        [Fact]
        public void OCP_Example1_Good()
        {
            Good.CalculadoraDePrecos calculadoraDePrecos =
                new Good.CalculadoraDePrecos(
                    new Good.TabelaDePreco2(),
                    new Good.Frete1());

            var result = calculadoraDePrecos.Calcular(new Good.Compra
            {
                Cidade = "SAO PAULO",
                Valor = 100
            });

            Assert.Equal(115, result);

        }
    }
}
