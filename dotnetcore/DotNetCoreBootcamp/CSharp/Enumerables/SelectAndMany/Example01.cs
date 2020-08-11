using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace CSharp.Enumerables.SelectAndMany
{
    public class Example01
    {
        [Params(10, 100, 1000)]
        public int N;

        static List<Order> Orders = new List<Order>();

        private class Order
        {
            public int Id { get; set; }
            public int Price { get; set; }
        }
        /*
         * Process1 Order verifying Price and Id properties
         * Process2 Order applying discount
         * Process3 Order update inventory
         */

        public static void Run()
        {
            var summary = BenchmarkRunner.Run<Example01>();
        }

        [GlobalSetup]
        public void CreateOrders()
        {
            Orders.Clear();
            for (int i = 0; i < N; i++)
            {
                Orders.Add(new Order { Id = i, Price = i + 10 });
            }
        }

        [Benchmark]
        public void RunWithForeach()
        {
            foreach (var order in Orders)
            {
                Process1(order);
                Process2(order);
                Process3(order);

                //Console.WriteLine($"Order being processed {order.Id}: {order.Price}");
            }
        }

        [Benchmark]
        public void RunWithSelect()
        {
            var orderToBeProcessed = Orders
                .Select(Process1)
                .Select(Process2)
                .Select(Process3);

            foreach (var order in orderToBeProcessed)
            {
                //Console.WriteLine($"Order being processed {order.Id}: {order.Price}");
            }
        }

        private static Order Process1(Order order)
        {
            //Console.WriteLine($"Process1 order: {order.Id}");
            return order;
        }

        private static Order Process2(Order order)
        {
            //Console.WriteLine($"Process2 order: {order.Id}");
            return order;
        }

        private static Order Process3(Order order)
        {
            //Console.WriteLine($"Process3 order: {order.Id}");
            return order;
        }

    }
}
