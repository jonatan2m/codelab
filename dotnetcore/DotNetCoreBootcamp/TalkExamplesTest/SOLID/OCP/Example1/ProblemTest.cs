using System;
using System.Collections.Generic;
using System.Text;
using TalkExamples.SOLID.OCP.Example1;
using Xunit;

namespace TalkExamplesTest.SOLID.OCP.Example1
{
    public class CalculoBonificacaoTeste
    {
        [Fact]
        public void DeveAplicarDezPorCentoDeBonusQuandoFuncionarioForDiretor()
        {
            //Arrange
            Funcionario diretor = new Funcionario(new Diretor(), 10000);

            //Act
            decimal bonus = diretor.CalcularBonificacao();

            //Assert
            Assert.Equal(1000, bonus);
        }

        [Fact]
        public void DeveAplicarVintePorCentoDeBonusQuandoFuncionarioForDesenvolvedor()
        {
            //Arrange
            Funcionario desenvolvedor = new Funcionario(new Desenvolvedor(), 10000);

            //Act
            decimal bonus = desenvolvedor.CalcularBonificacao();

            //Assert
            Assert.Equal(2000, bonus);
        }

        [Fact]
        public void DeveAplicarVintePorCentoDeBonusQuandoFuncionarioForTestador()
        {
            //Arrange
            var testador = new Funcionario(new Testador(), 6000);

            //Act
            decimal bonus = testador.CalcularBonificacao();

            //Assert
            Assert.Equal(1200, bonus);
        }

        [Fact]
        public void NaoDeveAplicarBonusQuandoFuncionarioForDeMarketing()
        {
            //Arrange
            var marketing = new Funcionario(new Marketing(), 5000);

            //Act
            decimal bonus = marketing.CalcularBonificacao();

            //Assert
            Assert.Equal(0, bonus);
        }
    }
}
