using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202114
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            try
            {
                input = File.ReadAllLines("../../../inputTest.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }

            List<char> polymer = new List<char>();
            List<char> newPolymer = new List<char>();
            List<string[]> insertionRules = new List<string[]>();
            List<(char, int)> possibleLetters = new List<(char, int)>();

            polymer.AddRange(input[0].ToCharArray());
            for (int i = 2; i < input.Length; i++)
            {
                string[] temp = input[i].Split(" -> ");
                insertionRules.Add(new string[] { temp[0], temp[1] });
                if (possibleLetters.Contains((temp[1][0], 0)) == false) possibleLetters.Add((temp[1][0], 0));
            }

            // Part one / two
            for (int step = 0; step < 10; step++)
            {
                for (int i = 0; i < polymer.Count - 1; i++)
                {
                    string temp = polymer[i] + "" + polymer[i + 1];
                    foreach (string[] rule in insertionRules)
                    {
                        if (temp == rule[0])
                        {
                            if (i == 0) newPolymer.AddRange(new char[] { polymer[i], rule[1][0], polymer[i + 1] });
                            else newPolymer.AddRange(new char[] { rule[1][0], polymer[i + 1] });
                            break;
                        }
                    }
                }
                polymer = newPolymer;
                newPolymer = new List<char>();
            }
            for (int i = 0; i < possibleLetters.Count; i++)
            {
                possibleLetters[i] = (possibleLetters[i].Item1, polymer.FindAll(x => x == possibleLetters[i].Item1).Count);
            }
            possibleLetters.Sort((x, y) => { if (x.Item2 < y.Item2) return -1; else return 0; });
            Console.WriteLine("Part one answer -> quantity of the most common element minus the quantity of the least common element: " + (possibleLetters[^1].Item2 - possibleLetters[0].Item2));

            // Part two - different approach?
            //Dictionary<char, int> possibleLtters2 = new Dictionary<char, int>();
            //foreach (var letter in possibleLetters) possibleLtters2.Add(letter.Item1, 0);
            //Dictionary<string, int> pairs = new Dictionary<string, int>();
            //for (int i = 0; i < input[0].Length - 1; i++)
            //{
            //    string key = input[0][i] + "" + input[0][i + 1];
            //    if (pairs.ContainsKey(key) == true) pairs[key]++;
            //    else pairs.Add(key, 1);
            //}
            //for (int step = 0; step < 10; step++)
            //{
            //    Dictionary<string, int> newPairs = new Dictionary<string, int>();
            //    //foreach (var pair in pairs) newPairs.Add(pair.Key, pair.Value);

            //    foreach (string[] rule in insertionRules)
            //    {
            //        if (pairs.ContainsKey(rule[0]) == true)
            //        {
            //            if (newPairs.ContainsKey(rule[0][0] + rule[1]) == true) newPairs[rule[0][0] + rule[1]]++;
            //            else newPairs.Add(rule[0][0] + rule[1], 1);
            //            if (newPairs.ContainsKey(rule[1] + rule[0][1]) == true) newPairs[rule[1] + rule[0][1]]++;
            //            else newPairs.Add(rule[1] + rule[0][1], 1);
            //        }
            //    }
            //    pairs = newPairs;
            //}
            //foreach (var pair in pairs)
            //{
            //    possibleLtters2[pair.Key[0]] += pair.Value;
            //    //possibleLtters2[pair.Key[1]] += pair.Value;
            //}

            //Console.WriteLine("Part two answer -> quantity of the most common element minus the quantity of the least common element: " + (possibleLetters[^1].Item2 - possibleLetters[0].Item2));



            Console.ReadLine();
        }
    }
}
