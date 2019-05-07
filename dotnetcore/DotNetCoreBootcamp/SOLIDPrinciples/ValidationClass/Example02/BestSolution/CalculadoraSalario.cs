using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example02.BestSolution
{
    public class CalculadoraSalario
    {
        public double Calcula(Funcionario funcionario)
        {
            if (Cargo.DESENVOLVEDOR.Equals(funcionario.Cargo))
                return dezOuVintePorCento(funcionario);
            if (Cargo.DBA.Equals(funcionario.Cargo) ||
                Cargo.TESTER.Equals(funcionario.Cargo))
                return quinzeOuVinteCincoPorCento(funcionario);

            throw new ArgumentException("funcionario invalido");
        }

        private double quinzeOuVinteCincoPorCento(Funcionario funcionario)
        {
            if (funcionario.Salario > 3000)
                return funcionario.Salario * 0.75;
            else
                return funcionario.Salario * 0.85;
        }

        private double dezOuVintePorCento(Funcionario funcionario)
        {
            if (funcionario.Salario > 3000)
                return funcionario.Salario * 0.8;
            else
                return funcionario.Salario * 0.9;
        }
    }
}
