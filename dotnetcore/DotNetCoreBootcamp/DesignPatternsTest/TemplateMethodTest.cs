using DesignPatterns.TemplateMethod.Example1;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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
    }
}
