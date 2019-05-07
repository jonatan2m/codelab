using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example02.BestSolution
{
    public abstract class RegraDeCalculo
    {
        public abstract double Calcula(Funcionario funcionario);
    }

    public class DezOuVintePorCento : RegraDeCalculo
    {
        public override double Calcula(Funcionario funcionario)
        {
            if (funcionario.Salario > 3000)
                return funcionario.Salario * 0.8;
            else
                return funcionario.Salario * 0.9;
        }
    }

    public class QuinzeOuVinteCincoPorCento : RegraDeCalculo
    {   
        public override double Calcula(Funcionario funcionario)
        {
            if (funcionario.Salario > 3000)
                return funcionario.Salario * 0.75;
            else
                return funcionario.Salario * 0.85;
        }
    }
}
