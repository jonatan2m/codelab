using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example02.BestSolution
{
    public enum Cargo
    {
        [Cargo(typeof(DezOuVintePorCento))]
        DESENVOLVEDOR,
        [Cargo(typeof(QuinzeOuVinteCincoPorCento))]
        DBA,
        [Cargo(typeof(QuinzeOuVinteCincoPorCento))]
        TESTER
    }

    public class CargoAttribute : Attribute
    {
        public readonly RegraDeCalculo regraDeCalculo;

        public CargoAttribute(Type regraDeCalculo)
        {
            this.regraDeCalculo = Activator.CreateInstance(regraDeCalculo) as RegraDeCalculo;
        }
    }

    public static class Cargos
    {
        public static double AplicarRegraDeCalculo(this Cargo cargo, Funcionario funcionario)
        {
            CargoAttribute attr = GetAttr(cargo);
            return attr.regraDeCalculo.Calcula(funcionario);
        }

        private static CargoAttribute GetAttr(Cargo cargo)
        {
            return (CargoAttribute)Attribute.GetCustomAttribute(
                ForValue(cargo), typeof(CargoAttribute));
        }

        private static MemberInfo ForValue(Cargo cargo)
        {
            return typeof(Cargo).GetField(Enum.GetName(typeof(Cargo), cargo));
        }
    }
}
