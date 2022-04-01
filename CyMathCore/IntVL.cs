using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{
    /// <summary>
    /// Manage integers > 19 digits in length
    /// </summary>
    public class IntVL : IComparable
    {
        bool positive;
        byte[] digits;
        const byte nil = 0;

        public IntVL()
        {
            positive = true;
            digits = new byte[1];
            digits[0] = 0;
        }
        

        public IntVL(bool sign, byte[] digitArray)
        {
            positive = sign;
            digits = CopyArray(digitArray);
        }

        public IntVL(IntVL copy)
        {
            positive = copy.positive;
            digits = CopyArray(copy.digits);
        }

        public IntVL(string digitString)
        {
            IntVL v = IntVL.Parse(digitString);

            positive = v.positive;
            digits = CopyArray(v.digits);
        }

        public IntVL(long val)
        {
            long res = val;
            if (val < 0)
            {
                positive = false;
                res = -1 * val;
            }
            else
            {
                positive = true;
            }

            IntVL v = IntVL.Parse(res.ToString());
            digits = CopyArray(v.digits);

        }

        public IntVL(int val) : this((long)val)
        {; }


        public bool IsZero
        {
            get
            {
                this.Compact();
                if (Length > 1) { return false; }

                if (!IsNil(digits[0])) { return false; }

                positive = true;
                return true;
            }
        }

        public bool IsEmpty
        {
            get
            {
                this.Compact();
                if (Length == 0) { return true; }

                return true;
            }
        }

        //---- internal

        bool IsNil(byte digit) => (nil | digit) == 0;
        byte TrimByte(byte digit) => digit < 10 ? digit : (byte)9;
        //---- internal

        public int Length { get { return digits.Length; } }

        public void ToNegative() => positive = false;
        public void ToPositive() => positive = true;

        public bool IsPositive
        {
            get { return positive; }
        }

        public byte this[int x]
        {
            get
            {
                return digits[x];
            }
        }


        //--------------- Operators --------------------------------
        #region Operators


        public static bool operator ==(IntVL left, IntVL right) => left.Equals(right);
        public static bool operator !=(IntVL left, IntVL right) => !left.Equals(right);

        public static bool operator ==(IntVL left, int right) => left.Equals(new IntVL(right));
        public static bool operator !=(IntVL left, int right) => !left.Equals(new IntVL(right));

        public static bool operator ==(IntVL left, long right) => left.Equals(new IntVL(right));
        public static bool operator !=(IntVL left, long right) => !left.Equals(new IntVL(right));


        public static bool operator >(IntVL left, IntVL right) => left.CompareTo(right) > 0;
        public static bool operator <(IntVL left, IntVL right) => left.CompareTo(right) < 0;

        public static bool operator >=(IntVL left, IntVL right) => left.CompareTo(right) >= 0;
        public static bool operator <=(IntVL left, IntVL right) => left.CompareTo(right) <= 0;

        public static bool operator >(IntVL left, int right) => left.CompareTo(new IntVL(right)) > 0;
        public static bool operator <(IntVL left, int right) => left.CompareTo(new IntVL(right)) < 0;

        public static bool operator >=(IntVL left, int right) => left.CompareTo(new IntVL(right)) >= 0;
        public static bool operator <=(IntVL left, int right) => left.CompareTo(new IntVL(right)) <= 0;

        public static bool operator >(IntVL left, long right) => left.CompareTo(new IntVL(right)) > 0;
        public static bool operator <(IntVL left, long right) => left.CompareTo(new IntVL(right)) < 0;

        public static bool operator >=(IntVL left, long right) => left.CompareTo(new IntVL(right)) >= 0;
        public static bool operator <=(IntVL left, long right) => left.CompareTo(new IntVL(right)) <= 0;


        // +
        public static IntVL operator +(IntVL left, IntVL right) => left.Add(right);
        public static IntVL operator +(IntVL left, int right) => left.Add(new IntVL(right));
        //public static IntVL operator +(int left, IntVL right) => right.Add(new IntVL(left));
        public static IntVL operator +(IntVL left, long right) => left.Add(new IntVL(right));
        //public static IntVL operator +(long left, IntVL right) => right.Add(new IntVL(left));

        // ++
        public static IntVL operator ++(IntVL val) => val + new IntVL(1);
        // --
        public static IntVL operator --(IntVL val) => val - new IntVL(1);
        

        // assignemt
        public static implicit operator IntVL(int x) => new(x);
        public static implicit operator IntVL(long x) => new(x);

        public static explicit operator int(IntVL x) => x.ToInt32();
        public static explicit operator long(IntVL x) => x.ToInt64();

        // -
        public static IntVL operator -(IntVL left, IntVL right) => left.Subtract(right);
        public static IntVL operator -(IntVL left, int right) => left.Subtract(new IntVL(right));
        public static IntVL operator -(IntVL left, long right) => left.Subtract(new IntVL(right));

        // *
        public static IntVL operator *(IntVL left, IntVL right) => left.Multiply(right);

        // /
        public static IntVL operator /(IntVL left, IntVL right) => left.Divide(right);

        public static IntVL operator %(IntVL left, IntVL right)
        {
            IntVL temp = new(left);
            return temp.Modulus(right);
        }


        public IntVL Add(IntVL val) => Combine(val);


        public IntVL Negate()
        {
            IntVL res = new(this)
            {
                positive = !positive
            };

            return res;
        }

        public IntVL Combine(IntVL val)
        {
            if (val.Negate() == this) { return new(); } // + and minus the same
            bool sign = val.positive;

            byte[] result;// = null;

            if (this.positive == val.positive)
            {
                result = ByteAdd(val.digits);
            }
            else
            {
                if(this.Absolute() < val.Absolute())
                {
                    result = ByteSubtract(this.digits, val.digits);
                    //sign = !sign;
                }
                else
                {
                    result = ByteSubtract(val.digits, this.digits);
                    sign = !sign;
                }
            }

            //if(result == null) { return new IntVL(121212121212); }

            IntVL output = new(sign, result);
            return output;

        } // Combine


        private byte[] ByteAdd(byte[] val)
        {
            int mxLen = Length;
            if (val.Length > Length) { mxLen = val.Length; }
            mxLen++;
            byte[] result = new byte[mxLen];
            byte residual = 0;
            ByteResult r = new();

            for (int n = 0; n < mxLen; n++)
            {
                byte A = 0;
                byte B = 0;
                if (n < Length) { A = digits[n]; }
                if (n < val.Length) { B = val[n]; }
                if (residual > 0)
                {
                    r.Add(residual, A);
                    A = r.Result;
                    residual = r.OverFlow;
                }
                r.Add(A, B);
                result[n] = r.Result;
                residual += r.OverFlow;
            }

            return result;
        }


        private byte[] ByteSubtract(byte[] th, byte[] val)
        {
            int mxLen = th.Length;
            if (val.Length > th.Length) { mxLen = val.Length; }
            mxLen++;
            byte[] result = new byte[mxLen];
            byte residual = 0;
            ByteResult r = new();

            for (int n = 0; n < mxLen; n++)
            {
                byte A = 0;
                byte B = 0;
                if (n < val.Length) { A = val[n]; }
                if (n < th.Length) { B = th[n]; }
                if (residual > 0)
                {
                    r.Subtract(A, residual);
                    A = r.Result;
                    residual = r.OverFlow;
                }
                r.Subtract(A, B);
                result[n] = r.Result;
                residual += r.OverFlow;
            }

            return result;
        }



        public IntVL Subtract(IntVL val) => Combine(val.Negate());
        /*
        
        public IntVL Subtract(IntVL val)
        {
            if (IsZero) { return new IntVL(val); }
            if (val.IsZero) { return new IntVL(this); }
            
            if (val.positive == this.positive)
            {
                IntVL tmp = new IntVL(val);
                //tmp.positive = true; // make positve
                return Add(tmp);
            }
            
            if (val.positive && !positive)
            {
                IntVL tmp = new IntVL(this);
                tmp.positive = true; // make positve
                return val.Add(tmp);
            }
            

            IntVL output = new IntVL();

            int mxLen = Length;
            if (val.Length > Length) { mxLen = val.Length; }
            mxLen++;
            byte[] result = new byte[mxLen];
            byte residual = 0;
            ByteResult r = new ByteResult();
            if (this >= val)
            {
                for (int n = 0; n < mxLen; n++)
                {
                    byte A = 0;
                    byte B = 0;
                    if (n < Length) { A = digits[n]; }
                    if (n < val.Length) { B = val[n]; }
                    if (residual > 0)
                    {
                        r.Subtract(A, residual);
                        A = r.Result;
                        residual = r.OverFlow;
                    }
                    r.Subtract(A, B);
                    result[n] = r.Result;
                    residual += r.OverFlow;
                }
                output.positive = true;
            }
            else
            {
                for (int n = 0; n < mxLen; n++)
                {
                    byte A = 0;
                    byte B = 0;
                    if (n < val.Length) { A = val[n]; }
                    if (n < Length) { B = digits[n]; }
                    if (residual > 0)
                    {
                        r.Subtract(A, residual);
                        A = r.Result;
                        residual = r.OverFlow;
                    }
                    r.Subtract(A, B);
                    result[n] = r.Result;
                    residual += r.OverFlow;
                }
                output.positive = false;
            }

            //if (this >= val) { positive = true; }
            //else { positive = false; }

            output = new IntVL(output.positive, result);
            output.Compact();
            return output;
        }
        */

        private IntVL Sub(IntVL Left, IntVL Right)
        {
            if (Right.IsZero) { return new IntVL(Left); }
            if (Left.IsZero) { return new IntVL(Right) { positive = !Right.positive }; }
                
            int mxLen = Left.Length;
            if (Right.Length > Left.Length) { mxLen = Right.Length; }
            mxLen++;

            //if(val > this) { positive = false; }
            IntVL tleft = Left.Absolute();
            IntVL tright = Right.Absolute();
            IntVL left = new(tleft);
            IntVL right = new(tright);
            if (tleft < tright)
            {
                left = new IntVL(tright);
                right = new IntVL(tleft);
            }

            ByteResult res = new();
            byte[] result = new byte[mxLen];
            byte residual = 0;
            for (int n = 0; n < mxLen; n++)
            {
                byte a = 0;
                byte b = 0;
                if (n < left.Length) { a = left.digits[n]; }
                if (n < right.Length) { b = right.digits[n]; }
                
                res.Subtract(a, b);
                if (residual > 0) { res.Subtract(res.Result, residual); }
                result[n] = res.Result;
                residual = res.OverFlow;
            }

            IntVL output = new(positive, result);
            if(Left < 0)
            {
                if(tleft > tright) { output.positive = false; }
                else { output.positive = true; }
                //throw new Exception($"*Debug: output.positive={output.positive}");
            }
            else
            {
                if (tleft > tright) { output.positive = true; }
                else { output.positive = false; }
            }

            output.Compact();
            return output;
        }

        /// <summary>
        /// Multiplies by 10 n times
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public IntVL ShiftLeft(int n)
        {
            if (IsZero) { return new(this); }
            IntVL res = new(this);
            for(int i = 0; i < n; i++)
            {
                res = res.ShiftLeft();
            }
            return res;
        }

        /// <summary>
        /// Multiplies by 10 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public IntVL ShiftLeft()
        {
            if(IsZero) { return new(this); }
            this.Compact();
            byte[] array = new byte[Length + 1];
            array[0] = 0;
            for(int x = 1; x < array.Length; x++)
            {
                array[x] = digits[x - 1];
            }

            return new IntVL(positive, array);
        }



        public IntVL Multiply(IntVL right)
        {
            if (IsZero || right == 1) { return new(this); }
            if (right.IsZero) { return new(); }

            IntVL top = new(this);
            IntVL bottom = new(right);
            if(bottom.Length > top.Length)
            {
                top = new(right);
                bottom = new(this);
            }
            IntVL res = new();
            //byte[] result = new byte[top.Length + bottom.Length];
            //IntVL ntop = new IntVL(top);
            for(int n = 0; n < bottom.Length; n++)
            {
                res += top.Multiply(bottom.digits[n]);
                top = top.ShiftLeft(); // x 10
            }
            res.positive = positive == right.positive;

            return res;
        }


        /// <summary>
        /// Multiplies positive values between 0 and 9.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public IntVL Multiply(byte val)
        {
            if (IsZero || val == 1) { return new(this); }
            if(val == 0) { return new(); }
            this.Compact();

            byte[] array = new byte[Length + 1];
            byte residual = 0;
            ByteResult res = new();
            for (int n = 0; n < Length; n++)
            {
                res.Multiply(val, digits[n]);
                if (residual > 0)
                {
                    byte tmp = res.OverFlow;
                    res.Add(residual, res.Result);
                    residual = (byte)(res.OverFlow + tmp);
                }
                else
                {
                    residual = res.OverFlow;
                }
                array[n] = res.Result;
            }
            array[Length] = residual;
            IntVL output = new(positive, array);
            output.Compact();
            
            return output;
        }


        public IntVL ShiftRight(int n)
        {
            if (IsZero) { return new(this); }
            IntVL res = new(this);
            for (int i = 0; i < n; i++)
            {
                res = res.ShiftRight();
                if (res == 0) { break; }
            }
            return res;
        }


        /// <summary>
        /// Divides by 10, no rounding
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public IntVL ShiftRight()
        {
            if (IsZero) { return new(this); }
            this.Compact();
            if(Length == 1) { return new(); } // Return zero if cannot shift right anymore

            byte[] array = new byte[Length - 1];
            for (int x = 1; x < Length; x++)
            {
                array[x - 1] = digits[x];
            }

            return new IntVL(positive, array);
        }


        public IntVL Modulus(IntVL denominator)
        {
            if (denominator.IsZero) { return new(0); };

            DivisionResult divRes = DivisionVL(this, denominator);

            return divRes.Remainder;
        }


        public IntVL Divide(IntVL denominator)
        {
            if(denominator.IsZero) { throw new DivideByZeroException("IntVL cannot divide by zero!"); };

            DivisionResult divRes = DivisionVL(this, denominator);

            return divRes.Quotient;

            //IntVL result = new IntVL();
            //this.Compact();
            //denominator.Compact();

            //if(Length < denominator.Length) { return result; }  // zero if numerator has less digits that denominator

            //bool sign = this.positive == denominator.positive;

            //if (Length == denominator.Length)
            //{
            //    //return new IntVL(GetTopTwoDigits(this));
            //    int num = GetTopTwoDigits(this);
            //    int den = GetTopTwoDigits(denominator);
            //    //return new IntVL(den);
            //    result = num / den;

            //    if(!sign)
            //    {
            //        return result.Negate();
            //    }
            //    return result;
            //}

            //IntVL n = new IntVL(this);
            //IntVL d = new IntVL(denominator);

            //int nrShifts = d.Length - 2;
            
            //if(nrShifts > 0)
            //{
            //    n = n.ShiftRight(nrShifts); // divide both by 10 * nrShits to line-up with a denomimator of two digits
            //    d = d.ShiftRight(nrShifts);
            //}
            //int res = GetTopTwoDigits(n);
            //int dInt = d.ToInt32();
            //int m = 0;
            ////return new IntVL(div);
            //// long division  https://en.wikipedia.org/wiki/Division_algorithm
            //for (int k = 1; k < n.Length; k++)
            //{
            //    m *= 10;
            //    int div = res / dInt;
                
            //    m = div + m;
            //    //int v = div * dInt;
            //    int v = res - div* dInt;
            //    if(n.Length - 2 - k < 0) { break; }
            //    res = v * 10 + n.digits[n.Length - 2 - k];
            //    //if (k > 1) { throw new Exception($"res={res},div={div},m={m},v={v}"); }
            //}
            
            //result = new IntVL(m);

            //if (!sign)
            //{
            //    return result.Negate();
            //}
            //return result;
        } // Divide



        private int GetTopTwoDigits(IntVL val)
        {
            if(val.IsZero) { return 0; }
            if(val.Length == 1)
            {
                return (int)val.digits[val.Length - 1];
            }

            int result = val.digits[val.Length - 1] * 10;
            result += val.digits[val.Length - 2];
            return result;
        }

        // get the integer at byte positions pos and pos+1
        private int GetIntAtPos(IntVL val, int pos)
        {
            return 0;
        }

        public static IntVL UnitVL => new(1);

        //------------------------------------------------------------------------------
        // Division
        //------------------------------------------------------------------------------
        #region division
        /*
        /// <summary>
        /// Estimates the division of the numerator and denominator (numerator/denominator) as a starting point for convergence to final answer.
        /// If the number of digits of the denominator is longer than 17 digits (Max long - Int64 = is 19 digits), then shift right decimal (divide by 10) 
        /// until the denominator is within the Int64 digit range.  Note this method is not to be called directly in general as it is called
        /// from Div().  There is no checking here for divide by zero etc, Div takes care of that.
        /// It is also assumed that Abs(numerator) > Abs(denominator).
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns>Value of the estimate.  Sign is preserved.</returns>
        public long DivisionEstimate(IntVL numerator, IntVL denominator)
        {
            int IntShift = 0;
            IntVL divisor = new IntVL(denominator);

            //TODO:
            // if numerator in divide range then return the long 
            
            // check denominator larger than 17 digits
            if(denominator.Length > 17)
            {
                IntShift = denominator.Length - 17;
                divisor = denominator.ShiftRight(IntShift);
            }
            IntVL num = new IntVL(GetTopNrDigits(numerator, divisor.Length + 1));  // Divide the top digits with the divisor 
            num.Compact();
            long estimate = num.Absolute().ToInt64() / divisor.Absolute().ToInt64();
            long n = num.Absolute().ToInt64();
            long d = divisor.Absolute().ToInt64();

            long res = new long();
            res = estimate;
            

            //throw new Exception($"res={res}");
            return res;

            //throw new Exception($"n={n}/d={d} and n/d={n/d}, estimate={estimate}");


        }  // DivideEstimate
        */

        private IntVL GetTopNrDigits(IntVL val, int nrDigits)
        {
            val.Compact();
            if(nrDigits >= val.Length) { return new(val); }

            byte[] narr = new byte[nrDigits];
            for(int n = 0; n < nrDigits; n++)
            {
                narr[n] = val[val.Length - 1 - nrDigits + n];
            }


            return new IntVL(val.positive, narr);
        }



        public static DivisionResult DivisionVL(IntVL numerator, IntVL denominator)
        {
            DivisionResult result = new() { Quotient = new(0), Remainder = new(0) };

            bool sign = !( numerator.positive ^ denominator.positive );

            IntVL Numerator = new(numerator);
            if(!Numerator.positive) { Numerator.positive = true; }
            IntVL Denominator = new(denominator);
            if(!Denominator.positive) { Denominator.positive = true; }

            if (Denominator > Numerator)
            {
                result.Remainder = new(Numerator);
                return result;  // Floor which is zero quotient
            }

            int range = Numerator.Length - Denominator.Length;
            while (Numerator >= Denominator)
            {
                if(range < 0) { break; }
                IntVL shifted_denominator = Denominator.ShiftLeft(range); // x10 so same nr digits above/below divide line
                //throw new Exception($"r={range},N={Numerator},SD={shifted_denominator}");
                IntVL diff = Numerator - shifted_denominator;
                if (shifted_denominator <= Numerator)
                {
                    Numerator -= shifted_denominator;
                    result.Quotient += new IntVL(1).ShiftLeft(range);
                }
                else
                {
                    range--;
                }

                if(Numerator < Denominator)
                {
                    result.Remainder = diff;
                    break;
                }

            }



            result.Quotient.positive = sign;
            return result;
        }



        #endregion division
        //---------------------------------------------------------------------------------------------------------------------------------------


        public int CompareTo(object obj)
        {
            if (Equals(obj)) { return 0; }
            IntVL inval = obj as IntVL;
            if (positive != inval.positive)
            {
                if (positive) { return 1; }
                else { return -1; }
            }

            this.Compact();
            inval.Compact();

            if (Length == inval.Length)
            {
                for (int n = Length - 1; n >= 0; n--)
                {
                    if (digits[n] == inval.digits[n]) { continue; }
                    if (digits[n] > inval.digits[n])
                    {
                        if (positive) { return 1; }
                        else { return -1; }
                    }
                    else
                    {
                        if (positive) { return -1; }
                        else { return 1; }
                    }
                }
                throw new OverflowException("*Error: CompareTo overflow!");
            }

            if (Length > inval.Length)
            {
                if (positive) { return 1; }
                else { return -1; }
            }
            else
            {
                if (positive) { return -1; }
                else { return 1; }
            }


            //return 0;
        }


        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            IntVL input = obj as IntVL;

            if (positive != input.positive) { return false; }
            this.Compact();
            input.Compact();
            if (Length != input.Length) { return false; }
            
            for (int ndx = 0; ndx < Length; ndx++)
            {
                if (digits[ndx] != input.digits[ndx]) { return false; }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                if (!positive) { hash *= 23; }
                foreach (byte b in digits)
                {
                    hash = hash * 23 + b.GetHashCode();
                }
                return hash;
            }
        }

        
        #endregion Operators


        //--------------- Functions --------------------------------
        #region Functions


        public IntVL Absolute()
        {
            IntVL res = new (this)
            {
                positive = true
            };
            return res;
        }

        /// <summary>
        /// Returns the digital root
        /// </summary>
        /// <returns></returns>
        public int DigitalRoot()
        {
            if (digits.Length == 0) { return 0; }
            if (this.IsZero) { return 0; }

            IntVL modeREs = this.Modulus(new(9));

            if (modeREs.IsZero) { return 9; }

            return modeREs.ToInt32();


            //IntVL one = new(1);
            //IntVL two = this - one;

            //IntVL res = one + two.Modulus(new(9));

            //return res.ToInt32();

            //long digital_root = -1;
            //IntVL check = new(this);

            //while (digital_root < 0 || digital_root > 9)
            //{
            //    digital_root = 0;
            //    for (int i = 0; i < check.digits.Length; i++)
            //    {
            //        digital_root += digits[i];
            //    }
            //    if(digital_root < 10) { break; }
            //    check = new(digital_root);
            //}

            //return (int)digital_root;
        }



        #endregion Functions



        //--------------- Conversions --------------------------------
        #region Conversions


        static byte[] CopyArray(byte[] inArray)
        {
            byte[] res = new byte[inArray.Length];
            for (int n = 0; n < inArray.Length; n++)
            { res[n] = inArray[n]; }

            return res;
        }

        /// <summary>
        /// Shrink space used do minimum size.  Minumum size = 1 for values < 10
        /// </summary>
        public void Compact()
        {
            if (Length == 1) { return; }
            int mxLen = Length;

            for (int n = Length - 1; n > 0; n--)
            {
                if (IsNil(digits[n]))
                { mxLen--; }
                else
                { break; }
            }

            if (mxLen == Length) { return; }
            byte[] temp = CopyArray(digits);
            digits = new byte[mxLen];
            for (int k = 0; k < mxLen; k++)
            { digits[k] = temp[k]; }


        } // Compact


        public Int32 ToInt32() => (int)ToInt64();

        public Int64 ToInt64()
        {
            Compact();
            if (digits.Length > 19) { throw new ArithmeticException("*Error: Overflow converting to Int64"); }

            long res = 0;
            long fact = 1;
            for (int x = 0; x < digits.Length; x++)
            {
                res += digits[x] * fact;
                fact *= 10;
            }

            if (!positive) { res = -res; }
            return res;
        }


        public static IntVL Parse(string stringVal)
        {
            //char[] cDigits = "0123456789".ToCharArray();

            Dictionary<char, byte> charMap = new()
            {
                { '0', 0 },
                { '1', 1 },
                { '2', 2 },
                { '3', 3 },
                { '4', 4 },
                { '5', 5 },
                { '6', 6 },
                { '7', 7 },
                { '8', 8 },
                { '9', 9 },
            };

            if (string.IsNullOrEmpty(stringVal)) { return new IntVL(); }
            bool pos = true;
            if (stringVal.StartsWith("-"))
            {
                pos = false;
                stringVal = stringVal[1..]; //.Substring(1, stringVal.Length - 1);
            }

            byte[] nDigits = new byte[stringVal.Length];

            for (int i = 0; i < stringVal.Length; i++)
            {
                char ch = stringVal[stringVal.Length - 1 - i];
                if (!charMap.ContainsKey(ch)) { throw new ArgumentException("*Error: IntVL Parsing Error!"); }
                nDigits[i] = charMap[ch];
            }

            return new IntVL(pos, nDigits);
        } // Parse


        public override string ToString()
        {
            string res = "";
            this.Compact();

            for (int i = digits.Length - 1; i >= 0; i--)
            { res += digits[i].ToString(); }

            if (!positive) { res = "-" + res; }

            return res;
        }

        #endregion Conversions

    }

}
