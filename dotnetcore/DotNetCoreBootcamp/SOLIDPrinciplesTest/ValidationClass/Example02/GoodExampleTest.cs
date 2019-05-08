using SOLIDPrinciples.ValidationClass.Example02.GoodSolution;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SOLIDPrinciplesTest.ValidationClass.Example02
{
    public class GoodExampleTest
    {
        [Fact]
        public void GoodExampleDesenvolvedor()
        {            
            Funcionario funcionario = new Funcionario
            {
                Cargo = Cargo.DESENVOLVEDOR,
                Salario = 1000
            };

            var resultado = funcionario.AplicarRegraDeCalculoParaCargo();

            Assert.Equal(900, resultado);
        }

        [Fact]
        public void GoodExampleDba()
        {            
            Funcionario funcionario = new Funcionario
            {
                Cargo = Cargo.DBA,
                Salario = 1000
            };

            var resultado = funcionario.AplicarRegraDeCalculoParaCargo();

            Assert.Equal(850, resultado);
        }

        [Fact]
        public void GoodExampleTester()
        {   
            Funcionario funcionario = new Funcionario
            {
                Cargo = Cargo.TESTER,
                Salario = 10000
            };

            var resultado = funcionario.AplicarRegraDeCalculoParaCargo();

            Assert.Equal(7500, resultado);
        }
    }
}
