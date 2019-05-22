using System.Collections.Generic;

namespace SOLIDPrinciples.ISP.Example1.Good
{
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
}
