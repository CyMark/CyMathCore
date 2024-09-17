using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{
    public class ComplexNumber : IEquatable<ComplexNumber>
    {

        public ComplexNumber()
        {
            Real = 0;
            Imaginary = 0;
        }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public ComplexNumber(PolarProperty polar)
        {
            Real = polar.Radius * Math.Cos(polar.Angle);
            Imaginary = polar.Radius * Math.Sin(polar.Angle);
        }


        public ComplexNumber(ComplexNumber copy)
        {
            Real = copy.Real;
            Imaginary = copy.Imaginary;
        }

        public double Real { get; set; }
        public double Imaginary { get; set; }


        public  bool IsZero => Real == 0 && Imaginary == 0;


        public ComplexNumber Conjugate() => new(Real, -1 * Imaginary);


        //--------------------------------------------------------
        // operators
        //--------------------------------------------------------
        #region operators

        public static bool operator ==(ComplexNumber left, ComplexNumber right) => left.Equals(right);
        public static bool operator !=(ComplexNumber left, ComplexNumber right) => !left.Equals(right);


        public static ComplexNumber operator +(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Real + right.Real, left.Imaginary + right.Imaginary);  
        }


        public static ComplexNumber operator -(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Real - right.Real, left.Imaginary - right.Imaginary);
        }

        public static ComplexNumber operator *(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Real * right.Real - left.Imaginary * right.Imaginary , left.Real * right.Imaginary + left.Imaginary * right.Real);
        }

        public static ComplexNumber operator *(double left, ComplexNumber right)
        {
            return new ComplexNumber(left * right.Real, left * right.Imaginary);
        }

        public static ComplexNumber operator /(ComplexNumber left, ComplexNumber right)
        {
            if (right.IsZero) { throw new DivideByZeroException(); }

            ComplexNumber rightConjugate = right.Conjugate();

            ComplexNumber denominator = right*rightConjugate; 

            ComplexNumber numerator = left*rightConjugate;

            return numerator / denominator.Real;
        }

        public static ComplexNumber operator /(ComplexNumber left, double right)
        {
            if(right == 0) { throw new DivideByZeroException($"Attempting to divide {left} by zero!"); }
            return 1/right * new ComplexNumber(left);
        }


        #endregion operators



        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            ComplexNumber tmp = obj as ComplexNumber;

            return tmp.Real == Real && tmp.Imaginary == Imaginary;
        }


        public PolarProperty ToPolar()
        {
            return new PolarProperty().CalculatePolar(Real, Imaginary);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Real, Imaginary);
        }

        public override string ToString()
        {
            string res = $"{Real}";
            if (Imaginary < 0) { res += $" {Imaginary}i"; }
            else { res += $" +{Imaginary}i"; }

            return res;
        }

        public bool Equals(ComplexNumber other)
        {
            return this.Real == other.Real && this.Imaginary <= other.Imaginary;
        }
    }
}
