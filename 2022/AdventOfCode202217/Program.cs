internal class Program
{
  private static void Main(string[] args)
  {
    string input = File.ReadAllText(@"input.txt");

    // Part one
    List<string> layers = new();
    layers.Add("-------");
    int inputIndex = 0, rockIndex = 0, highestRockPoint = 0;
    Rock currentRock;
    int previousRollOverHeight = 0;
    long previousRollOverRockCount = 0;
    int rollOverRockDiff, rollOverHeightDiff;
    int rolledOverTimes = 0;
    int rolloverRockIndex = 4;
    long calculatedHeight = 0;
    for (long rockNumber = 0; rockNumber < 1000000000000; rockNumber++)
    {
      // Create new rock
      currentRock = new Rock(rockIndex, highestRockPoint + 4);

      // Move stone
      while (true)
      {
        switch (input[inputIndex])
        {
          case '>':
            currentRock.MoveRight(layers);
            break;
          case '<':
            currentRock.MoveLeft(layers);
            break;
          default: throw new Exception("Input wrong");
        }

        do { inputIndex = (inputIndex + 1) % input.Length; }
        while (input[inputIndex] != '>' && input[inputIndex] != '<');
        if (currentRock.MoveDown(layers)) break;
      }
      highestRockPoint = AddRockToTower(currentRock, layers, highestRockPoint);

      // Get next stone
      rockIndex = (rockIndex + 1) % 5;

      if (rockNumber == 2021)
      {
        PrintTower(layers);
        Console.WriteLine($"Part one answer -> After 2022 rocks fall, the tower will be {highestRockPoint} units tall");
      }
      else if (rockNumber > 2021)
      {
        // Part two
      }
    }

    Console.WriteLine($"Part two answer -> After 1000000000000 rocks fall, the tower will be {highestRockPoint} units tall. If rolled over values differs from previous roll over calculate the height yourself.");
    return;
  }

  static int AddRockToTower(Rock rock, List<string> layers, int highestRockPoint)
  {
    for (int i = 0; i < rock.RockPositions.Length; i++)
    {
      while (rock.RockPositions[i].Y > layers.Count - 1) layers.Add("       ");

      layers[rock.RockPositions[i].Y] = layers[rock.RockPositions[i].Y].Remove(rock.RockPositions[i].X, 1);
      layers[rock.RockPositions[i].Y] = layers[rock.RockPositions[i].Y].Insert(rock.RockPositions[i].X, "#");

      if (rock.RockPositions[i].Y > highestRockPoint) highestRockPoint = rock.RockPositions[i].Y;
    }

    return highestRockPoint;
  }

  static void PrintTower(List<string> layers)
  {
    for (int i = layers.Count - 1; i >= 0; i--) Console.WriteLine(layers[i]);
  }

  class Rock
  {
    public Vec2[] RockPositions;
    public int Type;

    public Rock(int rockType, int layer)
    {
      Type = rockType;
      RockPositions = rockType switch
      {
        0 => new Vec2[] { new Vec2(2, layer + 0), new Vec2(3, layer + 0), new Vec2(4, layer + 0), new Vec2(5, layer + 0) },
        1 => new Vec2[] { new Vec2(3, layer + 0), new Vec2(2, layer + 1), new Vec2(3, layer + 1), new Vec2(4, layer + 1), new Vec2(3, layer + 2) },
        2 => new Vec2[] { new Vec2(2, layer + 0), new Vec2(3, layer + 0), new Vec2(4, layer + 0), new Vec2(4, layer + 1), new Vec2(4, layer + 2) },
        3 => new Vec2[] { new Vec2(2, layer + 0), new Vec2(2, layer + 1), new Vec2(2, layer + 2), new Vec2(2, layer + 3) },
        4 => new Vec2[] { new Vec2(2, layer + 0), new Vec2(3, layer + 0), new Vec2(2, layer + 1), new Vec2(3, layer + 1) },
        _ => throw new Exception("Rock index wrong")
      };
    }

    public void MoveLeft(List<string> layers)
    {
      for (int i = 0; i < RockPositions.Length; i++)
      {
        if (RockPositions[i].X == 0) return; // Already at left edge

        if (RockPositions[i].Y < layers.Count)
        {
          if (layers[RockPositions[i].Y][RockPositions[i].X - 1] != ' ') return; // Something is to the left of the rock
        }
      }

      // Reached this point - can move to left, so move
      for (int i = 0; i < RockPositions.Length; i++) RockPositions[i].X--;
    }

    public void MoveRight(List<string> layers)
    {
      for (int i = 0; i < RockPositions.Length; i++)
      {
        if (RockPositions[i].X == 6) return; // Already at right edge

        if (RockPositions[i].Y < layers.Count)
        {
          if (layers[RockPositions[i].Y][RockPositions[i].X + 1] != ' ') return; // Something is to the right of the rock
        }
      }

      // Reached this point - can move to right, so move
      for (int i = 0; i < RockPositions.Length; i++) RockPositions[i].X++;
    }

    public bool MoveDown(List<string> layers)
    {
      for (int i = 0; i < RockPositions.Length; i++)
      {
        if (RockPositions[i].Y == 1) return true; // Already at the bottom

        if (RockPositions[i].Y <= layers.Count)
        {
          if (layers[RockPositions[i].Y - 1][RockPositions[i].X] != ' ') return true; // Something is below of the rock
        }
      }

      // Reached this point - can move down, so move
      for (int i = 0; i < RockPositions.Length; i++) RockPositions[i].Y--;
      return false; // Return false if can move down
    }
  }

  struct Vec2
  {
    public int X, Y;
    public Vec2(int x, int y) { X = x; Y = y; }
    public override string ToString() { return $"{X}, {Y}"; }
  }
}