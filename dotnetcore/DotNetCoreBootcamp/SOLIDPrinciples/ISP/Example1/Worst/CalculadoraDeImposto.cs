using System;
using System.Text;

namespace SOLIDPrinciples.ISP.Example1.Worst
{
    public class CalculadoraDeImposto
    {
        public double Calcular(NotaFiscal nf)
        {
            double total = 0;
            foreach (var item in nf.GetItems())
            {
                if (item.Valor > 1000)
                    total += item.Valor * 0.02;
                else
                    total += item.Valor * 0.01;
            }
            return total;
        }
    }
}
