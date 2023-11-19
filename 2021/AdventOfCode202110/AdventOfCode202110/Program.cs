using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202110
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

            List<string> incompleteLines = new List<string>(); // Part of part two

            // Part one
            List<char> closingCharacters = new List<char>() { ')', ']', '}', '>' };
            int syntaxErrorScore = 0;
            foreach (string line in input)
            {
                string chars = string.Empty;
                bool foundIllegalChar = false;
                foreach (char c in line)
                {
                    if (closingCharacters.Contains(c))
                    {
                        switch (c)
                        {
                            case ')':
                                if (chars[^1] == '(') chars = chars.Remove(chars.Length - 1);
                                else
                                {
                                    syntaxErrorScore += 3;
                                    foundIllegalChar = true;
                                }
                                break;
                            case ']':
                                if (chars[^1] == '[') chars = chars.Remove(chars.Length - 1);
                                else
                                {
                                    syntaxErrorScore += 57;
                                    foundIllegalChar = true;
                                }
                                break;
                            case '}':
                                if (chars[^1] == '{') chars = chars.Remove(chars.Length - 1);
                                else
                                {
                                    syntaxErrorScore += 1197;
                                    foundIllegalChar = true;
                                }
                                break;
                            case '>':
                                if (chars[^1] == '<') chars = chars.Remove(chars.Length - 1);
                                else
                                {
                                    syntaxErrorScore += 25137;
                                    foundIllegalChar = true;
                                }
                                break;

                        }
                    }
                    else chars += c;

                    if (foundIllegalChar == true) break;
                }

                if (foundIllegalChar == false) incompleteLines.Add(chars);
            }
            Console.WriteLine("Part one answer -> Total syntax error score: " + syntaxErrorScore);

            // Part two
            List<ulong> autocompleteScores = new List<ulong>();
            foreach (string line in incompleteLines)
            {
                ulong score = 0;
                for (int i = line.Length - 1; i >= 0; i--)
                {
                    switch (line[i])
                    {
                        case '(':
                            score = (score * 5) + 1;
                            break;
                        case '[':
                            score = (score * 5) + 2;
                            break;
                        case '{':
                            score = (score * 5) + 3;
                            break;
                        case '<':
                            score = (score * 5) + 4;
                            break;
                    }
                }
                autocompleteScores.Add(score);
            }
            autocompleteScores.Sort();
            Console.WriteLine("Part two answer -> Middle score for autocomplete lines: " + autocompleteScores[autocompleteScores.Count / 2]);

            Console.ReadLine();
        }
    }
}
