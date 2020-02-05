//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace TalkExamples.SOLID.OCP.Example1
//{
//    public enum Cargos
//    {
//        Diretor,
//        Desenvolvedor
//    }

//    public class Funcionario
//    {
//        public Cargos Cargo { get; private set; }
//        public decimal Salario { get; private set; }

//        public Funcionario(Cargos cargo, decimal salario)
//        {
//            Cargo = cargo;
//            Salario = salario;
//        }
//    }

//    public class CalculoDeBonificacao
//    {
//        public decimal CalcularBonusDo(Funcionario funcionario)
//        {
//            if (funcionario.Cargo == Cargos.Diretor)
//                return funcionario.Salario * 1.1M;
//            else if (funcionario.Cargo == Cargos.Desenvolvedor)
//                return funcionario.Salario * 1.2M;
//            return 0;
//        }

//    }
//}
