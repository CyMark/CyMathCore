using System;


namespace CyMathCore
{
    /// <summary>
    /// Class implements Douglas Crockford's DEC64 concepts
    /// http://dec64.com/
    /// For double this does not work:  0.2 == 0.3 - 0.1.  This number set resolves these anomalies
    /// </summary>
    public class Number : IComparable
    {
        const string errorArithmatic = "*Error: Not a Number! Undefined or not initialised!";
        const string errorOverflow = "*Error: Arithmetic overflow! Nr larger or smaller than 56 bits!";
        const string errorExponentOverflow = "*Error: Exponent > 127 or < -127!";
        const string errorOperation = "*Error: Incorrect operation detected!";

        const long MinValue = -36028797018963968;
        const long MaxValue = 36028797018963967;

        const long MinValueDivTen = -3602879701896396;
        const long MaxValueDivTen = 3602879701896396;

        sbyte exponent;
        long significand;
        //long dec64;

        /// <summary>
        /// Always initialise to Nan or undefined!
        /// </summary>
        public Number()
        {
            //dec64 = -128;
            //MakeNumber();
            exponent = -128;
            significand = 1;
        }

        public Number(long coeff, sbyte exp)
        {
            significand = coeff;
            exponent = exp;
            CheckOverflow();
        }

        public Number(int value)
        {
            exponent = 0;
            significand = value;
            //dec64 = value;
            //MakeNumber();
        }

        public Number(long value)
        {

            exponent = 0;
            significand = value;
            //if(value > 2147483647) throw new Exception("*Debug: c=" + coefficient.ToString() + ", e=" + exponent + ", v=" + value.ToString());
            // Check precision
            if (significand >= MaxValue)
            {
                while (significand > MaxValue)
                {
                    significand /= 10;
                    exponent++;
                    //throw new Exception("*Debug: c=" + coefficient.ToString() + ", e=" + exponent);
                }
                return;
            }
            if (significand <= MinValue)
            {
                while (significand < MinValue)
                {
                    significand /= 10;
                    exponent++;
                }
            }
        }

        public Number(double value)
        {
            Number cnum = Number.Parse(value.ToString());
            significand = cnum.significand;
            exponent = cnum.exponent;
            return;
        }  // ctor


        public Number(decimal Value)
        {

            Number cnum = Number.Parse(Value.ToString());
            significand = cnum.significand;
            exponent = cnum.exponent;
            return;

        }


        public Number(Number nrToCopy)
        {
            significand = nrToCopy.significand;
            exponent = nrToCopy.exponent;
            CheckOverflow();
        }

        public static Number Parse(string txt)
        {
            if (string.IsNullOrEmpty(txt)) { throw new FormatException("*Error: Cannot parse empty string!"); }
            string mantissa = txt.Trim().ToUpper().Replace("+", "");
            string fraction = null;

            long coeff = 0;
            int expInt = 0;
            sbyte exp = 0;
            bool isNeg = false;

            if (mantissa.Contains("E"))
            {
                string[] parts = mantissa.Split("E".ToCharArray());
                mantissa = parts[0];
                if (parts.Length > 2) { throw new FormatException($"*Error: Cannot parse '{txt}' to Number!"); }
                if (parts.Length == 2)
                {
                    expInt = Int16.Parse(parts[1]);
                    //if(expInt > 127 || expInt < -127) { throw new ArithmeticException("*Error: Number to big! Exponent must be less than Abs(127)"); }
                    //exp = (sbyte)expInt;
                }
            }

            // has to be after checking for E
            if (mantissa.StartsWith("-"))
            {
                isNeg = true;
                mantissa = mantissa.Replace("-", "");
            }

            if (mantissa.Contains("."))
            {
                string[] parts = mantissa.Split(".".ToCharArray());
                mantissa = parts[0];
                if (parts.Length > 2) { throw new FormatException($"*Error: Cannot parse '{txt}' to Number!"); }
                if (parts.Length == 2) { fraction = parts[1]; }
            }

            // Check for leading zeroes on mantissa
            while (mantissa[0] == '0' && mantissa.Length > 1)
            {
                mantissa = mantissa.Substring(1, mantissa.Length - 1);
            }

            if (!string.IsNullOrEmpty(fraction))
            {
                if (mantissa.Length + fraction.Length > 15)
                {
                    if (mantissa.Length >= 15)
                    {
                        int len = mantissa.Length - 15;
                        //exp += (sbyte)len; // + exp;
                        expInt += len;
                        mantissa = mantissa.Substring(0, 15);
                    }
                    else
                    {
                        int len = mantissa.Length + fraction.Length - 15;
                        //exp -= (sbyte)(fraction.Length - len);
                        expInt -= fraction.Length - len;
                        mantissa += fraction.Substring(0, fraction.Length - len);
                    }
                }
                else
                {
                    int len = fraction.Length;
                    //exp -= (sbyte)(len);
                    expInt -= len;
                    mantissa += fraction.Substring(0, len);
                }
            }

            coeff = Int64.Parse(mantissa);

            if (coeff >= MaxValue)
            {
                while (coeff > MaxValue)
                {
                    coeff /= 10;
                    expInt++;
                }
            }

            if (expInt > 127 || expInt < -127) { throw new OverflowException(errorExponentOverflow); }
            else { exp = (sbyte)expInt; }

            return new Number(isNeg ? -coeff : coeff, exp);
        } // Parse


