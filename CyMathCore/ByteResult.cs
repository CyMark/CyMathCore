using System;

namespace CyMathCore
{
    /// <summary>
    /// Apply arithmetic to a single byte with overflow.  Max byte value is 0x9
    /// </summary>
    public class ByteResult
    {
        public ByteResult() { Result = 0; OverFlow = 0; }
        public ByteResult(byte a) { Result = a; OverFlow = 0; }

        public byte Result { get; private set; }
        public byte OverFlow { get; private set; }

        public void Add(byte a, byte b)
        {
            CheckError(a);
            CheckError(b);
            int res = a + b;
            if (res > 9) { OverFlow = 1; res %= 10; }
            else { OverFlow = 0; }
            Result = (byte)res;
        }


        public void Subtract(byte a, byte b)
        {
            CheckError(a);
            CheckError(b);
            if (b > a)
            {
                OverFlow = 1;
                Result = (byte)(10 + a - b);
            }
            else
            {
                OverFlow = 0;
                Result = (byte)(a - b);
            }
        }

        public void Multiply(byte a, byte b)
        {
            CheckError(a);
            CheckError(b);
            int val = a * b;
            if (val < 10)
            {
                OverFlow = 0;
                Result = (byte)val;
            }
            else
            {
                Result = (byte)(val % 10);
                OverFlow = (byte)(val / 10);
            }
        }

        public void Divide(byte a, byte b)
        {
            CheckError(a);
            CheckError(b);
            if (b == 0) { throw new DivideByZeroException("*Error: Division by zero!"); }
            if (a == 0)
            {
                OverFlow = 0;
                Result = 0;
            }
            else
            {
                OverFlow = (byte)(a % b);
                Result = (byte)(a / b);
            }
        }

        private static void CheckError(byte a)
        {
            if (a > 9) { throw new ArithmeticException($"*Error: Value range for byte is 0-9, but was {a}!"); }
        }

    }
}
