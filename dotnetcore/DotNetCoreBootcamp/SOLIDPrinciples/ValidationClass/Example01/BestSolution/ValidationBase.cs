using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example01.BestSolution
{
    public abstract class ValidationBase<T> : IValidation where T : class
    {
        public T Context { get; private set; }

        public ValidationBase(T context)
        {
            Context = context ?? throw new ArgumentNullException("context");
        }

        public void Validate()
        {
            if (!IsValid)
            {
                throw new ValidationException(Message);
            }
        }

        public virtual bool Condition
        {
            get
            {
                // If the condition is not overriden, the requirent always needs to be true
                return true;
            }
        }

        public abstract bool Requirement { get; }

        public bool IsValid { get { return Implication(Condition, Requirement); } }

        public abstract string Message { get; }

        private static bool Implication(bool condition, bool requirement)
        {
            return !condition || requirement;
        }
    }
}
