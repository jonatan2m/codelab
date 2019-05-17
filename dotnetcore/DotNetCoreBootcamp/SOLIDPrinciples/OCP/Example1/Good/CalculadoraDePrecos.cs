namespace SOLIDPrinciples.OCP.Example1.Good
{
    public class CalculadoraDePrecos
    {
        private readonly ITabelaDePreco tabela;
        private readonly IServicoDeEntrega entrega;

        public CalculadoraDePrecos(
            ITabelaDePreco tabela,
            IServicoDeEntrega entrega) =>
            (this.tabela, this.entrega) = (tabela, entrega);


        public double Calcular(Compra produto)
        {
            double desconto = tabela.DescontoPara(produto.Valor);

            double frete = entrega.Para(produto.Cidade);

            return produto.Valor * (1 - desconto) + frete;

        }
    }
}
