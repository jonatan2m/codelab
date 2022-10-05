using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SOLIDPrinciples.Encapsulation.Example1.Good
{
    public class ProcessadorDeBoletos
    {
        public void Processar(List<Boleto> boletos, Fatura fatura)
        {
            double total = 0;
            foreach (var boleto in boletos)
            {
                Pagamento pagamento = new Pagamento(boleto.Valor,
                    MeioDePagamento.BOLETO);

                fatura.AdicionarPagamento(pagamento);
                total += boleto.Valor;
            }
            
        }
    }
}
