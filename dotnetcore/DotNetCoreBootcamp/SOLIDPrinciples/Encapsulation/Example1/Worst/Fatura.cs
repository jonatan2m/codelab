using System.Collections.Generic;
using System.Linq;

namespace SOLIDPrinciples.Encapsulation.Example1.Worst
{
    public class Fatura
    {
        public List<Pagamento> Pagamentos { get; internal set; } = new List<Pagamento>();
        public double Valor
        {
            get
            {
                return Pagamentos.Sum(x => x.valor);
            }
        }
        public bool Pago { get; internal set; }
    }
}
