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

            ComplexNumber c1 = new (10.0, 10.0);
            ComplexNumber c2 = new (-10.0, 10.0);
            ComplexNumber result = new (0, 20);
            Assert.Equal(result, c1+c2);
            
            result = new (20, 0);
            Assert.Equal(result, c1 - c2);

            result = new (-20, 0);
            Assert.Equal(result, c2 - c1);

            result = new (-200, 0);
            Assert.Equal(result, c2 * c1);
            Assert.Equal(result, c1 * c2);

            ComplexNumber Conj = c1.Conjugate();
            result = new(10, -10);
            Assert.Equal(result, Conj);

            Conj = c2.Conjugate();
            result = new(-10, -10);
            Assert.Equal(result, Conj);

            result = new(200, 0);
            Assert.Equal(result, c2 * Conj);

            result = new(0, -1);
            Assert.Equal(result, c1 / c2);

            result = new(0, 1);
            Assert.Equal(result, c2 / c1);
        }
    }
}
