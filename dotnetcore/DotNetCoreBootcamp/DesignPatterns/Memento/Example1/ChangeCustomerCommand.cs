using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Memento.Example1
{
    public class ChangeCustomerCommand : IDbCommand
    {
        public Customer Customer { get; private set; }
        
        //NOTE: This example will potentially use a LOT of memory!
        private readonly List<MementoForCustomerEntity> _mementos =
            new List<MementoForCustomerEntity>();

        public ChangeCustomerCommand(Customer customer)
        {
            Customer = customer;
        }

        public void Execute(string newName)
        {
            _mementos.Add(new MementoForCustomerEntity(Customer));
            Customer.Name = newName;
        }

        public void Undo()
        {
            Customer = _mementos[_mementos.Count - 1].GetCustomer();
            _mementos.RemoveAt(_mementos.Count - 1);
        }
    }
}
