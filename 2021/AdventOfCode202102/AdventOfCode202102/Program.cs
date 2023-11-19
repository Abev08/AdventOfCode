using System;
using System.IO;

namespace AdventOfCode202102
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            try { input = File.ReadAllText("../../../input.txt").Split('\n'); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            // Part one
            int depth = 0, forward = 0;
            foreach (string s in input)
            {
                if (string.IsNullOrEmpty(s)) continue;

                string[] temp = s.Split(' ');
                switch (temp[0])
                {
                    case "forward":
                        forward += int.Parse(temp[1]);
                        break;

                    case "down":
                        depth += int.Parse(temp[1]);
                        break;

                    case "up":
                        depth -= int.Parse(temp[1]);
                        break;
                }
            }
            Console.WriteLine("Part one answer -> forward: " + forward + ", depth: " + depth + ", multiplied: " + forward * depth);

            // Part two
            int aim = depth = forward = 0;
            foreach (string s in input)
            {
                if (string.IsNullOrEmpty(s)) continue;

                string[] temp = s.Split(' ');
                switch (temp[0])
                {
                    case "forward":
                        forward += int.Parse(temp[1]);
                        depth += aim * int.Parse(temp[1]);
                        break;

                    case "down":
                        aim += int.Parse(temp[1]);
                        break;

                    case "up":
                        aim -= int.Parse(temp[1]);
                        break;
                }
            }
            Console.WriteLine("Part two answer -> forward: " + forward + ", depth: " + depth + ", multiplied: " + forward * depth);

            Console.ReadLine();
        }
    }
}
