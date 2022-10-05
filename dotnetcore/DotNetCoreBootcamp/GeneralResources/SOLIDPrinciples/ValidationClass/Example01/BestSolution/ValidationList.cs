using System;
using System.Collections.Generic;
using System.Linq;

namespace SOLIDPrinciples.ValidationClass.Example01.BestSolution
{
    public interface IValidationList
    {
        bool IsValid { get; }
        void Validate();
        IEnumerable<string> Messages { get; }
    }

    public class ValidationList : List<IValidation>,  IValidationList
    {
        public bool IsValid => this.All(v => v.IsValid);

        public IEnumerable<string> Messages => this
            .Where(x => !x.IsValid)
            .Select(x => x.Message);

        public void Validate()
        {
            foreach (var validation in this)
            {
                validation.Validate();
            }
        }
    }
}
