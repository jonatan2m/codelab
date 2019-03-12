using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Memento.Example2
{
    public class PersonCaretaker
    {
        /*lots of memory consumptive private data that is not necessary to define the
        * state and should thus not be saved. Hence the small memento object. 
        */
        readonly Stack<PersonMemento> _mementos = new Stack<PersonMemento>();

        public PersonMemento GetMemento()
        {
            return _mementos.Pop();
        }

        public void Add(PersonMemento memento)
        {
            _mementos.Push(memento);
        }
    }

    public class PersonMemento
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string CellPhone { get; private set; }
        public string Address { get; private set; }

        public PersonMemento(string fName, string lName, string cell, string address)
        {
            FirstName = fName;
            LastName = lName;
            CellPhone = cell;
            Address = address;
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }

        public PersonMemento CreateUnDo()
        {
            return new PersonMemento(FirstName, LastName, CellPhone, Address);
        }

        public void UnDo(PersonMemento memento)
        {
            FirstName = memento.FirstName;
            LastName = memento.LastName;
            CellPhone = memento.CellPhone;
            Address = memento.Address;
        }

        public void ShowInfo()
        {
            Console.WriteLine("FIRST NAME: {0}", FirstName);
            Console.WriteLine("LAST NAME: {0}", LastName);
            Console.WriteLine("ADDRESS: {0}", Address);
            Console.WriteLine("CELLPHONE: {0}", CellPhone);
        }
    }
}
