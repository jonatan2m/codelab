namespace TalkExamples.SOLID.OCP.Example1
{
    public abstract class Cargo
    {
        public virtual decimal PercentualBonificacao()
        {
            return 0;
        }
    }

    public class Diretor: Cargo
    {
        public override decimal PercentualBonificacao()
        {
            return 0.1M;
        }
    }

    public class Desenvolvedor : Cargo
    {
        public override decimal PercentualBonificacao()
        {
            return 0.2M;
        }
    }

    public class Testador : Cargo
    {
        public override decimal PercentualBonificacao()
        {
            return 0.2M;
        }
    }

    public class Marketing : Cargo
    {

    }
}
