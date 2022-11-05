namespace GeneralResources.XUnitExample.RaiseEventOrThrowException
{
    public class ThrowTests
    {
        private readonly Buggy exceptionThrower;

        public ThrowTests()
        {
            exceptionThrower = new Buggy();
        }

        [Fact]
        public void ThrowsExceptionAssertions()
        {            
            Assert.Throws<InvalidCastException>(exceptionThrower.ThrowsInvalidCastException);
            Assert.ThrowsAny<CustomInvalidOperationException>(exceptionThrower.ThrowsCustomInvalidOperationException);
        }

        [Fact]
        public void ThrowsExceptionAssertionsAsync()
        {
            Func<Task> ThrowExceptionFunc = () => exceptionThrower.ThrowsExceptionAsync();
            Assert.ThrowsAsync<InvalidCastException>(ThrowExceptionFunc);
            Assert.ThrowsAnyAsync<InvalidCastException>(ThrowExceptionFunc);
        }

        [Fact]
        public void ThrowsExceptionAssertionsRecord()
        {
            Exception ex = Record.Exception(exceptionThrower.ThrowsInvalidCastException);

            Assert.NotNull(ex);
        }
    }
}
