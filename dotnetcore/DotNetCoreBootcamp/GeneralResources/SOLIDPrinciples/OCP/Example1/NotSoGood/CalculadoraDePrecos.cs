namespace SOLIDPrinciples.OCP.Example1.NotSoGood
{
    public class CalculadoraDePrecos
    {
        public double Calcular(Compra produto)
        {
            TabelaDePrecoPadrao tabela = new TabelaDePrecoPadrao();
            Frete correios = new Frete();

            double desconto = tabela.DescontoPara(produto.Valor);
            double frete = correios.Para(produto.Cidade);

            return produto.Valor * (1 - desconto) + frete;

        }
    }
}
