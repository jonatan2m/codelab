namespace SOLIDPrinciples.OCP.Example1.Good
{
    public class TabelaDePreco2 : ITabelaDePreco
    {
        public double DescontoPara(double valor)
        {
            if (valor > 5000) return 0.05;
            if (valor > 1000) return 0.07;
            return 0;
        }
    }
}
