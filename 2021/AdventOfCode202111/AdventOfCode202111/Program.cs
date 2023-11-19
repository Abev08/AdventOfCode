using System;
using System.IO;

namespace AdventOfCode202111
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

            int[,] octopuses = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    octopuses[i, j] = input[i][j] - '0';
                }
            }

            // Part one
            int numberOfFlashes = 0;
            int step;
            for (step = 0; step < 100; step++)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (octopuses[i, j] != -1) octopuses[i, j]++;
                        if (octopuses[i, j] > 9) numberOfFlashes = Flash(ref octopuses, i, j, numberOfFlashes);
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (octopuses[i, j] == -1) octopuses[i, j] = 0;
                    }
                }
            }
            Console.WriteLine("Part one answer -> Number of flashes after 100 steps: " + numberOfFlashes);

            // Part two
            bool everyoneFlashed = false;
            while (everyoneFlashed == false)
            {
                step++;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (octopuses[i, j] != -1) octopuses[i, j]++;
                        if (octopuses[i, j] > 9) numberOfFlashes = Flash(ref octopuses, i, j, numberOfFlashes);
                    }
                }
                everyoneFlashed = true; // Assume that everyone flashed
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        everyoneFlashed = (everyoneFlashed == true) && ((octopuses[i, j] == -1) || (octopuses[i, j] == 0));
                        if (octopuses[i, j] == -1) octopuses[i, j] = 0;
                    }
                }
            }
            Console.WriteLine("Part two answer -> All of the octopuses flashed simultaneously at step: " + step);

            Console.ReadLine();
        }

        static int Flash(ref int[,] octopuses, int x, int y, int flashNumber)
        {
            octopuses[x, y] = -1;
            flashNumber++;

            if (x > 0)
            {
                if (octopuses[x - 1, y] != -1) octopuses[x - 1, y]++;
                if (octopuses[x - 1, y] > 9) flashNumber = Flash(ref octopuses, x - 1, y, flashNumber);
            }
            if (x < 9)
            {
                if (octopuses[x + 1, y] != -1) octopuses[x + 1, y]++;
                if (octopuses[x + 1, y] > 9) flashNumber = Flash(ref octopuses, x + 1, y, flashNumber);
            }
            if (y > 0)
            {
                if (octopuses[x, y - 1] != -1) octopuses[x, y - 1]++;
                if (octopuses[x, y - 1] > 9) flashNumber = Flash(ref octopuses, x, y - 1, flashNumber);
            }
            if (y < 9)
            {
                if (octopuses[x, y + 1] != -1) octopuses[x, y + 1]++;
                if (octopuses[x, y + 1] > 9) flashNumber = Flash(ref octopuses, x, y + 1, flashNumber);
            }
            if ((x > 0) && (y > 0))
            {
                if (octopuses[x - 1, y - 1] != -1) octopuses[x - 1, y - 1]++;
                if (octopuses[x - 1, y - 1] > 9) flashNumber = Flash(ref octopuses, x - 1, y - 1, flashNumber);
            }
            if ((x < 9) && (y < 9))
            {
                if (octopuses[x + 1, y + 1] != -1) octopuses[x + 1, y + 1]++;
                if (octopuses[x + 1, y + 1] > 9) flashNumber = Flash(ref octopuses, x + 1, y + 1, flashNumber);
            }
            if ((x > 0) && (y < 9))
            {
                if (octopuses[x - 1, y + 1] != -1) octopuses[x - 1, y + 1]++;
                if (octopuses[x - 1, y + 1] > 9) flashNumber = Flash(ref octopuses, x - 1, y + 1, flashNumber);
            }
            if ((x < 9) && (y > 0))
            {
                if (octopuses[x + 1, y - 1] != -1) octopuses[x + 1, y - 1]++;
                if (octopuses[x + 1, y - 1] > 9) flashNumber = Flash(ref octopuses, x + 1, y - 1, flashNumber);
            }

            return flashNumber;
        }
    }
}
