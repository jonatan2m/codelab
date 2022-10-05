namespace SOLIDPrinciples.OCP.Example1.Good
{
    public class TabelaDePreco3 : ITabelaDePreco
    {
        public double DescontoPara(double valor)
        {
            if (valor > 5000) return 0.04;
            if (valor > 1000) return 0.02;
            return 0;
        }
    }
}
