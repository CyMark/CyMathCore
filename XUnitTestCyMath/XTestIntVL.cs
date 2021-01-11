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


        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(10, 0, 10)]
        [InlineData(-10, 0, -10)]
        [InlineData(0, -10, -10)]
        [InlineData(100, -10, 90)]
        [InlineData(-100, 10, -90)]
        [InlineData(-999999999, 1000000000, 1)]
        [InlineData(999999999999999998, 1, 999999999999999999)]
        public void IntVL_Addition(long A, long B, long R)
        {
            IntVL a = new IntVL(A);
            IntVL b = new IntVL(B);
            IntVL res = new IntVL(R);
            Assert.Equal(res, a + b);
        }


        [Theory]
        [InlineData("-999999999", "1000000000", "1")]
        [InlineData("-000000999999999", "000000001000000000", "1")]
        [InlineData("987654321987654321", "987654321987654321", "1975308643975308642")]
        public void IntVL_AdditionLarge(string A, string B, string R)
        {
            IntVL a = new IntVL(A);
            IntVL b = new IntVL(B);
            IntVL res = new IntVL(R);
            Assert.Equal(res, a + b);
        }


        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(10, 0, 10)]
        [InlineData(-10, 0, -10)]
        [InlineData(0, -10, 10)]
        [InlineData(100, -10, 110)]
        [InlineData(-100, 10, -110)]
        [InlineData(-999999999, 1000000000, -1999999999)]
        [InlineData(999999999999999998, 1, 999999999999999997)]
        public void IntVL_Subtraction(long A, long B, long R)
        {
            IntVL a = new IntVL(A);
            IntVL b = new IntVL(B);
            IntVL res = new IntVL(R);
            Assert.Equal(res, a - b);
        }


        [Theory]
        [InlineData("-999999999", "1000000000", "-1999999999")]
        [InlineData("-000000999999999", "000000001000000000", "-1999999999")]
        [InlineData("987654321987654321", "987654321987654321", "0")]
        [InlineData("-987654321987654321", "987654321987654321", "-1975308643975308642")]
        public void IntVL_SubtractionLarge(string A, string B, string R)
        {
            IntVL a = new IntVL(A);
            IntVL b = new IntVL(B);
            IntVL res = new IntVL(R);
            Assert.Equal(res, a - b);
        }


        [Theory]
        [InlineData("0", "0", "0")]
        [InlineData("10", "0", "0")]
        [InlineData("0", "10", "0")]
        [InlineData("1", "10", "10")]
        [InlineData("10", "1", "10")]
        [InlineData("10", "-1", "-10")]
        [InlineData("-10", "1", "-10")]
        [InlineData("-10", "-1", "10")]
        [InlineData("987654321", "987654321", "975461057789971041")]
        [InlineData("987654321", "-987654321", "-975461057789971041")]
        public void IntVL_Multiply(string A, string B, string R)
        {
            IntVL a = new IntVL(A);
            IntVL b = new IntVL(B);
            IntVL res = new IntVL(R);
            Assert.Equal(res, a * b);
            var r = a.Multiply(b);
        }



        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(0, 10, 0)]
        [InlineData(0, 1000, 0)]
        [InlineData(10, 1000, 0)]
        [InlineData(10, 100, 0)]
        [InlineData(1, 10, 0)]
        [InlineData(6000, 3000, 2)]
        [InlineData(6000, -3000, -2)]
        [InlineData(-6000, 3000, -2)]
        [InlineData(-6000, -3000, 2)]
        [InlineData(10, 2, 5)]
        [InlineData(100, 2, 50)]
        [InlineData(100, 20, 5)]
        [InlineData(1000, 500, 2)]
        [InlineData(1000, 50, 20)]
        [InlineData(1000, 5, 200)]
        [InlineData(1000, 250, 4)]
        [InlineData(1000, 25, 40)]
        [InlineData(123456, 63, 1959)]
        [InlineData(987654024, 777, 1271112)]
        [InlineData(987654321, 7777, 126996)]
        public void IntVL_Division(long numerator, long denominator, long result)
        {
            IntVL Num = new IntVL(numerator);
            IntVL Den = new IntVL(denominator);
            IntVL Res = new IntVL(result);
            Assert.Equal(Res, Num / Den);
        }

        [Theory]
        [InlineData("10","100","0","0")]
        [InlineData("100","100","1","0")]
        [InlineData("100","10","10","0")]
        [InlineData("100","9","11","1")]
        [InlineData("9876","33","299","9")]
        [InlineData("987654321", "777", "1271112","297")]
        [InlineData("987654321", "7777", "126996", "6429")]
        [InlineData("-987654321", "7777", "-126996", "6429")]
        [InlineData("-987654321", "-7777", "126996", "6429")]
        [InlineData("987654321", "-7777", "-126996", "6429")]
        public void IntVL_Divide(string numerator, string denominator, string quotient, string remainder)
        {
            IntVL Numerator = new IntVL(numerator);
            IntVL Denominator = new IntVL(denominator);
            IntVL Quotient = new IntVL(quotient);
            IntVL Remainder = new IntVL(remainder);
            //var res = IntVL.Dev
            //Numerator.Div
            DivisionResult res = IntVL.DivisionVL(Numerator, Denominator);
            Assert.Equal(Quotient, res.Quotient);
            Assert.Equal(Remainder, res.Remainder);
        }

        [Theory]
        [InlineData("10", "100",  "0")]
        [InlineData("100", "100","0")]
        [InlineData("987654321", "777", "297")]
        [InlineData("987654321", "7777", "6429")]
        [InlineData("-987654321", "7777", "6429")]
        [InlineData("-987654321", "-7777", "6429")]
        [InlineData("987654321", "-7777", "6429")]
        public void IntVL_Modulus(string numerator, string denominator, string modulus)
        {
            IntVL Numerator = new IntVL(numerator);
            IntVL Denominator = new IntVL(denominator);
            IntVL Remainder = new IntVL(modulus);
            IntVL result = Numerator % Denominator;
            Assert.Equal(Remainder, result);
        }


    }
}
