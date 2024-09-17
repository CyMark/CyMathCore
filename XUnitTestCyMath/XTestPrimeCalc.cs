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

            //Assert.Equal(71, calc.Primes[^1]);

            Assert.True(calc.IsPrime(83));
            Assert.Equal(7, calc.Primes[^1]);
            Assert.True(calc.IsPrime(89));
            Assert.True(calc.IsPrime(97));
            Assert.True(calc.IsPrime(9413));
            Assert.Equal("[2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97]", calc.PrimeListToView(25));
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
            _ = calc.IsPrime(9049); // force creation of list above 97 * 97

            Assert.Equal(result, calc.PrimeListToView(firstNrOfPrimes));

        }

        [Theory]
        [InlineData(1, "[97]")]
        [InlineData(2, "[89,97]")]
        [InlineData(3, "[83,89,97]")]
        [InlineData(10, "[53,59,61,67,71,73,79,83,89,97]")]
        [InlineData(20, "[13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97]")]
        [InlineData(25, "[2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97]")]
        public void PrimeCalc_ListViewDesc(int firstNrOfPrimes, string result)
        {
            PrimeCalc calc = new();
            _ = calc.IsPrime(9049); // force creation of list above 97 * 97

            Assert.Equal(result, calc.PrimeListToViewDesc(firstNrOfPrimes));

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
        [InlineData(10086647, false)]
        [InlineData(841, false)]
        [InlineData(10000379, true)]
        [InlineData(10000397, false)]
        [InlineData(100000393, true)]
        [InlineData(1000000403, true)]
        [InlineData(1000000407, false)]
        [InlineData(600851475143, false)]
        [InlineData(600851475583, true)]
        [InlineData(18848997161, true)]
        [InlineData(18848997157, false)]
        [InlineData(188489971511, false)]
        public void PrimeCalc_IsPrime(long value, bool result)
        {
            PrimeCalc calc = new();
            Assert.Equal(result, calc.IsPrime(value));
        }



        [Fact]
        public void PrimCalc_Factors()
        {
            long testNr = 2 * 2 * 3 * 5 * 5; // 300
            PrimeCalc calc = new();

            var list = calc.GetPrimeFactors(testNr);

            Assert.Equal(3, list.Count);
            Assert.Equal(2, list[0].Prime);
            Assert.Equal(2, list[0].Count);

            Assert.Equal(3, list[1].Prime);
            Assert.Equal(1, list[1].Count);

            Assert.Equal(5, list[2].Prime);
            Assert.Equal(2, list[2].Count);

            //// big nr:
            testNr = 600851475143;
            list = calc.GetPrimeFactors(testNr);
            Assert.Equal(4, list.Count);

            Assert.Equal(71, list[0].Prime);
            Assert.Equal(1, list[0].Count);

            Assert.Equal(839, list[1].Prime);
            Assert.Equal(1, list[1].Count);

            Assert.Equal(1471, list[2].Prime);
            Assert.Equal(1, list[2].Count);

            Assert.Equal(6857, list[3].Prime);
            Assert.Equal(1, list[3].Count);

            // big nr:
            testNr = 188489971511;
             list = calc.GetPrimeFactors(testNr);
            Assert.Equal(2, list.Count);

            Assert.Equal(17, list[0].Prime);
            Assert.Equal(1, list[0].Count);

            Assert.Equal(11087645383, list[1].Prime);
            Assert.Equal(1, list[1].Count);


        }



        [Fact]
        public void PrimCalc_FactorsMulti()
        {
            long testNr = 600851475;
            PrimeCalc calc = new();

            var list = calc.GetPrimeFactors(testNr);

            Assert.Equal(4, list.Count);

            Assert.Equal(3, list[0].Prime);
            Assert.Equal(2, list[0].Count);

            Assert.Equal(5, list[1].Prime);
            Assert.Equal(2, list[1].Count);

            Assert.Equal(7, list[2].Prime);
            Assert.Equal(2, list[2].Count);

            Assert.Equal(54499, list[3].Prime);
            Assert.Equal(1, list[3].Count);


            testNr = 18848997157;
            var list2 = calc.GetPrimeFactors(testNr);

            Assert.Equal(2, list2.Count);

            Assert.Equal(13729, list2[0].Prime);
            Assert.Equal(1, list2[0].Count);

            Assert.Equal(1372933, list2[1].Prime);
            Assert.Equal(1, list2[1].Count);

        }

    }
}
