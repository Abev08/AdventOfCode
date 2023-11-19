internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Part one
    int X = 1, cycle = 0;
    int signalStrength = 0;
    for (int i = 0; i < input.Length; i++)
    {
      if (input[i] == "noop")
      {
        cycle++;
        if (((cycle - 20) % 40 == 0) && (cycle <= 220)) signalStrength += cycle * X;
      }
      else
      {
        cycle++;
        if (((cycle - 20) % 40 == 0) && (cycle <= 220)) signalStrength += cycle * X;

        cycle++;
        if (((cycle - 20) % 40 == 0) && (cycle <= 220)) signalStrength += cycle * X;

        X += int.Parse(input[i].Substring(input[i].IndexOf(' ')));
      }
    }
    Console.WriteLine("Part one answer -> Sum of 6 signal strengths: " + signalStrength);

    // Part two
    int spriteIndex = 1; // Index of middle of sprite symbol -> "###....................................."
    Console.WriteLine("Part two answer ->");
    cycle = 0;
    for (int i = 0; i < input.Length; i++)
    {
      if (string.IsNullOrEmpty(input[i])) continue;

      if (input[i] == "noop")
      {
        Console.Write((((cycle % 40) >= spriteIndex - 1) && ((cycle % 40) <= spriteIndex + 1)) ? '█' : ' ');
        cycle++;
        if (cycle % 40 == 0) Console.WriteLine();
      }
      else
      {
        Console.Write((((cycle % 40) >= spriteIndex - 1) && ((cycle % 40) <= spriteIndex + 1)) ? '█' : ' ');
        cycle++;
        if (cycle % 40 == 0) Console.WriteLine();

        Console.Write((((cycle % 40) >= spriteIndex - 1) && ((cycle % 40) <= spriteIndex + 1)) ? '█' : ' ');
        cycle++;
        if (cycle % 40 == 0) Console.WriteLine();

        spriteIndex += int.Parse(input[i].Substring(input[i].IndexOf(' ')));
      }
    }
  }
}