namespace RecursiveLab
{
    public class Factorial
    {
        public int Calculate(int number)
        {
            if(number <= 1)
                return 1;
            return number * Calculate(number - 1);
        }
    }
}