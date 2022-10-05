using System;
using System.Text;

namespace SOLIDPrinciples.ISP.Example1.Good
{
    public class CalculadoraDeImposto
    {
        public double Calcular(ITributavel t)
        {
            double total = 0;
            foreach (var item in t.ItensASeremTributados())
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
