using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{
    public static class MathVL
    {

        public static IntVL Fibbonacci(int val) => Fibbonacci(val, new Dictionary<int, IntVL>());

        // https://www.youtube.com/watch?v=oBt53YbR9Kk&t=4996s
        public static IntVL Fibbonacci(int val, Dictionary<int, IntVL> memo)
        {
            if (memo.ContainsKey(val)) { return memo[val]; }
            if (val <= 0) { throw new InvalidOperationException("*Error: Cannot caluculate Fibonacci nr for zero or negative numbers"); }
            if (val <= 2) { return new IntVL(1); }
            IntVL result = Fibbonacci(val - 2, memo) + Fibbonacci(val - 1, memo);
            memo.Add(val, result);
            return memo[val];
        }
        /*
        public static IntVL Fibbonacci2(int val)
        {
            if (val <= 0) { throw new InvalidOperationException("*Error: Cannot caluculate Fibonacci nr for zero or negatinve numbers"); }
            if (val <= 2) { return new IntVL(1); }
            List<IntVL> array = new List<IntVL>();
            array.Add(new IntVL(0));
            array.Add(new IntVL(1));
            array.Add(new IntVL(1));
            return Fibo(val, array);
        }
        */

        public static IntVL Padovan(int val)
        {
            if (val < 0) { throw new InvalidOperationException("*Error: Cannot caluculate Padovan nr for zero or negative numbers"); }
            if (val == 0) { return new IntVL(1); }
            if (val == 1 || val == 2) { return new IntVL(); }
            //if (val <= 3) { return new IntVL(1); }
            return Padovan(val - 3) + Padovan(val - 2);
        }
        
        //static IntVL FibbonacciA(int val)
        //{
        //    if (val <= 0) { throw new InvalidOperationException("*Error: Cannot caluculate Fibonacci nr for zero or negative numbers"); }
        //    if (val == 1) { return new IntVL(1); }
        //    IntVL p2 = new(0);
        //    IntVL p1 = new(1);
        //    return Fibo2(val, p1, p2);
        //}
        
        
        //static IntVL Fibo2(int val, IntVL p1, IntVL p2)
        //{
        //    if(val-- < 2) { return p1; }
        //    return Fibo2(val, p1 + p2, p1);
        //}
        
        public static IntVL Abs(IntVL v) => v.Absolute();


        public static IntVL Factorial(int nr)
        {
            if (nr < 0) { throw new InvalidOperationException("*Error: Cannot caluculate Factorial for negative numbers"); }
            //if (nr == 0) { return new IntVL(1); }
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

            return new IntVL();
        }

        public static bool IsPrime(IntVL checkPrime)
        {
            return new EratosthenesVL().IsPrime(checkPrime);
        }
        

    }
}
