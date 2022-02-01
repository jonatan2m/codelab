using System;
using System.Text;
using Xunit;

namespace DomainEventsTest.SharingContext.DataDrivenTests
{
    public class TakeDamageTests
    {
        [Theory]
        [InlineData(0, 100)]
        [InlineData(1, 99)]
        [InlineData(25, 75)]
        [InlineData(50, 50)]
        [InlineData(-1, 101)]
        public void TakeDamage(int damage, int expectedHealth)
        {
            var sut = new Player("A");

            sut.TakeDamage(damage);

            Assert.Equal(expectedHealth, sut.Health);

        }

        [Theory]
        [MemberData(nameof(InternalHealthDamageTestData.TestData), MemberType = typeof(InternalHealthDamageTestData))]
        public void TakeDamageTestDataClass(int damage, int expectedHealth)
        {
            var sut = new Player("A");

            sut.TakeDamage(damage);

            Assert.Equal(expectedHealth, sut.Health);
        }

        [Theory]
        [MemberData(nameof(ExternalHealthDamageTestData.TestData), MemberType = typeof(ExternalHealthDamageTestData))]
        public void TakeDamageTestClassExternal(int damage, int expectedHealth)
        {
            var sut = new Player("A");

            sut.TakeDamage(damage);

            Assert.Equal(expectedHealth, sut.Health);
        }
    }
}
