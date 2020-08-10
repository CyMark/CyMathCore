using System;
using Xunit;
using CyMathCore;

namespace XUnitTestCyMath
{
    public class XTestIntVL
    {
        [Fact]
        public void IntVL_Construction()
        {
            IntVL v = new IntVL();
            Assert.Equal(0, v);
            v = new IntVL(223);
            Assert.Equal(223, v);
            long res = 1234567890111;
            v = new IntVL(res);
            Assert.Equal(res, v);
            string rstr = "1234567890111";
            v = new IntVL(rstr);
            Assert.Equal(res, v);

            rstr = "1234567890111";
            v = new IntVL(rstr);
            Assert.Equal(res, v);
        }


        [Theory]
        [InlineData("123", "123")]
        [InlineData("000123", "123")]
        [InlineData("-123", "-123")]
        [InlineData("-00000000000000000000000000123", "-123")]
        [InlineData("-100000000000000000000000000123", "-100000000000000000000000000123")] // too large for long
        [InlineData("987654321987654321987654321987654321", "987654321987654321987654321987654321")] // too large for long
        public void IntVL_Parse(string input, string output)
        {
            IntVL v = new IntVL(input);
            Assert.Equal(output, v.ToString());
        }

        [Theory]
        [InlineData("123", 123)]
        [InlineData("000123", 123)]
        [InlineData("-123", -123)]
        [InlineData("-00000000000000000000000000123", -123)]
        [InlineData("-100000000000000123", -100000000000000123)]
        [InlineData("987654321987654321", 987654321987654321)]
        public void IntVL_ParseLong(string input, long output)
        {
            IntVL v = new IntVL(input);
            Assert.Equal(output, v);
        }

    }
}
