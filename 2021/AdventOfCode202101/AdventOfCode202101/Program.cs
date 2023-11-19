using System;
using System.IO;

namespace AdventOfCode202101
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            try { input = File.ReadAllText("../../../input.txt"); }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            };

            // Parse string text
            string[] scanText = input.Split('\n');
            int[] scan = new int[scanText.Length];
            for (int i = 0; i < scanText.Length - 1; i++) scan[i] = int.Parse(scanText[i]);

            // Part one
            int prevNum = scan[0], increased = 0;
            for (int i = 1; i < scan.Length - 1; i++)
            {
                if (scan[i] > prevNum) increased++;
                prevNum = scan[i];
            }
            Console.WriteLine("Part one answer:");
            Console.WriteLine("Number of times scan has increased: " + increased);

            // Part two
            increased = 0;
            prevNum = scan[0] + scan[1] + scan[2];
            int num;
            for (int i = 1; i < scan.Length - 3; i++)
            {
                num = scan[i] + scan[i + 1] + scan[i + 2];
                if (num > prevNum) increased++;
                prevNum = num;
            }
            Console.WriteLine("Part two answer:");
            Console.WriteLine("Number of times scan has increased: " + increased);

            Console.ReadLine();
        }
    }
}