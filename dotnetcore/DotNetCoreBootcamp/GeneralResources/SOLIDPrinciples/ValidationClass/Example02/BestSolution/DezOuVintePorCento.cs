namespace SOLIDPrinciples.ValidationClass.Example02.BestSolution
{
    public class DezOuVintePorCento : IRegraDeCalculo
    {
        public double Calcula(Funcionario funcionario)
        {
            if (funcionario.Salario > 3000)
                return funcionario.Salario * 0.8;
            else
                return funcionario.Salario * 0.9;
        }
    }
}