        /*
        private void MakeNumber()
        {
            coefficient = dec64 >> 8;
            exponent = (sbyte)dec64;
        }
        */

        public long Significand { get { return significand; } }
        public long Exponent { get { return exponent; } }


        /// <summary>
        /// Check if this is a valid number:  If Not a Number (NaN) true then not valid
        /// </summary>
        public bool NaN { get { return exponent == -128; } }


        /// <summary>
        /// Checks if the number is zero for any value of the coefficient, including -128
        /// </summary>
        public bool IsZero { get { return significand == 0; } }


        public void MultTen()
        {
            if (NaN) throw new ArithmeticException(errorArithmatic);
            if (exponent == 127)
            {
                significand *= 10;
                CheckOverflow();
            }
            else
                exponent++;
        } // MultTen


        public void DivTen()
        {
            if (NaN)
            {
                throw new ArithmeticException(errorArithmatic);
            }

            if (exponent == -127)
            {
                significand /= 10;
                CheckOverflow();
            }
            else
                exponent--;
        } // DivTen


        private void CheckOverflow()
        {
            if (significand > MaxValue || significand < MinValue) { throw new OverflowException(); }
        }

        private void CheckNaN()
        {
            if (this.NaN) throw new InvalidOperationException(errorArithmatic);
        }

        private void CheckExponent(int Exponent)
        {
            if (Exponent > 127 || Exponent < -127) { throw new OverflowException(errorExponentOverflow); ; }
        }


        /// <summary>
        /// Adjust the exponent to a value by dividing or multiplying the coefficient accordingly without changing the intrinsic value.
        /// If exponent is increased then division takes place. If the coefficient was not a multiple of 10, precision might be lost!
        /// Does not change number internal state!  Coefficient and Exponent unaffected.
        /// </summary>
        /// <param name="value"></param>
        public Number AdjustExponent(sbyte value)
        {
            Number res = new Number(this);
            if (NaN) throw new ArithmeticException(errorArithmatic);
            if (IsZero)
            {
                return res;
            }

            if (res.exponent == value)
            {
                return res;
            }

            long prevCoeff = res.significand;
            if (res.exponent < value)
            {
                while (res.exponent < value)
                {
                    res.significand /= 10;
                    if (res.significand == 0)
                    {
                        // stop when hitting limit
                        res.significand = prevCoeff;
                        return res;
                    }
                    res.exponent++;
                    prevCoeff = res.significand;
                }
                return res;
            }

            while (res.exponent > value)
            {
                res.significand *= 10;
                if (res.significand > MaxValue || res.significand < MinValue)
                {
                    // stop when hitting limit
                    res.significand = prevCoeff;
                    return res;
                }
                res.exponent--;
                prevCoeff = res.significand;
            }
            return res;
        } // AdjustExponent


