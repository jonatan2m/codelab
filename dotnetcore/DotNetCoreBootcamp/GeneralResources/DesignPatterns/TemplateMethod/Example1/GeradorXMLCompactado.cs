using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.TemplateMethod.Example1
{
    public class GeradorXMLCompactado : GeradorArquivo
    {
        protected override byte[] Processar(byte[] bytes)
        {            
            Console.WriteLine("Faz modificações para comprimir o arquivo.");
            return bytes;
        }

        protected override string GerarConteudo(Dictionary<string, object> propriedades)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var prop in propriedades)
            {
                builder.Append($"<{prop.Key}> {prop.Value} </{prop.Key}>");
            }
            return builder.ToString();
        }
    }
}
