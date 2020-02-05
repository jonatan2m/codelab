using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TalkExamplesTest.SOLID.LSP.Example1
{
    public class CollectionArrayTest
    {
        [Fact]
        public void Test()
        {
            var array = new[] { 1, 2, 3 };

            Assert.True(array is Array);

            ICollection<int> collection = array;

            Assert.True(array is ICollection<int>);

            collection.Add(4);
        }
    }
}
