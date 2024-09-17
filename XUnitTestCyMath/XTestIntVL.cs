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
            IntVL v = new();
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
            IntVL v = new(input);
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
            IntVL v = new(input);
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
            IntVL a = new(A);
            IntVL b = new(B);
            IntVL res = new(R);
            Assert.Equal(res, a + b);
        }


        [Theory]
        [InlineData("-999999999", "1000000000", "1")]
        [InlineData("-000000999999999", "000000001000000000", "1")]
        [InlineData("987654321987654321", "987654321987654321", "1975308643975308642")]
        public void IntVL_AdditionLarge(string A, string B, string R)
        {
            IntVL a = new(A);
            IntVL b = new(B);
            IntVL res = new(R);
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
            IntVL a = new(A);
            IntVL b = new(B);
            IntVL res = new(R);
            Assert.Equal(res, a - b);
        }


        [Theory]
        [InlineData("-999999999", "1000000000", "-1999999999")]
        [InlineData("-000000999999999", "000000001000000000", "-1999999999")]
        [InlineData("987654321987654321", "987654321987654321", "0")]
        [InlineData("-987654321987654321", "987654321987654321", "-1975308643975308642")]
        public void IntVL_SubtractionLarge(string A, string B, string R)
        {
            IntVL a = new(A);
            IntVL b = new(B);
            IntVL res = new(R);
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
            IntVL a = new(A);
            IntVL b = new(B);
            IntVL res = new(R);
            Assert.Equal(res, a * b);
            _ = a.Multiply(b);
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
            IntVL Num = new(numerator);
            IntVL Den = new(denominator);
            IntVL Res = new(result);
            Assert.Equal(Res, Num / Den);
        }

        [Theory]
        [InlineData("1", "9", "0", "1")]
        [InlineData("2", "9", "0", "2")]
        [InlineData("5", "9", "0", "5")]
        [InlineData("8", "9", "0", "8")]
        [InlineData("9", "9", "1", "0")]
        [InlineData("99","100","0","99")]
        [InlineData("10","100","0","10")]
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
            IntVL Numerator = new(numerator);
            IntVL Denominator = new(denominator);
            IntVL Quotient = new(quotient);
            IntVL Remainder = new(remainder);
            //var res = IntVL.Dev
            //Numerator.Div
            DivisionResult res = IntVL.DivisionVL(Numerator, Denominator);
            Assert.Equal(Quotient, res.Quotient);
            Assert.Equal(Remainder, res.Remainder);
        }

        [Theory]
        [InlineData("1", "9",  "1")]
        [InlineData("2", "9",  "2")]
        [InlineData("3", "9",  "3")]
        [InlineData("4", "9",  "4")]
        [InlineData("8", "9",  "8")]
        [InlineData("9", "9",  "0")]
        [InlineData("10", "100",  "10")]
        [InlineData("100", "100","0")]
        [InlineData("987654321", "777", "297")]
        [InlineData("987654321", "7777", "6429")]
        [InlineData("-987654321", "7777", "6429")]
        [InlineData("-987654321", "-7777", "6429")]
        [InlineData("987654321", "-7777", "6429")]
        public void IntVL_Modulus(string numerator, string denominator, string modulus)
        {
            IntVL Numerator = new(numerator);
            IntVL Denominator = new(denominator);
            IntVL Remainder = new(modulus);
            IntVL result = Numerator % Denominator;
            Assert.Equal(Remainder, result);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("0",0)]
        [InlineData("1", 1)]
        [InlineData("5", 5)]
        [InlineData("9", 9)]
        [InlineData("10", 1)]
        [InlineData("11", 2)]
        [InlineData("15", 6)]
        [InlineData("18", 9)]
        [InlineData("19", 1)]
        [InlineData("123", 6)]
        [InlineData("1234", 1)]
        [InlineData("12345", 6)]
        [InlineData("123456", 3)]
        [InlineData("987654321", 9)]
        [InlineData("999", 9)]
        [InlineData("-1", 1)]
        [InlineData("-5", 5)]
        [InlineData("-9", 9)]
        [InlineData("-987654321", 9)]
        public void IntVL_DigitalRoot(string digitStr, int  digitalRoot)
        {
            IntVL check = new(digitStr);
            int result = check.DigitalRoot();
            Assert.Equal(digitalRoot, result);
        }


        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(20, 6765)]
        [InlineData(50, 12586269025)]
        [InlineData(70, 190392490709135)]
        [InlineData(90, 2880067194370816120)]
        public void IntVL_Fibonacci(int input,Int64 output)
        {
            IntVL result = new(output);
            IntVL actual = MathVL.Fibonacci(input);
            Assert.Equal(actual, result);
        }

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
        public void IntVL_IsPrime(long val, bool isPrime)
        {
            IntVL valLarge = new(val);
            Assert.Equal(isPrime, MathVL.IsPrime(valLarge));
        }


    }
}