        /// <summary>
        /// Shift the coefficient right by dividing by 10 and increasing the exponent. If least significant not zero, presicion lost
        /// Imutable.
        /// </summary>
        /// <returns></returns>
        public Number ShiftCoefficientRight()
        {
            CheckNaN();
            CheckOverflow();
            if (IsZero) return new Number(0);
            return new Number(significand / 10, ++exponent);

        } // ShiftDecimalLeft



        /// <summary>
        /// Shift the coefficient left by multiplying by 10 and decreasing the exponent. Imutable.
        /// </summary>
        /// <returns></returns>
        public Number ShiftCoefficientLeft()
        {
            CheckNaN();
            CheckOverflow();
            if (IsZero) return new Number(0);
            return new Number(significand * 10, --exponent);

        } // ShiftDecimalRight


        /// <summary>
        /// Adjust number so the coefficient does not end with a zero.
        /// Does not change internal state of coefficient and exponent
        /// </summary>
        public Number FlushZeroes()
        {
            Number res = new Number(this);
            if (res.significand == 0) { return res; }
            int count = 0;
            while (res.significand % 10 == 0)
            {
                res.significand /= 10;
                res.exponent++;
                if (count++ > 20) throw new InvalidOperationException(errorOperation);
            }
            return res;
        } // FlushZeroes


        /// <summary>
        /// Move the coefficient to the left by Shifting Left to a maximum value
        /// </summary>
        /// <returns></returns>
        public Number FlushLeft()
        {
            Number res = new Number(this);

            while (res.significand < MaxValueDivTen && res.significand > MinValueDivTen)
            {
                res.significand *= 10;
                res.exponent--;
            }

            return res;
        }



        //--------- operators  ----------------------
        #region operators

        // explicit Number to long conversion operator
        //public static explicit operator long(Number d)  => d.ToInt64();




        public static Number operator +(Number left, Number right)
        {
            if (left.NaN || right.NaN) throw new InvalidOperationException(errorArithmatic);
            Number lh = new Number(left);
            Number rh = new Number(right);
            if (lh.IsZero) { return rh; }
            if (rh.IsZero) { return lh; }

            if (lh.exponent != rh.exponent)
            {
                if (lh.exponent < rh.exponent)
                {
                    rh = rh.AdjustExponent(lh.exponent);
                    // check if exponent adjusted all the way:
                    if (rh.exponent != lh.exponent)
                    {
                        lh = lh.AdjustExponent(rh.exponent);  // Adjust the other side if a limit was reach
                        if (rh.exponent != lh.exponent)
                        {
                            // numbers are not compatable
                            return rh;
                            throw new OverflowException(errorOverflow);
                        }
                    }
                }
                else
                {
                    lh = lh.AdjustExponent(rh.exponent);
                    // check if exponent adjusted all the way:
                    if (lh.exponent != rh.exponent)
                    {
                        rh = rh.AdjustExponent(lh.exponent);
                        if (rh.exponent != lh.exponent)
                        {
                            // numbers are not compatable
                            return lh;
                            throw new OverflowException(errorOverflow);
                        }
                    }
                }
            }

            lh = new Number(lh.significand + rh.significand, lh.exponent);

            return lh;

        } // operator +


        public static Number operator -(Number left, Number right)
        {
            return left + new Number(-right.significand, right.exponent);
            /*
            if (left.NaN || right.NaN) throw new InvalidOperationException(errorArithmatic);
            Number lh = new Number(left);
            Number rh = new Number(right);
            if (lh.exponent != rh.exponent)
            {
                if (lh.exponent < rh.exponent)
                {
                    rh = rh.AdjustExponent(lh.exponent);
                }
                else
                {
                    lh = lh.AdjustExponent(rh.exponent);
                }
            }

            lh = new Number(lh.coefficient - rh.coefficient, lh.exponent);

            return lh;
            */
        } // operator -


