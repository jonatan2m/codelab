using Cryptography.HashingFunction;
using System;
using Xunit;

namespace Cryptography
{
    public class Argon2Test
    {
        [Fact]
        public void Test1()
        {
            Argon2.Play("123456");
        }
    }
}
