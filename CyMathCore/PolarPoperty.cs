using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{
    public class PolarPoperty
    {
        public PolarPoperty()
        {
            Radius = 0;
            Angle = 0;
        }

        public double Radius  { get; set; }
        public double Angle  { get; set; }

        public PolarPoperty CalculatePolar(double XValue, double YValue)
        {
            PolarPoperty prop = new();

            if (XValue == 0 && YValue == 0) { return prop; }

            Radius = Math.Sqrt(XValue * XValue + YValue * YValue);
            Angle = Math.Atan2(YValue, XValue);

            return prop;
        }

    }
}
