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
            Primes = [2, 3, 5];//, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71 };
            //[2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97]
            FoundPrimes = [1, 2, 3];
        }

        public List<long> Primes { get; private set; }
        public List<long> FoundPrimes { get; private set; }

        long MaxPrime { get { return Primes[^1]; } }


        public void LoadPrimes(List<long> loadList)
        {
            Primes =[];
            foreach (long id in loadList)
            {
                if (id == 2 || id == 5) { Primes.Add(id); }
                else 
                {
                    if (id % 2 == 1 && id % 5 != 0) { Primes.Add(id); }
                }
            }
            Primes = [.. Primes.OrderBy(o => o)];
        }


        public bool IsPrime(int checkVal) => IsPrime((long)checkVal);
        public bool IsPrime(long checkVal)
        {

            if(checkVal <= 1) { return false; }
            if (checkVal == 2 || checkVal == 3 || checkVal == 5) { return true; }
            if (checkVal % 2 == 0 || checkVal % 3 == 0 || checkVal % 5 == 0) { return false; }
            
            if (FoundPrimes.Find(q => q == checkVal) != checkVal)
            {

                foreach (long val in Primes)
                {
                    //if (checkVal == 188489971511) { throw new Exception($"IsPrim: MAX={MaxPrime},val={val}"); }
                    if (val * val > checkVal) { FoundPrimes.Add(checkVal); return true; } // found!
                    if (checkVal % val == 0) { return false; }
                }

                if (checkVal > MaxPrime*MaxPrime)
                {
                    
                    for (long i = 5; i * i <= checkVal; i += 6)
                    {
                        if (i < MaxPrime) { continue; }
                        
                        if (i > MaxPrime && IsPrime(i)) { Primes.Add(i); FoundPrimes.Add(i);/* throw new Exception($"i={i}");*/ };
                        if ((i + 2) > MaxPrime &&  IsPrime(i + 2)) { Primes.Add(i + 2); FoundPrimes.Add(i + 2);/* throw new Exception($"i+2={i+2}");*/ };
                        if (checkVal % i == 0 || checkVal % (i + 2) == 0)
                        { return false; }
                    }
                }
                
                return true; // end of list so must be prime
            }
            return true;

        } // IsPrime


        public string PrimeListToView() => PrimeListToView(Primes.Count);

        public string PrimeListToView(int firstNrItems)
        {
            string pList = "[";
            for(int n = 0; n < Primes.Count && n < firstNrItems; n++)
            {
                pList += Primes[n].ToString() + ",";
            }
            pList = pList[..^1];
            return pList + "]";

        } // PrimeListToView

        public string PrimeListToViewDesc(int lastNrItems)
        {
            string pList = "[";
            for (int n = 0; n < Primes.Count + lastNrItems && n < lastNrItems; n++)
            {
                int idx = Primes.Count - lastNrItems + n;
                if(idx < 0) { continue; }
                pList += Primes[idx].ToString() + ",";
            }
            pList = pList[..^1];
            return pList + "]";

        } // PrimeListToView


        public List<PrimeTuple> GetPrimeFactors(long nrToEvaluate)
        {
            List<PrimeTuple> factors = [];


            if (IsPrime(nrToEvaluate)) { return factors; }

            long maxStart = MaxPrime;
            long nValue = nrToEvaluate;

            // first check all those in the existing list
            foreach (long val in Primes)
            {
                if (val * val > nrToEvaluate) { return factors; } // done!
                //factors = AddFactors(val, nValue, factors);
                while (nValue % val == 0)
                {
                    //if (nrToEvaluate == 188489971511) { throw new Exception($"val={val},NV={nValue}"); }
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
            }

            // check other nrs
            //nValue = nrToEvaluate;
            for (long i = 5; i*i < nrToEvaluate; i += 6)
            {
                if (i + 2 <= MaxPrime) { continue; }
                if (i * i > nValue) { break; }
                if (IsPrime(i))
                {
                    while (nValue % i == 0)
                    {
                        PrimeTuple tup = factors.Where(q => q.Prime == i).FirstOrDefault();
                        if (tup == null)
                        {
                            PrimeTuple nTup = new() { Prime = i, Count = 1 };
                            factors.Add(nTup);
                        }
                        else
                        {
                            tup.Count++;
                        }
                        nValue /= i;
                    }
                }

                if (IsPrime(i+2))
                {
                    
                    while (nValue % (i+2) == 0)
                    {
                        //if (i+2 > MaxPrime) { throw new Exception($"i+2={i + 2},nv={nValue},factors={factors.Count}");  }
                        
                        PrimeTuple tup = factors.Where(q => q.Prime == (i+2)).FirstOrDefault();
                        if (tup == null)
                        {
                            PrimeTuple nTup = new() { Prime = i+2, Count = 1 };
                            factors.Add(nTup);
                        }
                        else
                        {
                            tup.Count++;
                        }
                        nValue /= (i + 2);
                    }
                }

            }
            //throw new Exception($"NV={nValue},fCount={factors.Count}");
            if (IsPrime(nValue))
            {
                PrimeTuple nTup = new() { Prime = nValue, Count = 1 };
                factors.Add(nTup);
                //throw new Exception($"NV={nValue},fCount={factors.Count}");
            }

            return factors;

        } // GetPrimeFactors



        public List<PrimeTuple> GetPrimeFactors2(long nrToEvaluate)
        {
            List<PrimeTuple> factors = [];
            
            if (IsPrime(nrToEvaluate)) { return factors; }

            long maxStart = MaxPrime;
            long nValue = nrToEvaluate;

            // first check all those in the existing list
            foreach (long val in Primes)
            {
                if (val * val > nrToEvaluate) { return factors; } // done!
                //factors = AddFactors(val, nValue, factors);
                while (nValue % val == 0)
                {
                    PrimeTuple tup = factors.Where(q => q.Prime == val).FirstOrDefault();
                    if (tup == null)
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
            }

            // check other nrs
            //nValue = nrToEvaluate;
            for (long i = 5; i * i < nrToEvaluate; i++)
            {
                if (i <= MaxPrime) { continue; }
                if (i % 2 == 0 || i % 3 == 0 || i % 5 == 0) { continue; } 

                if (IsPrime(i))
                {

                    while (nValue % i == 0)
                    {
                        //if(i >  7 ) { throw new Exception($"i={i},nv={nValue}"); }

                        PrimeTuple tup = factors.Where(q => q.Prime == i).FirstOrDefault();
                        if (tup == null)
                        {
                            PrimeTuple nTup = new() { Prime = i, Count = 1 };
                            factors.Add(nTup);
                        }
                        else
                        {
                            tup.Count++;
                        }
                        nValue /= i;
                    }
                }

            }

            if (IsPrime(nValue))
            {
                PrimeTuple nTup = new() { Prime = nValue, Count = 1 };
                factors.Add(nTup);
            }

            return factors;

        } // GetPrimeFactors


        public string PrimeFactorsToView(List<PrimeTuple> factors)
        {
            string view = "";
            if (factors.Count == 0) { return view; }

            foreach(var fact in factors)
            {
                view += $"{fact.Prime}^{fact.Count} ";
            }

            return view.Replace("^1", "");
        }

    }
}
