using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example02.BestSolution
{
    public static class CargoExtension
    {
        private static readonly Dictionary<Cargo, Lazy<IRegraDeCalculo>> RegrasPadrao =
            new Dictionary<Cargo, Lazy<IRegraDeCalculo>>
            {
                {Cargo.DESENVOLVEDOR, new Lazy<IRegraDeCalculo>(() => new DezOuVintePorCento()) },
                {Cargo.DBA,  new Lazy<IRegraDeCalculo>(() => new QuinzeOuVinteCincoPorCento()) },
                {Cargo.TESTER, new Lazy<IRegraDeCalculo>(() => new QuinzeOuVinteCincoPorCento()) },
            };

        public static double AplicarRegraPadrao(this Cargo cargo, Funcionario funcionario)
        {
            return RegrasPadrao[cargo].Value.Calcula(funcionario);
        }
    }
}
