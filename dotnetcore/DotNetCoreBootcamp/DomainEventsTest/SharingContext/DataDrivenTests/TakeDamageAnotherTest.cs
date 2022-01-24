using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DomainEventsTest.SharingContext.DataDrivenTests
{
    public class TakeDamageAnotherTest
    {
        [Theory]
        [MemberData("TestData", MemberType = typeof(InternalHealthDamageTestData))]
        public void TakeDamage(int damage, int expectedHealth)
        {
            var sut = new Player("B");

            sut.TakeDamage(damage);

            Assert.Equal(expectedHealth, sut.Health);
        }
    }
}
