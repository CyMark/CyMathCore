using System;
using Xunit;
using CyMathCore;


namespace XUnitTestCyMath
{
    public class XTestPrimeCalc
    {

        [Fact]
        public void PrimeCalc_Basics()
        {
            PrimeCalc calc = new();

            Assert.Equal(71, calc.Primes[^1]);
            Assert.True(calc.IsPrime(9413));
            Assert.True(calc.IsPrime(83));
            Assert.True(calc.IsPrime(89));
            Assert.True(calc.IsPrime(97));
            //Assert.Equal(97, calc.Primes[^1]);
            //Assert.Equal("[2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97]", calc.PrimeListToView(25));
            //Assert.True(calc.IsPrime(1000000403));

        }


        [Theory]
        [InlineData(1, "[2]")]
        [InlineData(2, "[2,3]")]
        [InlineData(3, "[2,3,5]")]
        [InlineData(10, "[2,3,5,7,11,13,17,19,23,29]")]
        [InlineData(20, "[2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71]")]
        [InlineData(25, "[2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97]")]
        public void PrimeCalc_ListView(int firstNrOfPrimes, string result)
        {
            PrimeCalc calc = new();
            _ = calc.IsPrime(97*97*2); // force creation of list above 97 * 97

            Assert.Equal(result, calc.PrimeListToView(firstNrOfPrimes));

        }

        // result 2 | 3 | 5 | 7 | 11 | 13 | 17 | 19 | 23 | 29 | 31 | 37 | 41 | 43 | 47 | 53 | 59 | 61 | 67 | 71 | ... (46 primes)
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        [InlineData(7, true)]
        [InlineData(9, false)]
        [InlineData(83, true)]
        [InlineData(191, true)]
        [InlineData(5051, true)] // > 71 * 71
        [InlineData(5059, true)]
        [InlineData(5077, true)]
        [InlineData(5081, true)]
        [InlineData(5087, true)]
        [InlineData(5099, true)]
        [InlineData(10000397, true)]
        [InlineData(100000393, true)]
        [InlineData(1000000403, true)]
        [InlineData(1000000407, false)]
        [InlineData(600851475143, false)]
        public void PrimeCalc_IsPrime(long value, bool result)
        {
            PrimeCalc calc = new();
            Assert.Equal(result, calc.IsPrime(value));
        }

    }
}
