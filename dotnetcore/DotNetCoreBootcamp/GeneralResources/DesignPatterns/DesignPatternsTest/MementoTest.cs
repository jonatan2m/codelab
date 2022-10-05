using DesignPatterns.Memento.Example1;
using DesignPatterns.Memento.Example2;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DesignPatternsTest
{
    public class MementoTest
    {
        [Fact]
        public void Memento_Example1()
        {
            var customer = new Customer { Id = 5, Name = "Jonatan Machado" };
            var cmd = new ChangeCustomerCommand(customer);
            var newName = "Martins";
            cmd.Execute(newName);
            Assert.Equal(newName, cmd.Customer.Name);
        }

        [Fact]
        public void Memento_Example1_0()
        {
            var originalName = "Jonatan Machado";
            var customer = new Customer { Id = 5, Name = originalName };
            var cmd = new ChangeCustomerCommand(customer);
            var newName = "Martins";
            cmd.Execute(newName);
            cmd.Undo();
            Assert.Equal(originalName, cmd.Customer.Name);
        }

        [Fact]
        public void Memento_Example2()
        {
            //Exemplo sem usar o Command fica bem esquisito
            //Os métodos ficam largados, não usar esse exemplo
            var person = new Person
            {
                Address = "Under the Bridge 171",
                CellPhone = "122011233185",
                FirstName = "John",
                LastName = "Doe"
            };

            var caretaker = new PersonCaretaker();
            caretaker.Add(person.CreateUnDo());

            person.ShowInfo();
            Console.WriteLine();

            person.Address = "Under the Tree 119";
            caretaker.Add(person.CreateUnDo());

            person.ShowInfo();
            Console.WriteLine();

            person.CellPhone = "987654321";
            person.ShowInfo();
            Console.WriteLine();

            person.UnDo(caretaker.GetMemento());
            person.ShowInfo();
            Console.WriteLine();

            person.UnDo(caretaker.GetMemento());
            person.ShowInfo();
            Console.WriteLine();
        }
    }
}