        public static Number operator *(Number left, Number right)
        {
            left.CheckNaN();
            right.CheckNaN();
            //if (left.NaN || right.NaN) return new Number();
            if (left.IsZero || right.IsZero) return new Number(0);

            // Remove all trailing zeroes on the 
            Number lh = left.FlushZeroes(); // Make coefficient as small as possible
            Number rh = right.FlushZeroes();

            int exptot = (int)lh.exponent + (int)rh.exponent;
            if (exptot > 127 || exptot < -127) { throw new OverflowException(errorExponentOverflow); }

            bool LHisNeg = false;
            if (lh < 0)
            {
                LHisNeg = true;
            }

            bool RHisNeg = false;
            if (rh < 0)
            {
                RHisNeg = true;
            }

            lh = lh.Absolute();
            rh = rh.Absolute();

            // alternate in truncating the digits to the right until overflow is prevented
            bool CheckLH = true;
            while (lh.significand != 0 && rh.significand != 0)
            {
                if (CheckLH)
                {
                    if ((Int64.MaxValue / lh.significand) < rh.significand) { rh = rh.ShiftCoefficientRight(); }
                    else { break; }
                }
                else
                {
                    if ((Int64.MaxValue / rh.significand) < lh.significand) { lh = lh.ShiftCoefficientRight(); }
                    else { break; }
                }
                CheckLH = !CheckLH;
            }
            exptot = (int)lh.exponent + (int)rh.exponent; // recalc here after all the shifting

            long coeff = LHisNeg ? -lh.significand : lh.significand;

            try { coeff *= RHisNeg ? -rh.significand : rh.significand; }
            catch { throw new Exception("DEBUG: value too large!"); }

            // trim nr until coeff < 56 bits
            while (coeff > MaxValue || coeff < MinValue)
            {
                coeff /= 10;
                exptot++;
            }

            Number nNr = new Number(coeff, (sbyte)exptot);
            /*
            if(coeff > MaxValue || coeff < MinValue)
            {
                // todo adjust exp to fit coeff into 56 bits
            }
            */
            return nNr;

        } // operator *


        public static Number operator /(Number left, Number right)
        {
            left.CheckNaN();
            right.CheckNaN();
            if (right.IsZero && left.IsZero) { throw new InvalidOperationException(errorOperation); }
            if (right.IsZero) { throw new ArithmeticException("*Error: Divide by zero!"); }
            if (left.IsZero) { return new Number(0); }

            Number rh = new Number(right).FlushZeroes();  // Denominator coefficient as small as possible without
            Number lh = new Number(left).FlushLeft();   // Numerator coefficient as large as possible

            int exptot = lh.exponent - rh.exponent;
            if (exptot > 127 || exptot < -127) { throw new OverflowException(errorExponentOverflow); }

            return new Number(lh.significand / rh.significand, (sbyte)exptot);
        }

        public static bool operator ==(Number left, Number right) => left.Equals(right);

        public static bool operator !=(Number left, Number right) => !left.Equals(right);

        public static bool operator >(Number left, Number right) => left.CompareTo(right) > 0;

        public static bool operator <(Number left, Number right) => left.CompareTo(right) < 0;

        public static bool operator >=(Number left, Number right) => left.CompareTo(right) >= 0;

        public static bool operator <=(Number left, Number right) => left.CompareTo(right) <= 0;



        public static bool operator ==(Number left, decimal right) => left.ToDecimal() == right;

        public static bool operator !=(Number left, decimal right) => left.ToDecimal() != right;

        public static bool operator >(Number left, decimal right) => left.ToDecimal() > right;

        public static bool operator <(Number left, decimal right) => left.ToDecimal() < right;

        public static bool operator >=(Number left, decimal right) => left.ToDecimal() >= right;

        public static bool operator <=(Number left, decimal right) => left.ToDecimal() <= right;


        public static bool operator ==(Number left, int right) => left.ToInt32() == right;

        public static bool operator !=(Number left, int right) => left.ToInt32() != right;

        public static bool operator >(Number left, int right) => left.ToInt32() > right;

        public static bool operator <(Number left, int right) => left.ToInt32() < right;

        public static bool operator >=(Number left, int right) => left.ToInt32() >= right;

        public static bool operator <=(Number left, int right) => left.ToInt32() <= right;


        public static bool operator ==(Number left, long right) => left.ToInt64() == right;

        public static bool operator !=(Number left, long right) => left.ToInt64() != right;

        public static bool operator >(Number left, long right) => left.ToInt64() > right;

        public static bool operator <(Number left, long right) => left.ToInt64() < right;

        public static bool operator >=(Number left, long right) => left.ToInt64() >= right;

