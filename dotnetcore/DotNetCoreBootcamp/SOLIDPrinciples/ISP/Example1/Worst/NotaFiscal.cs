using System.Collections.Generic;

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
}
