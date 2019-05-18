using System.Collections.Generic;

namespace SOLIDPrinciples.Encapsulation.Example1.Good
{
    public class Fatura
    {
        public List<Pagamento> Pagamentos { get; internal set; } = new List<Pagamento>();
        public double Valor { get; set; }

        public bool Pago
        {
            get
            {
                return Pagamentos.Sum(x => x.valor) >= Valor;
            }
        }

        public void AdicionarPagamento(Pagamento pagamento)
        {
            Pagamentos.Add(pagamento);
        }
    }
}
