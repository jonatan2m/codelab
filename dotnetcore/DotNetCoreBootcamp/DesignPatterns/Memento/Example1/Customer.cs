using System;

namespace DesignPatterns.Memento.Example1
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //This is just one possible implementation.  
        public Customer Clone()
        {
            return MemberwiseClone() as Customer;
        }
    }
}