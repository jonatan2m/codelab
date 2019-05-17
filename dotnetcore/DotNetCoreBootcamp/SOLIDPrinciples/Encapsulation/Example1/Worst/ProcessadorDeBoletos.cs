using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SOLIDPrinciples.Encapsulation.Example1.Worst
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

                fatura.Pagamentos.Add(pagamento);
                total += boleto.Valor;
            }
            //Essa regra de negócio não deveria estar aqui e sim dentro de fatura
            //Se surgirem outros Processadores, essa regra devera ser replicada.
            if (total >= fatura.Valor)
                fatura.Pago = true;
        }
    }
}
