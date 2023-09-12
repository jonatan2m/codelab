using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.CSharp.Enumerables.IEnumerableAndList
{
    public class Piping
    {
        [Fact]
        public void Test1()
        {
            // Criar uma sequência de números
            var numeros = Enumerable.Range(1, 1000);

            // Filtrar apenas os números pares
            var numerosPares = numeros.Where(n =>
            {
                return n % 2 == 0;
            });

            // Filtrar apenas os números maiores que 100000
            var numerosGrandes = numerosPares.Where(n => n > 100);

            // Somar todos os números filtrados
            var soma = numerosGrandes.Sum();

            Console.WriteLine(soma); // Imprime 250000500000

        }
    }
}
