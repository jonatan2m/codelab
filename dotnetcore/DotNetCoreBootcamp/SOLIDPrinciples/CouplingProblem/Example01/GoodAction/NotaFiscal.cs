namespace SOLIDPrinciples.CouplingProblem.Example01.GoodAction
{
    public class NotaFiscal
    {
        private double valor;
        private double imposto;

        public NotaFiscal(double valor, double imposto)
        {
            this.valor = valor;
            this.imposto = imposto;
        }
    }
}