using System;
using EP.SOLID.OCP.Solucao;
using Xunit;

namespace DesignPatternsTest
{
    public class SolidOCPTest
    {
       [Fact]
       public void SOLID_OCP_Example()
        {
            DebitoConta debitoContaSolucao = new DebitoContaCorrente();

            debitoContaSolucao.Debitar(23, "xxxx");
        }
    }
}
