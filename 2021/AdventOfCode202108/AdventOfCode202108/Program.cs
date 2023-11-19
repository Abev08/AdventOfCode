using System;
using System.IO;
using System.Linq;

namespace AdventOfCode202108
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
            int numOfNumbers = 0;
            int[] uniqueNumOfSegments = new int[] { 2, 3, 4, 7 };
            foreach (string s in input)
            {
                string[] numbers = s.Split(" | ")[1].Split(' ');

                foreach (string num in numbers)
                {
                    if (uniqueNumOfSegments.Contains(num.Length) == true) numOfNumbers++;
                }
            }
            Console.WriteLine("Part one answer -> digits 1, 4, 7 or 8 appear " + numOfNumbers + " times.");

            // Part two
            //  0000
            // 1    2
            // 1    2
            //  3333
            // 4    5
            // 4    5
            //  6666
            string[] digits = new string[]
            {
                "1110111", // 0
                "0010010", // 1
                "1011101", // 2
                "1011011", // 3
                "0111010", // 4
                "1101011", // 5
                "1101111", // 6
                "1010010", // 7
                "1111111", // 8
                "1111011"  // 9
            };
            int partTwoAnswer = 0;
            foreach (string row in input)
            {
                string[] rightPart = row.Split(" | ")[1].Split(' ');
                string[] numbers = row.Replace(" | ", " ").Split(' ');
                string[] display = new string[7];

                string one = numbers.First(x => x.Length == 2);
                string four = numbers.First(x => x.Length == 4);
                string seven = numbers.First(x => x.Length == 3);
                string eight = numbers.First(x => x.Length == 7);

                // segment 0 = num 7 - num 1
                display[0] = GetRemainingString(seven, one);

                // segment 6 = num 9 - (num 7 + num 4)
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i].Length == 6)
                    {
                        display[6] = GetRemainingString(numbers[i], seven + four);
                        if (display[6].Length == 1) break;
                    }
                }
                if (display[6].Length > 1) Console.WriteLine("Error in segment 6");

                // segment 3 = num 3 - (num 7 + seg 6)
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i].Length == 5)
                    {
                        display[3] = GetRemainingString(numbers[i], seven + display[6]);
                        if (display[3].Length == 1) break;
                    }
                }
                if (display[3].Length > 1) Console.WriteLine("Error in segment 3");

                // segment 1 = num 4 - (num 1 + seg 3)
                display[1] = GetRemainingString(four, one + display[3]);

                // segment 5 = num 5 - (seg 0 + seg 1 + seg 3 + seg 6)
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i].Length == 5)
                    {
                        display[5] = GetRemainingString(numbers[i], display[0] + display[1] + display[3] + display[6]);
                        if (display[5].Length == 1) break;
                    }
                }
                if (display[5].Length > 1) Console.WriteLine("Error in segment 5");

                // segment 2 = num 1 - seg 5
                display[2] = GetRemainingString(one, display[5]);

                // segment 4 = num 8 - (all other segments)
                display[4] = GetRemainingString(eight, display[0] + display[1] + display[2] + display[3] + display[5] + display[6]);

                // All segments done, lets translate right part of the row
                string rowAnswer = string.Empty;
                foreach (string s in rightPart)
                {
                    string[] digit = new string[] { "0", "0", "0", "0", "0", "0", "0" };
                    foreach (char c in s)
                    {
                        for (int i = 0; i < display.Length; i++)
                        {
                            if (c == display[i][0])
                            {
                                digit[i] = "1";
                                break;
                            }
                        }
                    }
                    string sdigit = string.Join("", digit);
                    for (int i = 0; i < digits.Length; i++)
                    {
                        if (sdigit == digits[i])
                        {
                            rowAnswer += i.ToString();
                            break;
                        }
                    }
                }
                partTwoAnswer += int.Parse(rowAnswer);
            }
            Console.WriteLine("Part two answer -> sum of output values: " + partTwoAnswer);

            Console.ReadLine();
        }

        static string GetRemainingString(string s1, string s2)
        {
            foreach (char c in s2) s1 = s1.Replace(c + "", "");

            return s1;
        }
    }
}
