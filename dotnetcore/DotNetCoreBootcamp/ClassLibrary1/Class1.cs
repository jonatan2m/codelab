using System;
using System.Collections.Generic;
using System.Linq;

namespace TalkExamples
{
    public enum Flags
    {
        ON,
        OFF,
        FECHAR,
        COR
    }
    public class ClasseQueFazTudo
    {
        private string font;
        private string windowSize;
        private Xpto xpto;

        public string ProcuraPadrao(string textoBase, string padrao)
        {
            //Código que procura um padrão
            return "algum resultado";
        }

        public string ColocarPrimeiraLetraDaFraseMaiuscula(string frase) =>
            $"{char.ToUpper(frase[0])}{frase.Substring(1)}";

        public int CalcularAreaDoRetangulo(int @base, int altura)
        {
            return @base * altura;
        }

        public void Tratar(Flags flag)
        {
            switch (flag)
            {
                case Flags.ON:
                    // coisas para tratar de ON
                    break;
                case Flags.OFF:
                    // coisas para tratar de OFF
                    break;
                case Flags.FECHAR:
                    // coisas para tratar de FECHAR
                    break;
                case Flags.COR:
                    // coisas para tratar de COR
                    break;
            }
        }

        public void InicializarDados()
        {
            font = "times";
            windowSize = "200,400";
            xpto.nome = "desligado";
            xpto.tamanho = 12;
            xpto.localização = "/usr/local/lib/java";
        }
    }

    //Usar construtores e destrutores
    public class Xpto
    {
        public string nome;
        public int tamanho;
        public string localização;

        public Xpto()
        {
            this.nome = "desligado";
            this.tamanho = 12;
            this.localização = "/usr/local/lib/java";
        }
    }

    public class GeradorDeRelatorio : ClasseQueFazTudo
    {
        public string GerarRelatorio(List<string> frases)
        {
            var frasesModificadas = frases.Select(ColocarPrimeiraLetraDaFraseMaiuscula);
            return string.Join(Environment.NewLine, frasesModificadas);
        }
    }




  


}
