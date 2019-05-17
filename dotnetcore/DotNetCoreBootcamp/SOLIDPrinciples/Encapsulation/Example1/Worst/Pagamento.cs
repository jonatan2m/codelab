namespace SOLIDPrinciples.Encapsulation.Example1.Worst
{
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
}
