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
            this.itens = itens;
        }

        //outros metodos e implementações
        public List<Item> GetItems()
        {
            return itens;
        }
    }
}
