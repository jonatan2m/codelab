namespace GeneralResources.CSharp.Threads.ThreadStaticExamples
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/api/system.threadstaticattribute?view=net-6.0
    /// </summary>
    public class ThreadStaticAttributeExamples
    {
        //[Theory(Skip = "Para evitar deixar o teste quebrar a toa")]
        [Theory]
        [InlineData(3)]
        public async Task Example01(int timesToIncrement)
        {

            int nWorkers; // number of processing threads
            int nIOs; // number of I/O threads
            ThreadPool.GetMaxThreads(out nWorkers, out nIOs);

            var c1 = new ClassA();
            var c2 = new ClassA();

            for (int i = 0; i < timesToIncrement; i++)
            {
                await Task.Run(() => c1.Increment());
            }

            Assert.Equal(timesToIncrement, c1.GetSharedCounter());
            Assert.Equal(timesToIncrement, c1.GetIsolatedCounter());
            
            Assert.Equal(timesToIncrement, c2.GetSharedCounter());
            Assert.Equal(0, c2.GetIsolatedCounter());

            
        }
    }

    [System.Diagnostics.DebuggerDisplay("Contador compartilhado = {SharedCounter}")]
    class ClassA
    {
        public static int SharedCounter;

        /// <summary>
        /// Cada thread terá seu valor isolado e esse teste deixa isso claro. 
        /// Como a abertura de Task faz a Thread mudar, a variavel se torna indisponivel.
        /// </summary>
        [ThreadStatic]
        public static int IsolatedCounter;

        public void Increment()
        {
            SharedCounter++;
            IsolatedCounter++;
        }

        public int GetSharedCounter() => SharedCounter;
        public int GetIsolatedCounter() => IsolatedCounter;
    }
}
