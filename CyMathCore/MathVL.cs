using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{
    public static class MathVL
    {

        public static IntVL Fibonacci(int val) => Fibonacci(val, new Dictionary<int, IntVL>());

        // https://www.youtube.com/watch?v=oBt53YbR9Kk&t=4996s
        public static IntVL Fibonacci(int val, Dictionary<int, IntVL> memo)
        {
            if (memo.TryGetValue(val, out IntVL value)) { return value; }
            if (val <= 0) { throw new InvalidOperationException("*Error: Cannot calculate Fibonacci nr for zero or negative numbers"); }
            if (val <= 2) { return new IntVL(1); }
            IntVL result = Fibonacci(val - 2, memo) + Fibonacci(val - 1, memo);
            memo.Add(val, result);
            return memo[val];
        }
        
        public static IntVL Padovan(int val)
        {
            if (val < 0) { throw new InvalidOperationException("*Error: Cannot calculate Padovan nr for zero or negative numbers"); }
            if (val == 0) { return new IntVL(1); }
            if (val == 1 || val == 2) { return new IntVL(); }
            //if (val <= 3) { return new IntVL(1); }
            return Padovan(val - 3) + Padovan(val - 2);
        }

        
        public static IntVL Abs(IntVL v) => v.Absolute();


        public static IntVL Factorial(int nr)
        {
            if (nr < 0) { throw new InvalidOperationException("*Error: Cannot calculate Factorial for negative numbers"); }
            if (nr <= 1) { return new IntVL(1); }
            return new IntVL(nr) * Factorial(nr - 1);
        }


        /// <summary>
        /// Calculates the Greatest Common Divisor between two numbers
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static IntVL GCD(IntVL val1, IntVL val2)
        {
            if (val1.IsZero || val2.IsZero) { return new IntVL(); }
            if(val1 == 1 || val2 == 1) { return new IntVL(1); }

            IntVL large = new(val1.Absolute());
            IntVL small = new(val2.Absolute());
            if (large < small)
            {
                large = new(val2.Absolute());
                small = new(val1.Absolute());
            }
            //TODO:  Euclid's algorithm 
            throw new NotImplementedException();

            return new IntVL();
        }

        public static bool IsPrime(IntVL checkPrime)
        {
            if (checkPrime >= 0 && checkPrime < long.MaxValue)
            {
                PrimeCalc calc = new();
                return calc.IsPrime(checkPrime.ToInt64());
            }

            return new EratosthenesVL().IsPrime(checkPrime);
        }
        

    }
}
