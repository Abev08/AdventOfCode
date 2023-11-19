using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202109
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

            // Part one
            List<int> lowPoints = new List<int>();
            int[,] map = new int[input.Length, input[0].Length];
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++) map[i, j] = input[i][j] - '0';
            }
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    bool lowPoint = true;
                    if (i > 0) if (map[i - 1, j] <= map[i, j]) lowPoint = false;
                    if (i < input.Length - 1) if (map[i + 1, j] <= map[i, j]) lowPoint = false;
                    if (j > 0) if (map[i, j - 1] <= map[i, j]) lowPoint = false;
                    if (j < input[i].Length - 1) if (map[i, j + 1] <= map[i, j]) lowPoint = false;
                    if (lowPoint == true) lowPoints.Add(map[i, j]);
                }
            }
            int riskLevel = 0;
            foreach (int i in lowPoints) riskLevel += i + 1;
            Console.WriteLine("Part one answer -> Calculated risk level: " + riskLevel);

            // Part two
            List<int> poolsSizes = new List<int>();
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    var a = map[i, j];
                    if ((map[i, j] != -1) && (map[i, j] != 9))
                    {
                        poolsSizes.Add(CheckAdjecentCells(ref map, i, j, 0));
                    }
                }
            }
            poolsSizes.Sort();
            Console.WriteLine("Part two answer -> Value of 3 biggest pools multiplied together: " + poolsSizes[^1] * poolsSizes[^2] * poolsSizes[^3]);

            Console.ReadLine();
        }

        static int CheckAdjecentCells(ref int[,] map, int x, int y, int numOfCells)
        {
            if ((map[x, y] == -1) || (map[x, y] == 9)) return numOfCells;

            map[x, y] = -1;
            numOfCells++;
            if (x > 0) numOfCells = CheckAdjecentCells(ref map, x - 1, y, numOfCells);
            if (x < map.GetLength(0) - 1) numOfCells = CheckAdjecentCells(ref map, x + 1, y, numOfCells);
            if (y > 0) numOfCells = CheckAdjecentCells(ref map, x, y - 1, numOfCells);
            if (y < map.GetLength(1) - 1) numOfCells = CheckAdjecentCells(ref map, x, y + 1, numOfCells);
            return numOfCells;
        }
    }
}