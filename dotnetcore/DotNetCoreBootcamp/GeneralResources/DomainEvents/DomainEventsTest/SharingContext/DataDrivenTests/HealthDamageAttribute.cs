using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace DomainEventsTest.SharingContext.DataDrivenTests
{
    public class HealthDamageAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 0, 100 };
            yield return new object[] { 1, 99 };
            yield return new object[] { 25, 75 };
            yield return new object[] { 50, 50 };
            yield return new object[] { -1, 101 };
        }
    }
}
