using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.Coroutines
{
    /// <summary>
    /// There is a solution on ../coroutines-talk-main based on a talk of MS
    /// </summary>
    public class Starter
    {
        IEnumerable<Task> rotina()
        {
            Console.WriteLine("inicio do fluxo");
            yield return Task.Delay(30000);
            Console.WriteLine("passou-se 3 segundos");
            yield return Task.Delay(2000);
            Console.WriteLine("passou-se 2 segundos");
            yield return Task.CompletedTask;
        }

        [Fact]
        public async void Teste1()
        {
            foreach (var item in rotina())
            {
                await item;
            }
        }
    }
}
