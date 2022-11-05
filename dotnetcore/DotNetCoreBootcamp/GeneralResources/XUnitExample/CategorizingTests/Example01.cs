using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample.CategorizingTests
{
    public class SampleClass
    {
        public int Add(int left, int right) => left + right;
    }

    public class Example01
    {
        private SampleClass _sut;

        public Example01()
        {
            _sut = new SampleClass();
        }

        [Fact]
        public void SampleClass_Add_Always_ReturnsTheCorrectResult()
        {
            Assert.True(_sut.Add(2, 2) == 4);
        }

        [Fact]
        [Trait("UI", "Front")]
        public void Add_Always_ReturnsTheCorrectResult()
        {
            Assert.True(_sut.Add(2, 2) == 4);
        }

        [Fact]
        [Trait("UI", "Front")]
        public void Add_WithZeroArguments_ReturnsZero()
        {
            Assert.True(_sut.Add(0, 0) == 0);
        }

        [Fact]
        [Trait("UI", "Back")]
        public void Add_Always_ReturnsTheCorrectResult2()
        {
            Assert.True(_sut.Add(2, 2) == 4);
        }

        [Fact]
        [Trait("UI", "Back")]
        public void Add_WithZeroArguments_ReturnsZero2()
        {
            Assert.True(_sut.Add(0, 0) == 0);
        }
    }
}
