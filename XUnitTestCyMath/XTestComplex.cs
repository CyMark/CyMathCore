using System;
using Xunit;
using CyMathCore;

namespace XUnitTestCyMath
{
    public class XTestComplex
    {
        [Fact]
        public void ComplexNumber_Basics()
        {
            ComplexNumber z = new();
            Assert.Equal(0, z.Real);
            Assert.Equal(0, z.Imaginary);
        }
    }
}
