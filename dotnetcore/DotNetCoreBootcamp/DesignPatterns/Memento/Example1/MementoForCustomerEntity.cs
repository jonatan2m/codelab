using System;

namespace DesignPatterns.Memento.Example1
{
    internal class MementoForCustomerEntity
    {
        private Customer _customer;

        public MementoForCustomerEntity(Customer customer)
        {
            this._customer = customer.Clone();
        }

        internal Customer GetCustomer()
        {
            return _customer;
        }
    }
}