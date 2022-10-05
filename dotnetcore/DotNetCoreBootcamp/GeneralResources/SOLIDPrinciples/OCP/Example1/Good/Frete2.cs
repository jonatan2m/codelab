namespace SOLIDPrinciples.OCP.Example1.Good
{
    public class Frete2 : IServicoDeEntrega
    {
        public double Para(string cidade)
        {
            if ("RIO DE JANEIRO".Equals(cidade.ToUpper()))
            {
                return 10;
            }
            return 15;
        }
    }
}
