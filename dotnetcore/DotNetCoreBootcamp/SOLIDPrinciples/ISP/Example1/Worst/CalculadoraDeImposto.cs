using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ISP.Example1.Worst
{
    public class NotaFiscal
    {
        List<Item> itens;

        public NotaFiscal(
            Cliente cliente,
            List<Item> itens,
            List<Desconto> descontos,
            Endereco entrega,
            Endereco cobranca,
            FormaDePagamento pgto,
            double valorTotal)
        {
            //outros metodos e implementações
            this.itens = itens;

        }

        public List<Item> GetItems()
        {
            return itens;
        }
    }

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
