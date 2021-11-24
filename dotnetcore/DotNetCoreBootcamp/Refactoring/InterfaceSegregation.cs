using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Refactoring
{
    public class InterfaceSegregation
    {
        [Fact]
        public void Before()
        {
            var before = new Segregation_Before();

            UsingMMakeSomeOne(before);
        }

        [Fact]
        public void After()
        {
            var after = new Segregation_After();

            UsingMMakeSomeOne(after);
        }

        private void UsingMMakeSomeOne(Segregation_Before segregation)
        {
            //the is method only need the MakeSomething_One and the other Segregation_Before fields aren't needed.
            segregation.MakeSomething_One();
        }
        private void UsingMMakeSomeOne(Interface_One one)
        {
            //the is method only need the MakeSomething_One and the other Segregation_Before fields aren't needed.
            one.MakeSomething_One();
        }
    }

    public class Segregation_Before
    {
        public void MakeSomething_One() { Console.WriteLine("One"); }
        public void MakeSomething_Two() { Console.WriteLine("Two"); }
    }

    public class Segregation_After : Interface_One, Interface_Two
    {
        public void MakeSomething_One() { Console.WriteLine("One"); }
        public void MakeSomething_Two() { Console.WriteLine("Two"); }
    }

    public interface Interface_One
    {
        void MakeSomething_One();
    }

    public interface Interface_Two
    {
        void MakeSomething_Two();
    }

    
   
}
