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

    public class Pagamento
    {
        public readonly double valor;
        private readonly MeioDePagamento meioDePagamento;

        public Pagamento(double valor, MeioDePagamento meioDePagamento)
        {
            this.valor = valor;
            this.meioDePagamento = meioDePagamento;
        }
    }

    public class Fatura
    {
        public List<Pagamento> Pagamentos { get; internal set; } = new List<Pagamento>();
        public double Valor { get; set; }

        public bool Pago { get; private set; }

        public void AdicionarPagamento(Pagamento pagamento)
        {
            Pagamentos.Add(pagamento);
            double total = Pagamentos.Sum(x => x.valor);
            if (total >= Valor)
                Pago = true;
        }
    }

    public class Boleto
    {
        public double Valor { get; internal set; }
    }

    public enum MeioDePagamento
    {
        BOLETO
    }
}
