using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;
using System.Collections;

namespace Algorytm_RSA
{
    class RSA
    {
        public List<int> primeNumbers;
        public BigInteger N;
        public BigInteger PHI;
        public BigInteger E;
        public BigInteger D;

        public RSA(int n)
        {
            this.primeNumbers = PrimeNumbers.GeneratePrimesNaive(n);
        }

        public void GenerateKey(BigInteger p, BigInteger q)
        {
            N = p * q;
            PHI = (p - 1) * (q - 1);

            foreach (BigInteger primeNumber in primeNumbers)
            {
                if (NWD(primeNumber, PHI) == 1)
                {
                    E = primeNumber;
                    break;
                }
            }
            if(E == 0)  MessageBox.Show("e = 0 ");    
            
            for (int d = 2; d < int.MaxValue; d++)
            {
                if ((d * E) % PHI == 1)
                {
                    D = d;
                    break;
                }
            }
        }

        public BigInteger NWD(BigInteger a,BigInteger b)
        {
            while (a != b)
            {
                if (a > b)
                    a -= b;
                else
                    b -= a;
            }
            return a;
        }
    }
}
