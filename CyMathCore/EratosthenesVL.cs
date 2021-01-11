using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyMathCore
{
    /// <summary>
    /// Find prime numbers and keep a list of ones already found in format InvVL
    /// </summary>
    public class EratosthenesVL : IEnumerable<IntVL>
    {
        private IntVL _lastChecked;

        public EratosthenesVL()
        {
            PrimeList = new List<IntVL>
            {
                new IntVL(2),
                new IntVL(3),
                new IntVL(5),
                new IntVL(7),
                new IntVL(11),
            };
            _lastChecked = PrimeList[^1];
        }

        public List<IntVL> PrimeList { get; private set; }

        public void LoadPrimeList(List<IntVL> primeList)
        {
            PrimeList = new List<IntVL>();
            foreach (var vl in primeList) { PrimeList.Add(vl); }
            _lastChecked = PrimeList[^1];
        }

        public void LoadPrimeList(List<long> primeList)
        {
            PrimeList = new List<IntVL>();
            foreach (var vl in primeList) { PrimeList.Add(vl); }
            _lastChecked = PrimeList[^1];
        }

        /// <summary>
        /// Check if a number is prime
        /// </summary>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        public bool IsPrime(IntVL checkValue)
        {
            if (checkValue < 2) { return false; }

            if (checkValue == 2) { return true; }

            foreach (IntVL prime in this)
            {
                if ((prime * prime) > checkValue) { break; }

                if ((checkValue % prime) == 0) { return false; }
            }

            return true;
        } // IsPrime


        /// <summary>
        /// Foreach interface
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IntVL> GetEnumerator()
        {
            foreach (IntVL prime in PrimeList)
            {
                yield return prime;
            }

            while (_lastChecked < long.MaxValue)
            {
                _lastChecked++;

                if (IsPrime(_lastChecked))
                {
                    PrimeList.Add(_lastChecked);
                    yield return _lastChecked;
                }
            }

        } // GetEnumerator


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();

        } // GetEnumerator


    }
}
