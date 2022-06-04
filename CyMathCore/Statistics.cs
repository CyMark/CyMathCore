using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{

    public static class Statistics
    {

        /// <summary>
        /// Returns the probabity at a value on the x-axist of a bell curve
        /// with mean and standard deviation stdDeviation
        /// </summary>
        /// <param name="value">Indpendent varialble for which the distribtion function is calcualted</param>
        /// <param name="mean">Mean of probability function</param>
        /// <param name="stDeviation">Standard Deviation of probablity function</param>
        /// <returns></returns>
        public static double NormalDistribution(double value, double mean, double stDeviation)
        {
            double exponent = Math.Pow( (value - mean) / stDeviation, 2) / -2;

            double constFact = 1 / (Math.Sqrt(2 * Math.PI) * stDeviation);

            return constFact * Math.Exp(exponent);
        }

        /// <summary>
        /// Probablity is area under Normal curve from -infinity (3 std deviactions) to value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mean"></param>
        /// <param name="stDeviation"></param>
        /// <returns></returns>
        public static double NormalDistributionCumulative(double value, double mean, double stDeviation)
        {
            // calc area under the curve in 1000 units between mean - 3 * stdDev and mean + 3 * stdDeve

            if (value < mean - 3*stDeviation) { return 0; }
            if (value > mean + 3*stDeviation) { return 1; }

            int Nr = 1000;
            double range = 6 * stDeviation;
            double width = range / (double)Nr;

            double result = 0;
            double nextValue = mean - 3 * stDeviation - width;
            for (int n = 0; n < Nr; n++)
            {
                result += NormalDistribution(nextValue, mean, stDeviation) * width;
                nextValue += width;
                if (nextValue >= value) { break; }
            }

            return result;
        }

        /// <summary>
        /// Returns the probability of event happening between Low and High
        /// </summary>
        /// <param name="valueLow"></param>
        /// <param name="valueHigh"></param>
        /// <param name="mean"></param>
        /// <param name="stDeviation"></param>
        /// <returns></returns>
        public static double NormalDistributionBetween(double valueLow, double valueHigh, double mean, double stDeviation)
        {
            if (valueHigh < valueLow) { return 0; }

            return NormalDistributionCumulative(valueHigh, mean, stDeviation) - NormalDistributionCumulative(valueLow, mean, stDeviation);
        }

        public static double NormalDistributionCumulative_Inverse(double probability, double mean, double stDeviation)
        {

            if (probability < 0.003) { return mean - 3*stDeviation; }
            if (probability > 0.997) { return mean + 3*stDeviation; }
            if (probability > 0.49 && probability < 0.51) { return mean; }

            double result;

            //double trail_value;
            double low;
            double high;
            double range;

            if (probability < 0.5)
            {
                low = mean - 3 * stDeviation;
                high = mean;
                
            }
            else
            {
                low = mean;
                high = mean + 3 * stDeviation;
            }
            range = high - low;
            result = low + range / 2;

            int maxLoops = 1000;
            while (range > (3 * stDeviation) * 0.01 && maxLoops > 0) // 1% of max range
            {
                double test = NormalDistributionCumulative(result, mean, stDeviation);
                if  (test < probability)
                {
                    high = result;
                }
                else
                {
                    low = result;
                }
                range = high - low;
                result = low + range / 2;

                if (--maxLoops < 0) { break; }
            }

            return result;
        }


    }
}
