using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202104
{
    class Program
    {
        static int boardSize = 5;

        static void Main(string[] args)
        {
            string[] input;
            try { input = File.ReadAllLines("../../../input.txt"); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }

            string[] numbers = new string[0];
            List<string[,]> Boards = new List<string[,]>();
            List<string[,]> BoardsPlaying = new List<string[,]>();

            List<string[]> tempBoard = new List<string[]>();
            int boardIndex = -1;
            foreach (string s in input)
            {
                if (numbers.Length == 0) numbers = s.Split(',');
                else
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        if (boardIndex >= 0)
                        {
                            Boards.Add(ConvertToBoard(tempBoard));
                            BoardsPlaying.Add(ConvertToBoard(tempBoard));
                            tempBoard = new List<string[]>();
                        }

                        boardIndex++;
                    }
                    else tempBoard.Add(s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                }
            }
            if (tempBoard.Count == 5)
            {
                Boards.Add(ConvertToBoard(tempBoard));
                BoardsPlaying.Add(ConvertToBoard(tempBoard));
            }

            bool end = false;
            int winningIndex = -1, numberIndex = 0;
            while (end == false)
            {
                foreach (string[,] s in BoardsPlaying)
                {
                    for (int i = 0; i < boardSize; i++)
                    {
                        for (int j = 0; j < boardSize; j++)
                        {
                            if (s[i, j] == numbers[numberIndex])
                            {
                                s[i, j] = "-1";
                            }
                        }
                    }
                }

                (end, winningIndex) = CheckForWinningBoard(BoardsPlaying);
                if (end == false) numberIndex++;
            }

            int sumOfMarkedNumbers, sumOfUnmarkedNumbers;
            (sumOfMarkedNumbers, sumOfUnmarkedNumbers) = SumOfNumbers(BoardsPlaying[winningIndex], Boards[winningIndex]);
            Console.WriteLine("Part one answer -> sum of unmarked numbers: " + sumOfUnmarkedNumbers + ", winning number: " + numbers[numberIndex] + ", score: " + sumOfUnmarkedNumbers * int.Parse(numbers[numberIndex]));

            // Part two
            List<int> winningBoardsIndexes = new List<int>();
            BoardsPlaying = Boards;
            numberIndex = 0;
            while (numberIndex < numbers.Length)
            {
                foreach (string[,] s in BoardsPlaying)
                {
                    for (int i = 0; i < boardSize; i++)
                    {
                        for (int j = 0; j < boardSize; j++)
                        {
                            if (s[i, j] == numbers[numberIndex])
                            {
                                s[i, j] = "-1";
                            }
                        }
                    }
                }

                CheckForWinningBoard(BoardsPlaying, ref winningBoardsIndexes);
                if (winningBoardsIndexes.Count == Boards.Count) break;
                else numberIndex++;
            }

            (sumOfMarkedNumbers, sumOfUnmarkedNumbers) = SumOfNumbers(BoardsPlaying[winningBoardsIndexes[^1]], Boards[winningBoardsIndexes[^1]]);
            Console.WriteLine("Part two answer -> sum of unmarked numbers: " + sumOfUnmarkedNumbers + ", winning number: " + numbers[numberIndex] + ", score: " + sumOfUnmarkedNumbers * int.Parse(numbers[numberIndex]));

            Console.ReadLine();
        }

        static string[,] ConvertToBoard(List<string[]> input)
        {
            string[,] board = new string[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = input[i][j];
                }
            }
            return board;
        }

        static (bool, int) CheckForWinningBoard(List<string[,]> boards)
        {
            int index = 0;
            bool wins = false;

            for (index = 0; index < boards.Count; index++)
            {
                if (CheckIfBoardIsWinning(boards[index]))
                {
                    wins = true;
                    break;
                }
            }

            return (wins, index);
        }

        static void CheckForWinningBoard(List<string[,]> boards, ref List<int> winningBoardsIndexes)
        {
            int index = 0;

            for (index = 0; index < boards.Count; index++)
            {
                if (CheckIfBoardIsWinning(boards[index]))
                {
                    if (winningBoardsIndexes.Contains(index) == false) winningBoardsIndexes.Add(index);
                }
            }
        }

        static bool CheckIfBoardIsWinning(string[,] board)
        {
            bool wins = false;

            for (int i = 0; i < boardSize; i++)
            {
                // Sprawdź wiersze
                if (board[i, 0] == "-1")
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[i, j] != "-1") break;
                        if (j == boardSize - 1) wins = true;
                    }
                }
                if (wins == true) break;

                // Sprawdź kolumny
                if (board[0, i] == "-1")
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[j, i] != "-1") break;
                        if (j == boardSize - 1) wins = true;
                    }
                }
                if (wins == true) break;
            }

            return wins;
        }

        static (int, int) SumOfNumbers(string[,] boardMarked, string[,] boardUnmarked)
        {
            int sumOfMarked = 0, sumOfUnmarked = 0;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (boardMarked[i, j] == "-1") sumOfMarked += int.Parse(boardUnmarked[i, j]);
                    else sumOfUnmarked += int.Parse(boardUnmarked[i, j]);
                }
            }

            return (sumOfMarked, sumOfUnmarked);
        }
    }
}
