namespace SOLIDPrinciples.OCP.Example1.Worst
{
    public class CalculadoraDePrecos
    {
        public double Calcular(Compra produto)
        {
            Frete correios = new Frete();

            double desconto;
            if (true /*REGRA 1*/)
            {
                TabelaDePrecoPadrao tabela = new TabelaDePrecoPadrao();
                desconto = tabela.DescontoPara(produto.Valor);
            }
            /*else*/if (true /*REGRA 2*/)
            {
                TabelaDePrecoDiferenciada tabela = new TabelaDePrecoDiferenciada();
                desconto = tabela.DescontoPara(produto.Valor);
            }

            double frete = correios.Para(produto.Cidade);

            return produto.Valor * (1 - desconto) + frete;

        }
    }
}
