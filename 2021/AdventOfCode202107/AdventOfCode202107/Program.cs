using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202107
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

            int maxPos = 0;
            List<int> crabs = new List<int>();
            foreach (string s in input)
            {
                crabs.Add(int.Parse(s));
                if (crabs[^1] > maxPos) maxPos = crabs[^1];
            }

            // Part one
            int minFuel = int.MaxValue;
            int minPos = -1;
            for (int i = 0; i <= maxPos; i++)
            {
                int fuel = 0;
                foreach (int crab in crabs)
                {
                    fuel += Math.Abs(i - crab);
                }
                if (fuel < minFuel)
                {
                    minFuel = fuel;
                    minPos = i;
                }
            }
            Console.WriteLine("Part one answer -> Crabs need minimum of " + minFuel + " fuel to reach " + minPos + " position.");

            // Part two
            minFuel = int.MaxValue;
            minPos = -1;
            for (int i = 0; i <= maxPos; i++)
            {
                int fuel = 0;
                foreach (int crab in crabs)
                {
                    fuel += (int)(0.5 * Math.Pow(Math.Abs(i - crab), 2) + 0.5 * Math.Abs(i - crab));
                }
                if (fuel < minFuel)
                {
                    minFuel = fuel;
                    minPos = i;
                }
            }
            Console.WriteLine("Part two answer -> Crabs need minimum of " + minFuel + " fuel to reach " + minPos + " position.");

            Console.ReadLine();
        }
    }
}
