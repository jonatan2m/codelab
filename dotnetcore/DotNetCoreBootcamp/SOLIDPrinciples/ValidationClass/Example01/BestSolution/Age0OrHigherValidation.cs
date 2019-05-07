using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example01.BestSolution
{
    public class Age0OrHigherValidation : ValidationBase<Person>
    {
        public Age0OrHigherValidation(Person context)
            : base(context)
        {
        }

        //public override bool IsValid => Context.Age >= 0;        

        public override string Message
        {
            get
            {
                return string.Format("The Age {1} of {0} is not 0 or higher.",
                    Context.Name, Context.Age);
            }
        }

        public override bool Requirement => Context.Age >= 0;
    }
}
