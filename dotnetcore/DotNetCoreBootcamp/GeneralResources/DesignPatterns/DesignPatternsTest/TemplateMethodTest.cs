using DesignPatterns.Strategy.LoanExample;
using DesignPatterns.TemplateMethod.Example1;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Worse = DesignPatterns.TemplateMethod.CapitalStrategy.Worse;
namespace DesignPatternsTest
{
    public class TemplateMethodTest
    {
        [Fact]
        public void TemplateMethod_Example1()
        {
            GeradorArquivo geradorArquivo = new GeradorXMLCompactado();
            geradorArquivo.GerarArquivo("teste", new Dictionary<string, object>
            {
                {"body", "examples" },
                {"div", "conteudo da div" },
                {"p", 1 }
            });
        }

        [Fact]
        public void TemplateMethod_CapitalStrategy_Worse()
        {
            Worse.CapitalStrategy strategy = new Worse.CapitalStrategyAdviseLine();
            var loan = new Loan(2, null, null, null, 3, null);

            var result = strategy.Capital(loan);

            Assert.Equal(945, result);
        }

        [Fact]
        public void TemplateMethod_CapitalStrategy_Good()
        {
            Worse.CapitalStrategy strategy = new Worse.CapitalStrategyAdviseLine();
            var loan = new Loan(2, null, null, null, 3, null);

            var result = strategy.Capital(loan);

            Assert.Equal(945, result);
        }
    }
}
