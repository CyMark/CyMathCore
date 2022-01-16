using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{
    public class ComplexNumber
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

        public ComplexNumber(ComplexNumber copy)
        {
            Real = copy.Real;
            Imaginary = copy.Imaginary;
        }

        public double Real { get; set; }
        public double Imaginary { get; set; }


        public  bool IsZero => Real == 0 && Imaginary == 0;


        //--------------------------------------------------------
        // operators
        //--------------------------------------------------------
        #region operators

        public static bool operator ==(ComplexNumber left, ComplexNumber right) => left.Equals(right);
        public static bool operator !=(ComplexNumber left, ComplexNumber right) => !left.Equals(right);


        public static ComplexNumber operator+(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Real + right.Real, left.Imaginary + right.Imaginary);  
        }


        public static ComplexNumber operator -(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Real - right.Real, left.Imaginary - right.Imaginary);
        }

        public static ComplexNumber operator *(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Real * right.Real - left.Imaginary * left.Imaginary , left.Real*right.Imaginary + left.Imaginary * right.Real);
        }


        #endregion operators



        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            ComplexNumber tmp = obj as ComplexNumber;

            return tmp.Real == Real && tmp.Imaginary == Imaginary;

        }


        public PolarPoperty ToPolar()
        {
            return new PolarPoperty().CalculatePolar(Real, Imaginary);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Real.GetHashCode();
                hash = hash * 23 + Imaginary.GetHashCode();
                return hash;
            }
        }


    }
}
