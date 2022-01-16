using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyMathCore
{
    public class PrimeCalc
    {
        public PrimeCalc()
        {
            Primes = new() { 2, 3, 5, 7, 11, 13, 19 };
        }

        public List<long> Primes { get; private set; }

        long MaxPrime { get { return Primes[^1]; } }


        public void LoadPrimes(List<long> loadList)
        {
            Primes = new();
            foreach (long id in loadList)
            {
                if (id == 2 || id == 5) { Primes.Add(id); }
                else 
                {
                    if (id % 2 == 1 && id % 5 != 0) { Primes.Add(id); }
                }
            }
            Primes = Primes.OrderBy(o => o).ToList();
        }


        public bool IsPrime(int checkVal) => IsPrime((long)checkVal);
        public bool IsPrime(long checkVal)
        {
            if(checkVal <= 1) { return false; }
            if(Primes.Find(q => q == checkVal) != checkVal)
            {
                // first check existing list
                /*foreach (long val in Primes)
                {
                    if (checkVal % val == 0) { return false; } //checkval divisble by a smaller prime
                }
                int startIndex = Primes.Count;
                */
                int maxCheck = (int)Math.Sqrt(checkVal);

                if(maxCheck > MaxPrime)
                {
                    // add more primes
                    for(long p = MaxPrime + 2; p <= maxCheck; p += 2)
                    {
                        if(p % 5 != 0)
                        {
                            if (IsPrime(p)) { Primes.Add(p); }
                        }
                    }
                }

                foreach(long val in Primes)
                {
                    if(val > maxCheck) { return true; } // found!
                    if (checkVal % val == 0) { return false; } 
                }

                return true; // end of list
            }
            return true;

        } // IsPrime

        public string PrimeListToView() => PrimeListToView(Primes.Count);

        public string PrimeListToView(int firstNrItems)
        {
            string plist = "[";
            for(int n = 0; n < Primes.Count && n < firstNrItems; n++)
            {
                plist += Primes[n].ToString() + ",";
            }

            return plist + "]";

        } // PrimeListToView

        public string PrimeListToViewDesc(int lastNrItems)
        {
            string plist = "[";
            for (int n = 0; n < Primes.Count + lastNrItems && n < lastNrItems; n++)
            {
                int idx = Primes.Count - lastNrItems + n;
                if(idx < 0) { continue; }
                plist += Primes[idx].ToString() + ",";
            }

            return plist + "]";

        } // PrimeListToView


        public List<PrimeTuple> GetPrimeFactors(long nrToEvaluate)
        {
            List<PrimeTuple> factors = new();

            if(IsPrime(nrToEvaluate)) { return factors; }

            int maxCheck = (int)Math.Sqrt(nrToEvaluate);

            long nValue = nrToEvaluate;

            foreach (long val in Primes)
            {
                if (val > maxCheck) { return factors; } // done!
                while (nValue % val == 0)
                {
                    PrimeTuple tup = factors.Where(q => q.Prime == val).FirstOrDefault();
                    if(tup == null)
                    {
                        PrimeTuple nTup = new() { Prime = val, Count = 1 };
                        factors.Add(nTup);
                    }
                    else
                    {
                        tup.Count++;
                    }
                    nValue /= val;
                }
                if(IsPrime(nValue))
                {
                    PrimeTuple nTup = new() { Prime = nValue, Count = 1 };
                    factors.Add(nTup);
                    break;
                }
            }

            return factors;

        } // GetPrimeFactors


        public string PrimeFactorsToView(List<PrimeTuple> factors)
        {
            string view = "";
            if (factors.Count() == 0) { return view; }

            foreach(var fact in factors)
            {
                view += $"{fact.Prime}^{fact.Count} ";
            }

            return view.Replace("^1", "");
        }

    }
}
