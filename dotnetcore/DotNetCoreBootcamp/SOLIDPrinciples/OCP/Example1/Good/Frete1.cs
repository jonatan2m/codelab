namespace SOLIDPrinciples.OCP.Example1.Good
{
    public class Frete1 : IServicoDeEntrega
    {
        public double Para(string cidade)
        {
            if ("SAO PAULO".Equals(cidade.ToUpper()))
            {
                return 15;
            }
            return 30;
        }
    }
}
