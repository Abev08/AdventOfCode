internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");
    // Part one
    Dictionary<string, int> rules = new Dictionary<string, int>() {
      { "A X", 1 + 3 }, { "A Y", 2 + 6 }, { "A Z", 3 + 0 },
      { "B X", 1 + 0 }, { "B Y", 2 + 3 }, { "B Z", 3 + 6 },
      { "C X", 1 + 6 }, { "C Y", 2 + 0 }, { "C Z", 3 + 3 }
      };

    int score = 0;
    for (int i = 0; i < input.Length; i++) score += rules[input[i]];

    Console.WriteLine($"Part one answer -> If everything would go according to my strategy the score would be {score}");

    // Part two
    rules = new Dictionary<string, int>() {
      { "A X", 3 + 0 }, { "A Y", 1 + 3 }, { "A Z", 2 + 6 },
      { "B X", 1 + 0 }, { "B Y", 2 + 3 }, { "B Z", 3 + 6 },
      { "C X", 2 + 0 }, { "C Y", 3 + 3 }, { "C Z", 1 + 6 }
      };
    score = 0;
    for (int i = 0; i < input.Length; i++) score += rules[input[i]];

    Console.WriteLine($"Part two answer -> If everything would go according to my updated strategy the score would be {score}");
  }
}
