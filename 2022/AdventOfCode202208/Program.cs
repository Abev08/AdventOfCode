internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Part one
    bool[] isHidden = new bool[4];
    int hiddenTrees = 0;
    for (int row = 1; row < input.Length - 1; row++)
    {
      for (int column = 1; column < input[0].Length - 1; column++)
      {
        isHidden = new bool[4];
        // check left
        for (int i = 1; i <= column; i++)
        {
          if (input[row][column - i] >= input[row][column])
          {
            isHidden[0] = true;
            break;
          }
        }
        // check right
        for (int i = 1; i < input[0].Length - column; i++)
        {
          if (input[row][column + i] >= input[row][column])
          {
            isHidden[1] = true;
            break;
          }
        }
        // check top
        for (int i = 1; i <= row; i++)
        {
          if (input[row - i][column] >= input[row][column])
          {
            isHidden[2] = true;
            break;
          }
        }
        // check down
        for (int i = 1; i < input.Length - row; i++)
        {
          if (input[row + i][column] >= input[row][column])
          {
            isHidden[3] = true;
            break;
          }
        }

        // Check if tree is hidden from all sides
        if (isHidden.All(x => x == true)) hiddenTrees++;
      }
    }
    Console.WriteLine("Part one answer -> Amount of trees visible from outside the grid: " + ((input.Length * input[0].Length) - hiddenTrees));

    // Part two
    int[] viewDistance;
    int scenicScore = 0, newScenicScore;
    for (int row = 1; row < input.Length - 1; row++)
    {
      for (int column = 1; column < input[0].Length - 1; column++)
      {
        viewDistance = new int[] { 0, 0, 0, 0 };
        // check left
        for (int i = 1; i <= column; i++)
        {
          if (input[row][column - i] >= input[row][column]) { viewDistance[0]++; break; }
          else viewDistance[0]++;
        }
        // check right
        for (int i = 1; i < input[0].Length - column; i++)
        {
          if (input[row][column + i] >= input[row][column]) { viewDistance[1]++; break; }
          else viewDistance[1]++;
        }
        // check top
        for (int i = 1; i <= row; i++)
        {
          if (input[row - i][column] >= input[row][column]) { viewDistance[2]++; break; }
          else viewDistance[2]++;
        }
        // check down
        for (int i = 1; i < input.Length - row; i++)
        {
          if (input[row + i][column] >= input[row][column]) { viewDistance[3]++; break; }
          else viewDistance[3]++;
        }

        // Check current tree scenic score
        newScenicScore = viewDistance.Aggregate((a, b) => a * b);
        if (newScenicScore > scenicScore) scenicScore = newScenicScore;
      }
    }
    Console.WriteLine("Part two answer -> Tree with highest scenic score have score of: " + scenicScore);
  }
}