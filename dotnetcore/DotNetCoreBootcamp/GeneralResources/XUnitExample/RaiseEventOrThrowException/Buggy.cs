namespace GeneralResources.XUnitExample.RaiseEventOrThrowException
{
    public class Buggy
    {
        public void ThrowsInvalidCastException()
        {
            throw new InvalidCastException();
        }

        public void ThrowsCustomInvalidOperationException()
        {
            throw new CustomInvalidOperationException();
        }

        public Task ThrowsExceptionAsync()
        {
            throw new InvalidCastException();
        }
    }

    public class CustomInvalidOperationException : InvalidOperationException
    {

    }
}
