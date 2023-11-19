internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Part one
    string secondCompartment;
    int sumOfPriorities = 0;
    for (int i = 0; i < input.Length; i++)
    {
      secondCompartment = input[i].Substring(input[i].Length / 2);
      for (int j = 0; j < secondCompartment.Length; j++)
      {
        if (secondCompartment.Contains(input[i][j]))
        {
          if (input[i][j] < 95) sumOfPriorities += input[i][j] - 38; // Upper case
          else sumOfPriorities += input[i][j] - 96; // Lower case
          break;
        }
      }
    }
    Console.WriteLine("Part one answer -> Sum of priorites of duplicated items: " + sumOfPriorities);

    // Part two
    sumOfPriorities = 0;
    for (int i = 0; i < input.Length; i += 3)
    {
      for (int j = 0; j < input[i].Length; j++)
      {
        if (input[i + 1].Contains(input[i][j]) && input[i + 2].Contains(input[i][j]))
        {
          if (input[i][j] < 95) sumOfPriorities += input[i][j] - 38; // Upper case
          else sumOfPriorities += input[i][j] - 96; // Lower case
          break;
        }
      }
    }
    Console.WriteLine("Part two answer -> Sum of priorites of badges: " + sumOfPriorities);
  }
}