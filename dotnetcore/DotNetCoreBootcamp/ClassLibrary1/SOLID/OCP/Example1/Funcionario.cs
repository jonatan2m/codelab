using System;
using System.Collections.Generic;
using System.Text;

namespace TalkExamples.SOLID.OCP.Example1
{
    public class Funcionario
    {
        public Cargo Cargo { get; private set; }
        public decimal Salario { get; private set; }
               
        public Funcionario(Cargo cargo, decimal salario)
        {
            Cargo = cargo;
            Salario = salario;
        } 
        
        public decimal CalcularBonificacao()
        {
            return Salario * Cargo.PercentualBonificacao();
        }
    }
}
