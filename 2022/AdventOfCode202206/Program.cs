internal class Program
{
  private static void Main(string[] args)
  {
    string input = File.ReadAllText(@"input.txt");

    Dictionary<char, bool> characters = new Dictionary<char, bool>();
    bool lookForPartOne = true, lookForPartTwo = true;
    for (int i = 0; i < input.Length; i++)
    {
      // Part one
      if (lookForPartOne)
      {
        if (characters.TryAdd(input[i], false) && characters.TryAdd(input[i + 1], false) &&
        characters.TryAdd(input[i + 2], false) && characters.TryAdd(input[i + 3], false))
        {
          Console.WriteLine("Part one awswer -> Packet start is at index " + (i + 4));
          lookForPartOne = false;
        }
        characters.Clear();
      }

      // Part two
      if (lookForPartTwo)
      {
        for (int j = 0; j < 14; j++)
        {
          if (!characters.TryAdd(input[i + j], false)) break;
          else if (j == 13)
          {
            Console.WriteLine("Part two awswer -> Message start is at index " + (i + 14));
            lookForPartTwo = false;
          }
        }
        characters.Clear();
      }

      if (!(lookForPartOne || lookForPartTwo)) break;
    }
  }
}