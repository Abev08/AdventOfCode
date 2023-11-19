internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Parse input
    string[] points;
    List<Vector2> rockTiles = new();
    Vector2 currentRock, endRock;
    Vector2 min = new Vector2(int.MaxValue, int.MaxValue);
    Vector2 max = new Vector2(int.MinValue, int.MinValue);
    for (int i = 0; i < input.Length; i++)
    {
      points = input[i].Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
      for (int j = 1; j < points.Length; j++)
      {
        currentRock = new Vector2(points[j - 1]);
        endRock = new Vector2(points[j]);
        TryAdd(rockTiles, currentRock, min, max);
        while (currentRock.X != endRock.X || currentRock.Y != endRock.Y)
        {
          if (currentRock.X > endRock.X) currentRock = new Vector2(currentRock.X - 1, currentRock.Y);
          else if (currentRock.X < endRock.X) currentRock = new Vector2(currentRock.X + 1, currentRock.Y);
          else if (currentRock.Y > endRock.Y) currentRock = new Vector2(currentRock.X, currentRock.Y - 1);
          else if (currentRock.Y < endRock.Y) currentRock = new Vector2(currentRock.X, currentRock.Y + 1);
          TryAdd(rockTiles, currentRock, min, max);
        }
      }
    }
    min.X -= 10; // Extend min and max
    min.Y = 0;
    max.X += 5;
    max.Y += 1;
    char[,] cave = new char[max.X - min.X, max.Y];
    InitCave(cave);
    // Generate cave
    for (int i = 0; i < rockTiles.Count; i++) cave[rockTiles[i].X - min.X, rockTiles[i].Y] = '#';
    cave[500 - min.X, 0] = '+'; // Add start tile

    // Part one
    Vector2? currentPosition;
    int sandFallenToVoid = 0, stacionarySand = 0;
    bool isStationary;
    while (sandFallenToVoid < 5)
    {
      currentPosition = new Vector2(500 - min.X, 1);
      isStationary = false;
      while (!isStationary)
      {
        if (currentPosition.Y >= max.Y - 1)
        {
          sandFallenToVoid++; // Fallen to void
          isStationary = true;
        }
        else if (cave[currentPosition.X, currentPosition.Y + 1] == ' ') currentPosition.Y++; // Try to move down
        else if (cave[currentPosition.X - 1, currentPosition.Y + 1] == ' ') { currentPosition.X--; currentPosition.Y++; } // Try to move down left
        else if (cave[currentPosition.X + 1, currentPosition.Y + 1] == ' ') { currentPosition.X++; currentPosition.Y++; } // Try to move down right
        else
        {
          cave[currentPosition.X, currentPosition.Y] = 'o'; // Occupy a place in cave
          isStationary = true;
          stacionarySand++;
        }
      }
    }
    PrintCave(cave);
    Console.WriteLine("Part one answer -> Amount of sand that came to rest before it began falling into void: " + stacionarySand);

    // Part two
    min.X -= 50000; // Extend min and max
    min.Y = 0;
    max.X += 50000;
    max.Y += 1;
    // Generate cave2
    char[,] cave2 = new char[max.X - min.X, max.Y + 1];
    InitCave(cave2);
    for (int i = 0; i < rockTiles.Count; i++) cave2[rockTiles[i].X - min.X, rockTiles[i].Y] = '#';
    cave2[500 - min.X, 0] = '+'; // Add start tile
    for (int i = 3; i < max.X - min.X; i++) cave2[i, max.Y] = '#'; // Add floor
    stacionarySand = 0;
    bool filledTheCave = false;
    while (!filledTheCave)
    {
      currentPosition = new Vector2(500 - min.X, 0);
      isStationary = false;
      while (!isStationary)
      {
        if (cave2[currentPosition.X, currentPosition.Y + 1] == ' ') currentPosition.Y++; // Try to move down
        else if (cave2[currentPosition.X - 1, currentPosition.Y + 1] == ' ') { currentPosition.X--; currentPosition.Y++; } // Try to move down left
        else if (cave2[currentPosition.X + 1, currentPosition.Y + 1] == ' ') { currentPosition.X++; currentPosition.Y++; } // Try to move down right
        else
        {
          cave2[currentPosition.X, currentPosition.Y] = 'o'; // Occupy a place in cave
          isStationary = true;
          stacionarySand++;
          if (currentPosition.X == 500 - min.X && currentPosition.Y == 0) filledTheCave = true;
        }
      }
    }
    Console.WriteLine("Part two answer -> Amount of sand that came to rest before it reached entry point: " + stacionarySand);
  }

  static void TryAdd(List<Vector2> rockTiles, Vector2 rock, Vector2 min, Vector2 max)
  {
    // Save cave size
    if (rock.X < min.X) min.X = rock.X;
    if (rock.X > max.X) max.X = rock.X;
    if (rock.Y < min.Y) min.Y = rock.Y;
    if (rock.Y > max.Y) max.Y = rock.Y;

    for (int i = 0; i < rockTiles.Count; i++)
    {
      if (rockTiles[i].X == rock.X && rockTiles[i].Y == rock.Y) return;
    }
    rockTiles.Add(rock);
  }

  static void InitCave(char[,] cave)
  {
    string number;
    for (int i = 0; i <= cave.GetUpperBound(0); i++)
    {
      for (int j = 0; j <= cave.GetUpperBound(1); j++)
      {
        if (i > 2) cave[i, j] = ' ';
        else if (i == 0)
        {
          cave[i, j] = ' ';
          cave[i + 1, j] = ' ';
          cave[i + 2, j] = ' ';
          number = j.ToString("D3");
          cave[i, j] = number[0];
          cave[i + 1, j] = number[1];
          cave[i + 2, j] = number[2];
        }
      }
    }
  }

  static void PrintCave(char[,] cave)
  {
    for (int j = 0; j <= cave.GetUpperBound(1); j++)
    {
      for (int i = 0; i <= cave.GetUpperBound(0); i++) Console.Write(cave[i, j]);
      Console.WriteLine();
    }
  }

  class Vector2
  {
    public int X, Y;
    public Vector2(string point)
    {
      X = int.Parse(point.Substring(0, point.IndexOf(',')));
      Y = int.Parse(point.Substring(point.IndexOf(',') + 1));
    }
    public Vector2(int x, int y) { X = x; Y = y; }
    public override string ToString() { return $"{X}, {Y}"; }
  }
}