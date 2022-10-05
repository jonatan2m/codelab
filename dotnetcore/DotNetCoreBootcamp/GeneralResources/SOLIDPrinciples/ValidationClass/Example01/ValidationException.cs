using System;

namespace SOLIDPrinciples.ValidationClass.Example01
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, params object[] args)
            : base(String.Format(message, args))
        {
        }
    }
}
