using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DesignPatterns.TemplateMethod.Example1
{
    public abstract class GeradorArquivo
    {
        public void GerarArquivo(string nome,
            Dictionary<string, object> propriedades)
        {
            string conteudo = GerarConteudo(propriedades);
            byte[] bytes = Encoding.Unicode.GetBytes(conteudo);

            bytes = Processar(bytes);
            var file = new FileStream(nome, FileMode.OpenOrCreate);
            file.Write(bytes);
            file.Close();
        }

        protected virtual byte[] Processar(byte[] bytes)
        {
            //permite que alguma transformação seja feita na subclasse
            return bytes;
        }

        protected abstract string GerarConteudo(Dictionary<string, object> propriedades);
    }
}
