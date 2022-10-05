using SOLIDPrinciples.ValidationClass.Example02.WorstSolution;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SOLIDPrinciplesTest.ValidationClass.Example02
{
    public class WorstExampleTest
    {
        [Fact]
        public void WorstExample()
        {
            CalculadoraSalario calculadoraSalario = new CalculadoraSalario();
            Funcionario funcionario = new Funcionario
            {
                Cargo = Cargo.DESENVOLVEDOR,
                Salario = 1000
            };

            var resultado = calculadoraSalario.Calcula(funcionario);

            Assert.Equal(900, resultado);
        }
    }
}
