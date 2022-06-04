using System;
using Xunit;
using CyMathCore;


namespace XUnitTestCyMath
{
    public class XTestStatistics
    {

        [Fact]
        public void Statistics_NormalDistribution()
        {
            double actual = Statistics.NormalDistribution(10, 10, 3);
            //Assert.Equal(result, actual);
            Assert.InRange(actual, 0.132980, 0.132981);

            actual = Statistics.NormalDistribution(7, 10, 3);
            Assert.InRange(actual, 0.0806566, 0.0806570);

            actual = Statistics.NormalDistribution(13, 10, 3);
            Assert.InRange(actual, 0.0806566, 0.0806570);

            // CUMULITIVE:

            actual = Statistics.NormalDistributionCumulative(10, 10, 3);
            Assert.InRange(actual, 0.495, 0.504);

            actual = Statistics.NormalDistributionCumulative(7, 10, 3);  // 1 SDEV
            Assert.InRange(actual, 0.157, 0.158);

            actual = Statistics.NormalDistributionCumulative(13, 10, 3);  // 1 SDEV + mean
            Assert.InRange(actual, 0.839, 0.841);

        }


        [Fact]
        public void Statistics_NormalDistributionBetween()
        {
            double actual = Statistics.NormalDistributionBetween(7, 13, 10, 3); // +/- 1 Std Dev
            Assert.InRange(actual, 0.67, 0.69);

            actual = Statistics.NormalDistributionBetween(4, 16, 10, 3); // +/- 2 Std Dev
            Assert.InRange(actual, 0.94, 0.96);
        }


        [Fact]
        public void NormalDistributionCumulative_Inverse()
        {
            double actual = Statistics.NormalDistributionCumulative_Inverse(0.001, 10, 3);
            Assert.Equal(1, actual); // 10 - 3 * 3 = 1

            actual = Statistics.NormalDistributionCumulative_Inverse(0.998, 10, 3);
            Assert.Equal(19, actual); // 10 + 3 * 3 = 19 

            actual = Statistics.NormalDistributionCumulative_Inverse(0.1571, 10, 3);
            Assert.InRange(actual, 6.5, 7.5);

        }

    }
}
