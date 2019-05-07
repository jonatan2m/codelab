using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example01.WorstSolution
{

    public class AlcoholSeller
    {
        // Other codes not shown

        /// <summary>
        /// This is not a good (SOLID) solution:
        /// 1. The Validate method has multiple responsibilities(Single responsibility).
        /// 2. The Validate method has to change when a new validation will be added(Open/close principle, Dependency inversion).
        /// 3. It shows only the first message that is invalid.
        /// The conditions are inverted(negative condition is harder to read).
        /// This model can not stand when the Person has 20 validations(otherwise the method would be much to long).
        /// </summary>
        /// <param name="person"></param>
        private void Validate(Person person)
        {
            // Previous code forces person can not be null

            const int MinimumAge = 18;

            if (person.Age < MinimumAge && person.ConsumesAlcohol)
            {
                throw new ValidationException(
                    "{0} is not allowed to consume alcohol because his or her age ({1}) is not {2} or higher.",
                    person.Name, person.Age, MinimumAge);
            }
            if (person.Age < 0)
            {
                throw new ValidationException("Age of {0} must be higher than 0",
                    person.Name);
            }
        }
    }
}
