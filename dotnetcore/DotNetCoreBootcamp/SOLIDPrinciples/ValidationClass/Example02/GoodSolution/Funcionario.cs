using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example02.GoodSolution
{
    public class Funcionario
    {
        public Cargo Cargo { get; set; }
        public double Salario { get; set; }

        public double AplicarRegraDeCalculoParaCargo()
        {
            return Cargo.AplicarRegraDeCalculo(this);
        }
    }
}
