internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Read crates
    List<List<char>> crates = new List<List<char>>();
    for (int i = 0; i < (input[0].Length + 1) / 4; i++) crates.Add(new List<char>()); // Add crates columnts
    int index = 0;
    while (!input[index].StartsWith("move"))
    {
      // If input is empty just continue
      if (string.IsNullOrEmpty(input[index]))
      {
        index++;
        continue;
      }

      // Add crates to lists
      for (int i = 0; i < (input[index].Length + 1) / 4; i++)
      {
        if (char.IsWhiteSpace(input[index][1 + i * 4])) continue;
        crates[i].Insert(0, input[index][1 + i * 4]);
      }
      index++;
    }

    // Part one
    int count, from, to, indexof, indexCopy = index;
    List<List<char>> cratesCopy = new List<List<char>>();
    // Don't modify crates, work on copy
    for (int i = 0; i < crates.Count; i++)
    {
      cratesCopy.Add(new List<char>());
      for (int j = 0; j < crates[i].Count; j++) cratesCopy[i].Add(crates[i][j]);
    }
    while (indexCopy < input.Length)
    {
      if (string.IsNullOrEmpty(input[index])) continue;

      indexof = input[indexCopy].IndexOf("move") + "move ".Length;
      count = int.Parse(input[indexCopy].Substring(indexof, input[indexCopy].IndexOf(' ', indexof) - indexof));
      indexof = input[indexCopy].IndexOf("from") + "from ".Length;
      from = int.Parse(input[indexCopy].Substring(indexof, input[indexCopy].IndexOf(' ', indexof) - indexof));
      indexof = input[indexCopy].IndexOf("to") + "to ".Length;
      to = int.Parse(input[indexCopy].Substring(indexof));

      // Move crates count amount of times
      for (int i = 0; i < count; i++)
      {
        cratesCopy[to - 1].Add(cratesCopy[from - 1][^1]);
        cratesCopy[from - 1].RemoveAt(cratesCopy[from - 1].Count - 1);
      }

      indexCopy++;
    }
    Console.Write("Part one answer -> On top of each stack there are crates: ");
    for (int i = 0; i < cratesCopy.Count; i++) Console.Write(cratesCopy[i][^1]);
    Console.WriteLine();

    // Part two
    cratesCopy.Clear();
    // Don't modify crates, work on copy
    for (int i = 0; i < crates.Count; i++)
    {
      cratesCopy.Add(new List<char>());
      for (int j = 0; j < crates[i].Count; j++) cratesCopy[i].Add(crates[i][j]);
    }
    indexCopy = index;
    while (indexCopy < input.Length)
    {
      if (string.IsNullOrEmpty(input[index])) continue;

      indexof = input[indexCopy].IndexOf("move") + "move ".Length;
      count = int.Parse(input[indexCopy].Substring(indexof, input[indexCopy].IndexOf(' ', indexof) - indexof));
      indexof = input[indexCopy].IndexOf("from") + "from ".Length;
      from = int.Parse(input[indexCopy].Substring(indexof, input[indexCopy].IndexOf(' ', indexof) - indexof));
      indexof = input[indexCopy].IndexOf("to") + "to ".Length;
      to = int.Parse(input[indexCopy].Substring(indexof));

      // Move crates count amount of times
      for (int i = count - 1; i >= 0; i--)
      {
        cratesCopy[to - 1].Add(cratesCopy[from - 1][cratesCopy[from - 1].Count - 1 - i]);
      }
      cratesCopy[from - 1].RemoveRange(cratesCopy[from - 1].Count - count, count);

      indexCopy++;
    }
    Console.Write("Part two answer -> On top of each stack there are crates: ");
    for (int i = 0; i < cratesCopy.Count; i++) Console.Write(cratesCopy[i][^1]);
    Console.WriteLine();
  }
}