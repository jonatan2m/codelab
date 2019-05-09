namespace SOLIDPrinciples.ValidationClass.Example02.BestSolution
{
    public class QuinzeOuVinteCincoPorCento : IRegraDeCalculo
    {   
        public double Calcula(Funcionario funcionario)
        {
            if (funcionario.Salario > 3000)
                return funcionario.Salario * 0.75;
            else
                return funcionario.Salario * 0.85;
        }
    }
}
