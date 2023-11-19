internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Part one
    int maxCalories = 0;
    int currentCalories = 0;
    for (int i = 0; i < input.Length; i++)
    {
      if (string.IsNullOrEmpty(input[i]))
      {
        if (currentCalories > maxCalories) maxCalories = currentCalories;
        currentCalories = 0;
      }
      else currentCalories += int.Parse(input[i]);
    }
    Console.WriteLine($"Part one answer -> Maximum calories carried by one Elf: {maxCalories}");

    // Part two
    List<int> calories = new List<int>();
    currentCalories = 0;
    for (int i = 0; i < input.Length; i++)
    {
      if (string.IsNullOrEmpty(input[i]))
      {
        calories.Add(currentCalories);
        currentCalories = 0;
      }
      else currentCalories += int.Parse(input[i]);
    }
    calories.Sort((x, y) => { return y - x; });
    Console.WriteLine($"Part two answer -> Maximum calories carried by top 3 Elfs: {calories[0] + calories[1] + calories[2]}");
  }
}