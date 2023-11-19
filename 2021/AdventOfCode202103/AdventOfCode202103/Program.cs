using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202103
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            try { input = File.ReadAllLines("../../../input.txt"); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }

            // Part one
            PartOne(input);

            // Part two
            PartTwo(input);

            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            string gamma = "", epsilon = "";
            int[] numOfZeros = new int[input[0].Length];
            int[] numOfOnes = new int[input[0].Length];
            foreach (string s in input)
            {
                if (string.IsNullOrEmpty(s)) continue;

                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '0') numOfZeros[i]++;
                    else numOfOnes[i]++;
                }
            }

            for (int i = 0; i < numOfZeros.Length; i++)
            {
                if (numOfZeros[i] > numOfOnes[i])
                {
                    gamma += '0';
                    epsilon += '1';
                }
                else
                {
                    gamma += '1';
                    epsilon += '0';
                }
            }
            Console.WriteLine("Part one answer -> gamma: " + Convert.ToInt32(gamma, 2) + ", epsilon: " + Convert.ToInt32(epsilon, 2) + ", power consumption: " + Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2));
        }

        static void PartTwo(string[] input)
        {
            List<string> zeros = new List<string>(), ones = new List<string>();
            List<string> generator = new List<string>(); // oxygen generator rating
            List<string> scrubber = new List<string>(); // CO2 scrubber rating

            // Split input string into lists that start with a 0 or 1
            int numOfOnes = 0, numOfZeros = 0;
            foreach (string s in input)
            {
                if (string.IsNullOrEmpty(s)) continue;

                if (s[0] == '0')
                {
                    zeros.Add(s);
                    numOfZeros++;
                }
                else
                {
                    ones.Add(s);
                    numOfOnes++;
                }
            }

            // Assign starting lists to generator and scrubber
            if (numOfZeros > numOfOnes)
            {
                generator.AddRange(zeros);
                scrubber.AddRange(ones);
            }
            else
            {
                generator.AddRange(ones);
                scrubber.AddRange(zeros);
            }

            int maxNum, letterIndex = 1;
            while ((generator.Count > 1) || (scrubber.Count > 1))
            {
                List<string> genZerosTemp = new List<string>();
                List<string> genOnesTemp = new List<string>();
                List<string> scrZerosTemp = new List<string>();
                List<string> scrOnesTemp = new List<string>();

                maxNum = Math.Max(generator.Count, scrubber.Count);

                for (int i = 0; i < maxNum; i++)
                {
                    if ((generator.Count > 2) && (i < generator.Count))
                    {
                        if (generator[i][letterIndex] == '1') genOnesTemp.Add(generator[i]);
                        else genZerosTemp.Add(generator[i]);
                    }
                    else if (generator.Count == 2)
                    {
                        if (generator[0][letterIndex] == '1') generator.RemoveAt(1);
                        else generator.RemoveAt(0);
                    }

                    if ((scrubber.Count > 2) && (i < scrubber.Count))
                    {
                        if (scrubber[i][letterIndex] == '1') scrOnesTemp.Add(scrubber[i]);
                        else scrZerosTemp.Add(scrubber[i]);
                    }
                    else if (scrubber.Count == 2)
                    {
                        if (scrubber[0][letterIndex] == '0') scrubber.RemoveAt(1);
                        else scrubber.RemoveAt(0);
                    }
                }

                if (genZerosTemp.Count > genOnesTemp.Count) generator = genZerosTemp;
                else if ((genZerosTemp.Count != 0) && (genZerosTemp.Count <= genOnesTemp.Count)) generator = genOnesTemp;

                if ((scrZerosTemp.Count != 0) && (scrZerosTemp.Count <= scrOnesTemp.Count)) scrubber = scrZerosTemp;
                else if (scrZerosTemp.Count > scrOnesTemp.Count) scrubber = scrOnesTemp;

                letterIndex++;
            }

            Console.WriteLine("Part two answer -> oxygen generator rating: " + Convert.ToInt32(generator[0], 2) + ", CO2 scrubber rating: " + Convert.ToInt32(scrubber[0], 2) + ", life support rating: " + Convert.ToInt32(generator[0], 2) * Convert.ToInt32(scrubber[0], 2));
        }
    }
}
