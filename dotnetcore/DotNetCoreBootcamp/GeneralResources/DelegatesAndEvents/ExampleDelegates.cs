using System;
namespace DelegatesAndEvents
{
    public class ExampleDelegates
    {
        public delegate int Comparison<in T>(T left, T right);

        public Comparison<int> comparator;

        public void CallDelegate(int a, int b)
        {
            comparator.Invoke(a, b);
        }

        /// <summary>
        /// Delegate como parametro.
        /// </summary>
        /// <returns>The como parametro.</returns>
        /// <param name="delegate">Delegate.</param>
        private int DelegateComoParametro(Comparison<int> @delegate)
        {
            return @delegate.Invoke(2, 3);
        }


        public static void Run()
        {
            ExampleDelegates exampleDelegates = new ExampleDelegates();


            exampleDelegates.comparator += (left, right) =>
            {
                var result = left + right;
                Console.WriteLine(result);
                return result;
            };

            exampleDelegates.comparator += (left, right) =>
            {
                var result = left * right;
                Console.WriteLine(result);
                return result;
            };
            exampleDelegates.CallDelegate(1, 2);
            //Assert.Equal(460, DelegateComoParametro((a,b) => a + b));
            //Assert.Equal(3, exampleDelegates.comparator(1, 2));

            int[] inteiros = new int[] { 1, 2, 3, 4, 5 };

            var number = Array.Find(inteiros, (int n) =>
            {
                return n == 3;
            });
        }
    }
}
