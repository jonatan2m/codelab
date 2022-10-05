using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example01.BestSolution
{
    public class OnlyAdultsCanConsumeAlcoholValidation : ValidationBase<Person>
    {
        private const int MinimumAge = 18;

        public OnlyAdultsCanConsumeAlcoholValidation(Person context) : base(context)
        {
        }

        //public override bool IsValid => !Context.ConsumesAlcohol || Context.Age >= MinimumAge;

        public override string Message
        {
            get
            {
                return string.Format(
                    @"{0} is not allowed to consume alcohol because
                      his or her age ({1}) not is {2} or higher.",
                    Context.Name, Context.Age, MinimumAge);
            }
        }

        public override bool Condition => Context.ConsumesAlcohol;

        public override bool Requirement => Context.Age >= MinimumAge;
    }
}
