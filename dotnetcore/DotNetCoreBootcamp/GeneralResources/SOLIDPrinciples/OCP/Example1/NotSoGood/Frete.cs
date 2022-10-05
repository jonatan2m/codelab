namespace SOLIDPrinciples.OCP.Example1.NotSoGood
{
    public class Frete
    {
        public double Para(string cidade)
        {
            if("SAO PAULO".Equals(cidade.ToUpper()))
            {
                return 15;
            }
            return 30;
        }
    }
}
