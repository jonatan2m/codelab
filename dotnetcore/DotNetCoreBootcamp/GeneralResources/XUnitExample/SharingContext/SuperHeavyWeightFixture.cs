namespace GeneralResources.XUnitExample.SharingContext
{
    /// <summary>
    /// The first thing we need to do it to create a class fixture for our SUT and use that in our test class instead.
    /// </summary>
    public class SuperHeavyWeightFixture : IDisposable
    {
        public SuperHeavyWeight Sut { get; private set; }

        public SuperHeavyWeightFixture()
        {
            Sut = new SuperHeavyWeight();
        }

        public void Dispose()
        {
            Sut.Dispose();
        }
    }
    
    /// <summary>
    /// We will use the Class fixture in the method and we spend only one time the creation of the class
    /// </summary>
    public class SuperHeavyWeightFixtureTest : IClassFixture<SuperHeavyWeightFixture>
    {
        private readonly SuperHeavyWeightFixture _fixture;

        public SuperHeavyWeightFixtureTest(SuperHeavyWeightFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CalculationOne_WhenCalled_ReturnsTheCorrectResult()
        {
            var result = _fixture.Sut.CalculationOne(2);

            Assert.Equal(Math.PI * 2, result);
        }

        [Fact]
        public void CalculationTwo_WhenCalled_ReturnsTheCorrectResult()
        {
            var result = _fixture.Sut.CalculationOne(5);

            Assert.Equal(Math.PI * 5, result);
        }

        [Fact]
        public void CalculationThree_WhenCalled_ReturnsTheCorrectResult()
        {
            var result = _fixture.Sut.CalculationOne(7);

            Assert.Equal(Math.PI * 7, result);
        }
    }
}
