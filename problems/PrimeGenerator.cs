using System;

namespace PrimeGenerator
{
    class PrimeGenerator
    {
        static void Main()
        {
            //Parsing (Reading input from standard input)
            string test = Console.ReadLine();
            int numIntervals = int.Parse(test);

            int[,] intervals = new int[numIntervals, 2];
            int n = 0;
            for (int i = 0; i < numIntervals; i++)
            {
                string line = Console.ReadLine();
                string[] elements = line.Split(' ');
                intervals[i, 0] = int.Parse(elements[0]) == 1 ? 2 : int.Parse(elements[0]); //1 is not considered prime number in this problem
                intervals[i, 1] = int.Parse(elements[1]);

                if (intervals[i, 1] > n) n = intervals[i, 1];
            }

            int[] sizes = new int[intervals.GetLength(0)];
            for (int i = 0; i < intervals.GetLength(0); i++)
            {
                sizes[i] = intervals[i, 1] - intervals[i, 0] + 1;
            }


            //Generate core primes (primes between 1 and sqrt(n))
            int limit = 1000000000;
            int prime_limit = (int)Math.Sqrt(limit) + 1;
            bool[] isNotPrime = new bool[prime_limit];
            //isNotPrime[0] = true;

            for (int i = 2; i * i < prime_limit; i++)
            {
                if (!isNotPrime[i])
                {
                    int factor = 0;
                    for (int j = i * i; j < prime_limit; j = i * i + factor * i)
                    {
                        isNotPrime[j] = true;

                        factor++;
                    }
                }
            }

            int corePrimesArraySize = 0;
            for (int i = 0; i < isNotPrime.Length; i++)
            {
                if (!isNotPrime[i])
                {
                    corePrimesArraySize++;
                }
            }
            corePrimesArraySize -= 2;//0 and 1 are not prime in this solution


            int[] corePrimes = new int[corePrimesArraySize];
            int corePrimesCounter = 0;
            for (int i = 2; i < isNotPrime.Length; i++)
            {
                if (!isNotPrime[i])
                {
                    corePrimes[corePrimesCounter] = i;

                    corePrimesCounter++;
                }
            }

            //Sieve of Eratosthenes for each interval
            for (int currentInterval = 0; currentInterval < numIntervals; currentInterval++)
            {
                if (intervals[currentInterval, 0] == intervals[currentInterval, 1])
                {
                    if (!isNotPrime[intervals[currentInterval, 0]]) Console.WriteLine(intervals[currentInterval, 0]);

                    continue;
                }

                bool[] notPrimes = new bool[sizes[currentInterval]];
                int primesIndex = 0;
                for (int corePrimesIndex = 0; corePrimesIndex < corePrimes.Length && corePrimes[corePrimesIndex] * corePrimes[corePrimesIndex] < intervals[currentInterval, 1] && primesIndex < notPrimes.Length;)
                {
                    if (!notPrimes[primesIndex])
                    {
                        for (int j = (intervals[currentInterval, 0] / corePrimes[corePrimesIndex]) * corePrimes[corePrimesIndex]; j <= intervals[currentInterval, 1]; j += corePrimes[corePrimesIndex])
                        {
                            int currentIndex = j - intervals[currentInterval, 0];

                            if (j == corePrimes[corePrimesIndex]) { }

                            else if (currentIndex >= 0)
                            {
                                notPrimes[currentIndex] = true;
                            }
                        }
                        corePrimesIndex++;
                    }

                    primesIndex = (primesIndex + 1) % notPrimes.Length;
                }

                for (int i = 0; i < notPrimes.Length; i++)
                {
                    if (!notPrimes[i])
                    {
                        Console.WriteLine(i + intervals[currentInterval, 0]);
                    }
                }
                if (currentInterval != numIntervals - 1)
                {
                    Console.WriteLine();
                }

            }
        }
    }
}
