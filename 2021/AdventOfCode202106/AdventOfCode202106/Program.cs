using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202106
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            try { input = File.ReadAllText("../../../input.txt").Split(','); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }

            List<int> fish = new List<int>();
            foreach (string s in input) fish.Add(int.Parse(s));

            // Part one
            for (int i = 0; i < 80; i++)
            {
                for (int j = fish.Count - 1; j >= 0; j--)
                {
                    fish[j]--;
                    if (fish[j] < 0)
                    {
                        fish[j] = 6;
                        fish.Add(8);
                    }
                }
            }
            Console.WriteLine("Part one answer -> After 80 days there will be " + fish.Count + " lanternfish");

            // Part two
            long[] fishTwo = new long[9];
            foreach (string s in input) fishTwo[int.Parse(s)]++;
            for (int i = 0; i < 256; i++)
            {
                long temp = fishTwo[0];
                Array.Copy(fishTwo, 1, fishTwo, 0, 8);
                fishTwo[8] = temp;
                fishTwo[6] += temp;
            }
            long sum = 0;
            for (int i = 0; i < 9; i++) sum += fishTwo[i];
            Console.WriteLine("Part two answer -> After 256 days there will be " + sum + " lanternfish");

            Console.ReadLine();
        }
    }
}