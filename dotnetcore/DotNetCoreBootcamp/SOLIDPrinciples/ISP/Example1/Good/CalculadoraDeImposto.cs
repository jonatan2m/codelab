using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ISP.Example1.Good
{
    public interface ITributavel
    {
        List<Item> ItensASeremTributados();
    }

    public class NotaFiscal : ITributavel
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

        //O metodo GetItens passou a ser definido por uma interface e com isso
        //pode ter extraido para uma interface.
        public List<Item> ItensASeremTributados()
        {
            return itens;
        }
    }

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
