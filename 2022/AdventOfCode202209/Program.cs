using System.Numerics;

internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Part one
    Vector2 headPosition = new Vector2();
    Vector2 tailPosition = new Vector2(0, 0);
    Vector2 tailMove = new Vector2();
    Dictionary<(float, float), bool> visitedPosition = new();
    visitedPosition.TryAdd((tailPosition.X, tailPosition.Y), false);
    char direction;
    int moveCount;
    for (int i = 0; i < input.Length; i++)
    {
      direction = input[i][0];
      moveCount = int.Parse(input[i].Substring(input[i].IndexOf(' ') + 1));

      for (int move = 0; move < moveCount; move++)
      {
        switch (direction)
        {
          case 'L': headPosition.X--; break;
          case 'R': headPosition.X++; break;
          case 'U': headPosition.Y--; break;
          case 'D': headPosition.Y++; break;
          default: throw new ArgumentException(nameof(direction));
        }

        if (Vector2.Distance(headPosition, tailPosition) > 1.5f)
        {
          if (headPosition.X != tailPosition.X && headPosition.Y != tailPosition.Y)
          {
            // Not in the same row and column - move diagonally
            if (headPosition.X < tailPosition.X) tailMove.X = -1;
            else tailMove.X = 1;
            if (headPosition.Y < tailPosition.Y) tailMove.Y = -1;
            else tailMove.Y = 1;
            tailPosition += tailMove;
          }
          else
          {
            // In the same row or column - just move towards head
            if (headPosition.X == tailPosition.X)
            {
              // The same row
              if (headPosition.Y < tailPosition.Y) tailPosition.Y--;
              else tailPosition.Y++;
            }
            else
            {
              // The same column
              if (headPosition.X < tailPosition.X) tailPosition.X--;
              else tailPosition.X++;
            }
          }
          visitedPosition.TryAdd((tailPosition.X, tailPosition.Y), false);
        }
      }
    }
    Console.WriteLine("Part one answer -> Amount of unique positions visited by tail: " + visitedPosition.Count);

    // Part two
    Vector2[] rope = new Vector2[10];
    visitedPosition.Clear();
    visitedPosition.Add((rope[^1].X, rope[^1].Y), false);
    for (int i = 0; i < input.Length; i++)
    {
      direction = input[i][0];
      moveCount = int.Parse(input[i].Substring(input[i].IndexOf(' ') + 1));

      for (int move = 0; move < moveCount; move++)
      {
        switch (direction)
        {
          case 'L': rope[0].X--; break;
          case 'R': rope[0].X++; break;
          case 'U': rope[0].Y--; break;
          case 'D': rope[0].Y++; break;
          default: throw new ArgumentException(nameof(direction));
        }

        for (int j = 1; j < rope.Length; j++)
        {
          if (Vector2.Distance(rope[j - 1], rope[j]) > 1.5f)
          {
            if (rope[j - 1].X != rope[j].X && rope[j - 1].Y != rope[j].Y)
            {
              // Not in the same row and column - move diagonally
              if (rope[j - 1].X < rope[j].X) tailMove.X = -1;
              else tailMove.X = 1;
              if (rope[j - 1].Y < rope[j].Y) tailMove.Y = -1;
              else tailMove.Y = 1;
              rope[j] += tailMove;
            }
            else
            {
              // In the same row or column - just move towards head
              if (rope[j - 1].X == rope[j].X)
              {
                // The same row
                if (rope[j - 1].Y < rope[j].Y) rope[j].Y--;
                else rope[j].Y++;
              }
              else
              {
                // The same column
                if (rope[j - 1].X < rope[j].X) rope[j].X--;
                else rope[j].X++;
              }
            }

            if (j == rope.Length - 1) visitedPosition.TryAdd((rope[^1].X, rope[^1].Y), false);
          }
        }
      }
    }
    Console.WriteLine("Part two answer -> Amount of unique positions visited by tail: " + visitedPosition.Count);
  }
}