        public static bool operator <=(Number left, long right) => left.ToInt64() <= right;


        public static implicit operator Number(long d) => new Number(d);  // implicit conversion
        public static implicit operator Number(double value) => new Number(value);
        public static implicit operator Number(int value) => new Number(value);
        public static implicit operator Number(decimal value) => new Number(value);


        //public static explicit operator Number(decimal val) => new Number(val);

        public static explicit operator decimal(Number val) => val.ToDecimal();

        //public static explicit operator Number(double val) => new Number(val);

        public static explicit operator double(Number val) => val.ToDouble();

        //public static explicit operator Number(int val) => new Number(val);

        public static explicit operator int(Number val) => val.ToInt32();

        // public static explicit operator Number(long val) => new Number(val);

        public static explicit operator long(Number val) => val.ToInt64();

        #endregion operators

        //--------- Built in arithmatic operators  ----------------------
        #region Built in arithmatic operators



        /// <summary>
        /// Returns Absolute value of Number.  Immutable.
        /// </summary>
        /// <returns></returns>
        public Number Absolute()
        {
            CheckOverflow();
            CheckNaN();
            //long coeff = coefficient < 0 ? -coefficient : coefficient;
            return new Number(significand < 0 ? -significand : significand, exponent);
        } // Asbsolute

        public Number Abs() => Absolute();

        /// <summary>
        /// Returns PI, regardless of the value of this nr, but does not change this value;
        /// </summary>
        /// <returns></returns>
        public static Number Pi() => new Number(Math.PI);


        /// <summary>
        /// Returns this nr Multiplied by Pi.  Immutable.
        /// </summary>
        /// <returns></returns>
        public Number PiMult() => Number.Pi() * this;

        public Number Divide(Number denominator) => this / denominator;

        public Number Multiply(Number number) => this * number;


        public Number Power(int Exponent)
        {
            if (Exponent == 0) { return new Number(1); }
            bool isNeg = Exponent < 0;
            int expAbs = (isNeg ? -Exponent : Exponent);
            int expInt = expAbs * exponent;

            long coeff = significand;
            for (int n = 1; n < expAbs; n++)
            {
                coeff *= significand;
            }

            CheckExponent(expInt);

            if (isNeg) { return new Number(1) / new Number(coeff, (sbyte)expInt); }

            return new Number(coeff, (sbyte)expInt);
        } // Power


        public static Number Factorial(int n) => new Number(Faculty(n));

        public static long Faculty(int n)
        {
            if (n < 0) { throw new OverflowException("*Error: Cannot calculate faculty for negative values"); }
            if (n <= 1) return 1;
            long res = n;
            for (int k = n - 1; k > 0; k--)
            {
                res *= k;
            }
            return res;
        }


        /// <summary>
        /// Exp as in e^x.  Inverse of Ln(x).  Returns e^{this number}. Immutable.
        /// </summary>
        /// <returns></returns>
        public Number Euler()
        {
            Number res = 1 + new Number(this);
            //res = new Number(0) + (this.Power(2) / Number.Faculty(2));
            //return res + (this.Power(2) / Number.Faculty(2));,

            for (int n = 2; n < 24; n++) //25
            {
                Number div = Number.Faculty(n);
                if (div.IsZero) { break; }
                Number temp = this.Power(n) / div;
                if (temp.IsZero) { break; }
                res += temp;
            }
            return res;
        }

        /// <summary>
        /// Exp as in e^x.  Inverse of Ln(x).  Returns e^{this number}. Immutable.
        /// </summary>
        /// <returns></returns>
        public Number Exp() => Euler();


        public Number Ln()
        {
            //object[] nrs = { 0b1, 0b10, "text", new object[] { 0b100, 0b10000, 0b100000, 0b11_1001_0100_0100_0111 }, 55 };

            //(int sum, int count) = Tally(nrs);

            //string x = $"Sum={sum} and Count={count}";

            if (this <= 0) { throw new ArithmeticException("*Error: Natural logarithms for X <= 0 is not valid!"); }
            if (this == 1) { return new Number(0); }

            //return this.Exp();

            Number tmp = this - 1;
            Number res = tmp;

            //return res  + (tmp.Power(2)/ new Number(-2)); // + (tmp.Power(3)/ new Number(3)));

            for (int n = 2; n < 100000; n++) //25
            {
                Number div = n;
                if (n % 2 == 0) { div = -n; }
                Number temp = tmp.Power(n) / div;
                if (temp.IsZero) { break; }
                res = res + temp;
            }
            return res;
        }
        

