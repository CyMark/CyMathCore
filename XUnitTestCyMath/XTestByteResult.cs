using System;
using Xunit;
using CyMathCore;


namespace XUnitTestCyMath
{
    public class XTestByteResult
    {
        [Fact]
        public void ByteResult_Add()
        {
            ByteResult bres = new ByteResult();
            Assert.Equal(0, bres.Result);
            Assert.Equal(0, bres.OverFlow);

            bres = new ByteResult(0xF);
            Assert.Equal(15, bres.Result);
            Assert.Equal(0, bres.OverFlow);
            Action action = () => bres.Add(0xF, 1);
            Assert.Throws<ArithmeticException>(action);

            bres.Add(0x9, 1);
            Assert.Equal(0, bres.Result);
            Assert.Equal(1, bres.OverFlow);
        }

        [Theory]
        [InlineData(0x0, 0x0, 0x0, 0x0)]
        [InlineData(0x0, 0x1, 0x1, 0x0)]
        [InlineData(0x1, 0x0, 0x1, 0x0)]
        [InlineData(0x9, 0x0, 0x9, 0x0)]
        [InlineData(0x9, 0x1, 0x0, 0x1)]
        [InlineData(0x1, 0x9, 0x0, 0x1)]
        [InlineData(0x2, 0x9, 0x1, 0x1)]
        [InlineData(0x3, 0x9, 0x2, 0x1)]
        [InlineData(0x9, 0x9, 0x8, 0x1)]
        public void ByteResult_Addition(byte A, byte B, byte Res, byte Overflow)
        {
            ByteResult b = new ByteResult();
            b.Add(A, B);
            Assert.Equal(Res, b.Result);
            Assert.Equal(Overflow, b.OverFlow);
        }


        [Theory]
        [InlineData(0x0, 0x0, 0x0, 0x0)]
        [InlineData(0x0, 0x1, 0x0, 0x0)]
        [InlineData(0x1, 0x0, 0x0, 0x0)]
        [InlineData(0x9, 0x0, 0x0, 0x0)]
        [InlineData(0x9, 0x1, 0x9, 0x0)]
        [InlineData(0x1, 0x9, 0x9, 0x0)]
        [InlineData(0x2, 0x9, 0x8, 0x1)]
        [InlineData(0x9, 0x2, 0x8, 0x1)]
        [InlineData(0x9, 0x3, 0x7, 0x2)]
        [InlineData(0x9, 0x9, 0x1, 0x8)]
        public void ByteResultMultiplication(byte A, byte B, byte Res, byte Overflow)
        {
            ByteResult b = new ByteResult();
            b.Multiply(A, B);
            Assert.Equal(Res, b.Result);
            Assert.Equal(Overflow, b.OverFlow);
        }



        [Theory]
        [InlineData(0x0, 0x0, 0x0, 0x0)]
        [InlineData(0x0, 0x1, 0x9, 0x1)]
        [InlineData(0x1, 0x0, 0x1, 0x0)]
        [InlineData(0x9, 0x0, 0x9, 0x0)]
        [InlineData(0x9, 0x8, 0x1, 0x0)]
        [InlineData(0x1, 0x9, 0x2, 0x1)]
        [InlineData(0x2, 0x9, 0x3, 0x1)]
        [InlineData(0x8, 0x9, 0x9, 0x1)]
        [InlineData(0x9, 0x9, 0x0, 0x0)]
        public void ByteResult_Subtraction(byte A, byte B, byte Res, byte Overflow)
        {
            ByteResult b = new ByteResult();
            b.Subtract(A, B);
            Assert.Equal(Res, b.Result);
            Assert.Equal(Overflow, b.OverFlow);
        }


        [Fact]
        public void ByteResult_DivideByZero()
        {
            ByteResult b = new ByteResult();
            Action action = () => b.Divide(0x9, 0x0);
            Assert.Throws<DivideByZeroException>(action);
        }


        [Theory]
        [InlineData(0x0, 0x9, 0x0, 0x0)]
        [InlineData(0x0, 0x1, 0x0, 0x0)]
        [InlineData(0x9, 0x8, 0x1, 0x1)]
        [InlineData(0x1, 0x9, 0x0, 0x1)]
        [InlineData(0x2, 0x9, 0x0, 0x2)]
        [InlineData(0x8, 0x9, 0x0, 0x8)]
        [InlineData(0x9, 0x9, 0x1, 0x0)]
        [InlineData(0x8, 0x6, 0x1, 0x2)]
        [InlineData(0x8, 0x7, 0x1, 0x1)]
        [InlineData(0x8, 0x3, 0x2, 0x2)]
        [InlineData(0x8, 0x2, 0x4, 0x0)]
        public void ByteResult_Division(byte A, byte B, byte Res, byte Overflow)
        {
            ByteResult b = new ByteResult();
            b.Divide(A, B);
            Assert.Equal(Res, b.Result);
            Assert.Equal(Overflow, b.OverFlow);
        }


    }
}
