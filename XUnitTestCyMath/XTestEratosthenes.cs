using System;
using Xunit;
using CyMathCore;

namespace XUnitTestCyMath
{
    public class XTestEratosthenes
    {
        [Theory]
        [InlineData(5, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(31, true)]
        [InlineData(127, true)]
        public void Eratosthenes_PrimeTest(long prime, bool result)
        {
            EratosthenesVL e = new ();
            IntVL CheckPrime = new (prime);
            Assert.Equal(result, e.IsPrime(CheckPrime));
        }
    }
}
