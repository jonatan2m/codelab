using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.TemplateMethod.Example1
{
    public class GeradorPropriedadesCriptografado : GeradorArquivo
    {
        private int delay;
        public GeradorPropriedadesCriptografado(int delay)
        {
            this.delay = delay;
        }

        protected override byte[] Processar(byte[] bytes)
        {
            Console.WriteLine("Aplica criptografia");
            return bytes;
        }

        protected override string GerarConteudo(Dictionary<string, object> propriedades)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var prop in propriedades)
            {
                builder.Append($"{prop.Key}={prop.Value}\n");
            }
            return builder.ToString();
        }
    }
}
