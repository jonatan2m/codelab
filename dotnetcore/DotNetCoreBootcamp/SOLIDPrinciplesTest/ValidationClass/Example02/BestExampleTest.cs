using SOLIDPrinciples.ValidationClass.Example02.BestSolution;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SOLIDPrinciplesTest.ValidationClass.Example02
{
    public class BestExampleTest
    {
        [Fact]
        public void BestExampleDesenvolvedor()
        {
            CalculadoraSalario calculadoraSalario = new CalculadoraSalario();
            Funcionario funcionario = new Funcionario
            {
                Cargo = Cargo.DESENVOLVEDOR,
                Salario = 1000
            };

            var resultado = funcionario.AplicarRegraDeCalculoParaCargo();

            Assert.Equal(900, resultado);
        }

        [Fact]
        public void BestExampleDba()
        {
            CalculadoraSalario calculadoraSalario = new CalculadoraSalario();
            Funcionario funcionario = new Funcionario
            {
                Cargo = Cargo.DBA,
                Salario = 1000
            };

            var resultado = funcionario.AplicarRegraDeCalculoParaCargo();

            Assert.Equal(850, resultado);
        }

        [Fact]
        public void BestExampleTester()
        {
            CalculadoraSalario calculadoraSalario = new CalculadoraSalario();
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
