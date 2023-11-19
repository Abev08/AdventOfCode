using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AdventOfCode202105
{
    class Program
    {
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

            List<Point[]> points = new List<Point[]>();
            int maxX = 0, maxY = 0;
            foreach (string s in input)
            {
                string[] tempPoint = s.Split(new string[] { " -> ", "," }, StringSplitOptions.RemoveEmptyEntries);

                points.Add(new Point[] { new Point(int.Parse(tempPoint[0]), int.Parse(tempPoint[1])), new Point(int.Parse(tempPoint[2]), int.Parse(tempPoint[3])) });
                maxX = points[^1][0].X > maxX ? points[^1][0].X : maxX;
                maxX = points[^1][1].X > maxX ? points[^1][1].X : maxX;
                maxY = points[^1][0].Y > maxY ? points[^1][0].Y : maxY;
                maxY = points[^1][1].Y > maxY ? points[^1][1].Y : maxY;
            }

            // Part one
            int[,] board = new int[maxX + 1, maxY + 1];
            foreach (Point[] p in points)
            {
                Point startPoint, endPoint;
                if ((p[0].X <= p[1].X) && (p[0].Y <= p[1].Y)) { startPoint = p[0]; endPoint = p[1]; }
                else { startPoint = p[1]; endPoint = p[0]; }

                if (startPoint.X == endPoint.X)
                {
                    for (int i = startPoint.Y; i <= endPoint.Y; i++)
                    {
                        board[startPoint.X, i]++;
                    }
                }
                else if (startPoint.Y == endPoint.Y)
                {
                    for (int i = startPoint.X; i <= endPoint.X; i++)
                    {
                        board[i, startPoint.Y]++;
                    }
                }
            }

            int numberOfDangerousAreas = 0;
            for (int i = 0; i <= maxX; i++)
            {
                for (int j = 0; j <= maxY; j++)
                {
                    if (board[i, j] > 1) numberOfDangerousAreas++;
                    //Console.Write(board[j, i]);
                }
                //Console.WriteLine();
            }

            Console.WriteLine("Part one answer -> number of dangerous areas: " + numberOfDangerousAreas);


            // Part two
            board = new int[maxX + 1, maxY + 1];
            foreach (Point[] p in points)
            {
                int distance = Math.Abs(p[0].X - p[1].X) > Math.Abs(p[0].Y - p[1].Y) ? Math.Abs(p[0].X - p[1].X) : Math.Abs(p[0].Y - p[1].Y);
                int xDir = p[0].X > p[1].X ? -1 : 1;
                int yDir = p[0].Y > p[1].Y ? -1 : 1;
                if (p[0].X == p[1].X) xDir = 0;
                if (p[0].Y == p[1].Y) yDir = 0;

                for (int i = 0; i <= distance; i++)
                {
                    board[p[0].X + (i * xDir), p[0].Y + (i * yDir)]++;
                }
            }

            numberOfDangerousAreas = 0;
            for (int i = 0; i <= maxX; i++)
            {
                for (int j = 0; j <= maxY; j++)
                {
                    if (board[i, j] > 1) numberOfDangerousAreas++;
                    //if (board[j, i] != 0) Console.Write(board[j, i]);
                    //else Console.Write('.');
                }
                //Console.WriteLine();
            }

            Console.WriteLine("Part two answer -> number of dangerous areas: " + numberOfDangerousAreas);


            Console.ReadLine();
        }
    }
}