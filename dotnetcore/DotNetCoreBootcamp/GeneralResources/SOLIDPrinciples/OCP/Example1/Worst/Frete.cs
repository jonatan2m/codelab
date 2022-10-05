namespace SOLIDPrinciples.OCP.Example1.Worst
{
    public class Frete
    {
        public double Para(string cidade)
        {
            if (true /*REGRA 1*/)
            {
                if ("SAO PAULO".Equals(cidade.ToUpper()))
                {
                    return 15;
                }
                return 30;
            }
            //if(REGRA 2) { ... }
            //if(REGRA 3) { ... }
            //if(REGRA 4) { ... }
        }
    }
}
