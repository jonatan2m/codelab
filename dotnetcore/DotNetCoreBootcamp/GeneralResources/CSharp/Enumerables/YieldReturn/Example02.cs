using System;
using System.Linq;

namespace CSharp.Enumerables.YieldReturn
{
    public static class Example02
    {
        public class Order
        {
            public int Id { get; set; }
            public int Value { get; set; }
            public double DiscountRate { get; set; }
        }

        public static class OrderSequence
        {
            private static int _current = 0;

            public static int Next()
            {
                return ++_current;
            }
        }

        public static void Run()
        {
            var orderTemplate = new Order { Value = 10, DiscountRate = 0.1 };
            var orders = Enumerable.Repeat(orderTemplate, 5);
            
            orders = orders.Select(FillOrderId)
                .Select(FillOrderValueWithRandom)
                .Select(FillOrderDiscountRateWithRandom)
                .Select(WarnDiscountRateGreaterThen50Percent);
            
            var total = orders.Sum(CalculateTotal);

            Console.WriteLine($"Total of Orders {total}");

            /*
             * Fill with correct Id.
             * Random Value base default value set on instancition.
             * Random DiscountRate. Its range varies from 0 to 90%.
             * Warn when DiscountRate is greater than 50%.
             * Calculate the total value orders.
             */
        }

        public static Order FillOrderId(Order order)
        {
            order.Id = OrderSequence.Next();
            Console.WriteLine($"Id: {order.Id}");
            return order;
        }

        public static Order FillOrderValueWithRandom(Order order)
        {
            var random = new Random();
            order.Value = random.Next(order.Value, 100);
            Console.WriteLine($"Value: {order.Value}");
            return order;
        }

        public static Order FillOrderDiscountRateWithRandom(Order order)
        {
            var random = new Random();
            order.DiscountRate = random.NextDouble();
            Console.WriteLine($"DiscountRate: {order.DiscountRate}");
            return order;
        }

        public static Order WarnDiscountRateGreaterThen50Percent(Order order)
        {
            if (order.DiscountRate >= 0.5)
            {
                Console.WriteLine($"DiscountRate ({order.DiscountRate * 100}%) greater than 50%");
            }
            Console.WriteLine("DiscountRate allowed");
            return order;
        }

        public static double CalculateTotal(Order order)
        {
            double total = 0;

            var totalWithDiscountApplied = order.Value - (order.Value * order.DiscountRate);

            total += totalWithDiscountApplied;

            Console.WriteLine($"Total Calculated {total}");

            return total;
        }
    }
}
