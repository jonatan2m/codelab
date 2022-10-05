namespace SOLIDPrinciples.OCP.Example1.Good
{
    public class Frete3 : IServicoDeEntrega
    {
        public double Para(string cidade)
        {
            if ("MINAS GERAIS".Equals(cidade.ToUpper()))
            {
                return 20;
            }
            return 30;
        }
    }
}
