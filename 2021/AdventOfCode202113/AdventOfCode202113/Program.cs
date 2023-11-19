using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode202113
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            try
            {
                input = File.ReadAllLines("../../../input.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }

            List<int> x = new List<int>();
            List<int> y = new List<int>();
            int firstFoldIndex = 0;
            foreach (string line in input)
            {
                if (string.IsNullOrWhiteSpace(line)) break;
                string[] temp = line.Split(',');
                x.Add(int.Parse(temp[1]));
                y.Add(int.Parse(temp[0]));
                firstFoldIndex++;
            }

            int xDim = x.Max() + 1;
            int yDim = y.Max() + 1;
            int[,] paper = new int[xDim, yDim];
            for (int i = 0; i < x.Count; i++) paper[x[i], y[i]] = 1;

            // Part one / two
            List<string> instructions = new List<string>();
            for (int i = firstFoldIndex + 1; i < input.Length; i++) instructions.Add(input[i]);
            for (int part = 1; part < 3; part++)
            {
                int len;
                if (part == 1) len = 1;
                else len = instructions.Count;
                int visibleDots = 0;

                for (int instruction = part - 1; instruction < len; instruction++)
                {
                    string[] fold = instructions[instruction].Split(' ')[^1].Split('=');
                    int foldIndex = int.Parse(fold[1]);
                    switch (fold[0])
                    {
                        case "x":
                            for (int i = 0; i < xDim; i++)
                            {
                                for (int j = 0; j < (yDim - foldIndex); j++)
                                {
                                    if (paper[i, foldIndex + j] == 1) paper[i, foldIndex - j] = 1;
                                }
                            }
                            yDim = foldIndex;
                            break;
                        case "y":
                            for (int i = 0; i < yDim; i++)
                            {
                                for (int j = 0; j < (xDim - foldIndex); j++)
                                {
                                    if (paper[foldIndex + j, i] == 1) paper[foldIndex - j, i] = 1;
                                }
                            }
                            xDim = foldIndex;
                            break;
                    }
                    if (part == 1)
                    {
                        for (int i = 0; i < xDim; i++)
                        {
                            for (int j = 0; j < yDim; j++)
                            {
                                if (paper[i, j] == 1) visibleDots++;
                            }
                        }
                    }
                }

                if (part == 1) Console.WriteLine("Part one answer -> After the first folding there are visible " + visibleDots + " dots.");
                else
                {
                    Console.WriteLine("Part two answer:");
                    for (int i = 0; i < xDim; i++)
                    {
                        for (int j = 0; j < yDim; j++)
                        {
                            if (paper[i, j] == 0) Console.Write(' ');
                            else Console.Write('*');
                        }
                        Console.WriteLine();
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
