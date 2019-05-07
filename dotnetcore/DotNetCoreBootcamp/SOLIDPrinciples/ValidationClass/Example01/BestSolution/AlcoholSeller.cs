using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example01.BestSolution
{
    public class AlcoholSeller
    {
        private readonly IValidationList validationList;

        public AlcoholSeller(IValidationList validationList)
        {
            this.validationList = validationList;
        }

        // Other code not shown

        public bool IsValid()
        {
            return validationList.IsValid;
        }

        public IEnumerable<string> Messages
        {
            get
            {
                return validationList.Messages;
            }
        }
    }
}