        #endregion Built in arithmatic operators

        //--------- conversions  ----------------------
        #region conversions

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            Number tmp = obj as Number;
            if (tmp.NaN) throw new InvalidOperationException(errorArithmatic);

            if (tmp.significand == significand && tmp.exponent == exponent) return true;
            Number lh = new Number(this);
            Number rh = new Number(tmp);
            if (lh.exponent != rh.exponent)
            {
                if (lh.exponent < rh.exponent)
                {
                    rh = rh.AdjustExponent(lh.exponent);
                }
                else
                {
                    lh = lh.AdjustExponent(rh.exponent);
                }
            }

            return (lh.significand == rh.significand && lh.exponent == rh.exponent);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + significand.GetHashCode();
                hash = hash * 23 + exponent.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            Number tmp = obj as Number;
            if (tmp.NaN) throw new InvalidOperationException(errorArithmatic);

            Number lh = new Number(this);
            Number rh = new Number(tmp);
            if (lh.exponent != rh.exponent)
            {
                if (lh.exponent < rh.exponent)
                {
                    rh = rh.AdjustExponent(lh.exponent);
                }
                else
                {
                    lh = lh.AdjustExponent(rh.exponent);
                }
            }

            return lh.significand.CompareTo(rh.significand);
        }

        public long ToInt64()
        {
            if (IsZero) return 0;
            if (NaN) throw new ArithmeticException(errorArithmatic);

            Number res = AdjustExponent(0);

            if (res.exponent == 0) { return res.significand; }
            else return 0;
        }


        public int ToInt32()
        {
            if (IsZero) return 0;
            if (NaN) throw new ArithmeticException(errorArithmatic);

            return (int)ToInt64();
        }


        public double ToDouble()
        {
            if (IsZero) return 0;
            if (NaN) throw new ArithmeticException(errorArithmatic);

            double result = significand;
            if (exponent == 0) return result;
            if (exponent > 0)
            {
                for (int x = 0; x < exponent; x++)
                { result *= 10; }
                return result;
            }
            if (exponent > -16)
            {
                long mult = 1;
                for (int x = 0; x > exponent; x--)
                { mult *= 10; } // quicker to multiply
                //if (mult >= 10000000000) throw new Exception("*Debug: ToDouble() c=" + coefficient + ", e=" + exponent + ", m=" + mult);
                return result / mult;
            }
            else
            {
                //if (exponent <= -16) throw new Exception("*Debug: ToDouble() c=" + coefficient + ", e=" + exponent);
                for (int x = 0; x > exponent; x--)
                { result /= 10; }
            }
            //if (mult >= 10000000000) throw new Exception("*Debug: ToDouble() c=" + coefficient + ", e=" + exponent + ", m=" + mult);
            return result;

        } // ToDouble


        public decimal ToDecimal()
        {
            if (IsZero) return 0;
            if (NaN) throw new ArithmeticException(errorArithmatic);

            decimal result = significand;
            if (exponent == 0) { return result; }
            if (exponent > 0)
            {
                for (int x = 0; x < exponent; x++)
                { result *= 10; }
                return result;
            }
            if (exponent > -16)
            {
                long mult = 1;
                for (int x = 0; x > exponent; x--)
                { mult *= 10; } // quicker to multiply
                return result / mult;
            }
            else
            {
                //if (exponent <= -16) throw new Exception("*Debug: ToDouble() c=" + coefficient + ", e=" + exponent);
                for (int x = 0; x > exponent; x--)
                { result /= 10; }
            }
            return result;

        } // ToDecimal


        public override string ToString()
        {
            if (NaN) return "NaN";
            if (IsZero) return "0";
            if (exponent == 0) return significand.ToString();
            return significand.ToString() + "e" + exponent.ToString();
        }

        #endregion conversions

    } // class number
}
