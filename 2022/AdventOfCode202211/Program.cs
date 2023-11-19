internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Read input
    List<Monkey> monkeys = new();
    monkeys.Add(new Monkey());
    Monkey? currentMonkey = null;
    int index;
    for (int i = 0; i < input.Length; i++)
    {
      if (input[i].StartsWith("Monkey"))
      {
        // Change current monkey
        index = int.Parse(input[i][input[i].IndexOf(" ") + 1] + "");
        while (monkeys.Count < index + 1) monkeys.Add(new Monkey());
        currentMonkey = monkeys[index];
      }
      else if (input[i].Trim().StartsWith("Starting"))
      {
        currentMonkey.SetItems(input[i].Substring(input[i].IndexOf(':') + 1).Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
      }
      else if (input[i].Trim().StartsWith("Operation"))
      {
        if (input[i].Contains('+'))
        {
          if (input[i].Substring(input[i].LastIndexOf(' ')).Contains("old")) currentMonkey.Operation = (arg) => arg + arg;
          else
          {
            int value = int.Parse(input[i].Substring(input[i].LastIndexOf(' ')));
            currentMonkey.Operation = (arg) => arg + value;
          }
        }
        else if (input[i].Contains('*'))
        {
          if (input[i].Substring(input[i].LastIndexOf(' ')).Contains("old")) currentMonkey.Operation = (arg) => arg * arg;
          else
          {
            int value = int.Parse(input[i].Substring(input[i].LastIndexOf(' ')));
            currentMonkey.Operation = (arg) => arg * value;
          }
        }
      }
      else if (input[i].Trim().StartsWith("Test"))
      {
        currentMonkey.Test = int.Parse(input[i].Substring(input[i].LastIndexOf(' ')));
      }
      else if (input[i].Trim().StartsWith("If true"))
      {
        currentMonkey.OnTestTrue = int.Parse(input[i].Substring(input[i].LastIndexOf(' ')));
      }
      else if (input[i].Trim().StartsWith("If false"))
      {
        currentMonkey.OnTestFlase = int.Parse(input[i].Substring(input[i].LastIndexOf(' ')));
      }
    }
    List<Monkey> monkeysClone = new();
    foreach (Monkey monkey in monkeys) monkeysClone.Add(monkey.Clone());

    // Part one, I looked for tips for this part. Couldn't figure out how to keep values in managable range.
    for (int round = 0; round < 20; round++)
    {
      for (int i = 0; i < monkeys.Count; i++)
      {
        currentMonkey = monkeys[i];
        while (currentMonkey.Items.Count > 0)
        {
          currentMonkey.Items[0] = currentMonkey.Operation(currentMonkey.Items[0]) / 3;
          if (currentMonkey.Items[0] % currentMonkey.Test == 0) monkeys[currentMonkey.OnTestTrue].Items.Add(currentMonkey.Items[0]);
          else monkeys[currentMonkey.OnTestFlase].Items.Add(currentMonkey.Items[0]);
          currentMonkey.Items.RemoveAt(0);
          currentMonkey.InspectedItems++;
        }
      }
    }
    Monkey monkey1;
    currentMonkey = monkeys[0];
    foreach (Monkey monkey in monkeys)
    {
      if (monkey.InspectedItems > currentMonkey.InspectedItems) currentMonkey = monkey;
    }
    monkey1 = currentMonkey;
    currentMonkey = monkeys[0];
    foreach (Monkey monkey in monkeys)
    {
      if (monkey == monkey1) continue;
      if (monkey.InspectedItems > currentMonkey.InspectedItems) currentMonkey = monkey;
    }
    Console.WriteLine("Part one answer -> The level of monkey bussines after 20 rounds is " + (monkey1.InspectedItems * currentMonkey.InspectedItems));

    // Part two
    long divider = FindDivider(monkeysClone);
    for (int round = 0; round < 10000; round++)
    {
      for (int i = 0; i < monkeysClone.Count; i++)
      {
        currentMonkey = monkeysClone[i];
        while (currentMonkey.Items.Count > 0)
        {
          currentMonkey.Items[0] = currentMonkey.Operation(currentMonkey.Items[0]) % divider;
          if (currentMonkey.Items[0] % currentMonkey.Test == 0) monkeysClone[currentMonkey.OnTestTrue].Items.Add(currentMonkey.Items[0]);
          else monkeysClone[currentMonkey.OnTestFlase].Items.Add(currentMonkey.Items[0]);
          currentMonkey.Items.RemoveAt(0);
          currentMonkey.InspectedItems++;
        }
      }
    }
    currentMonkey = monkeysClone[0];
    foreach (Monkey monkey in monkeysClone)
    {
      if (monkey.InspectedItems > currentMonkey.InspectedItems) currentMonkey = monkey;
    }
    monkey1 = currentMonkey;
    currentMonkey = null;
    foreach (Monkey monkey in monkeysClone)
    {
      if (monkey == monkey1) continue;
      if (currentMonkey is null) currentMonkey = monkey;
      if (monkey.InspectedItems > currentMonkey.InspectedItems) currentMonkey = monkey;
    }
    Console.WriteLine("Part two answer -> The level of monkey bussines after 10000 rounds is " + ((long)monkey1.InspectedItems * (long)currentMonkey.InspectedItems));
  }

  static long FindDivider(List<Monkey> monkeys)
  {
    long num = 1;
    foreach (Monkey monkey in monkeys) num *= monkey.Test;
    return num;
  }

  class Monkey
  {
    public List<long> Items = new();
    public Func<long, long> Operation = (arg) => arg * 2;
    /// <summary> Divisable by number </summary>
    public int Test = 0;
    /// <summary> On test true throw to monkey number: </summary>
    public int OnTestTrue = 0;
    /// <summary> On test false throw to monkey number: </summary>
    public int OnTestFlase = 0;
    public int InspectedItems = 0;

    public Monkey Clone()
    {
      Monkey newMonkey = new();
      foreach (int item in Items) newMonkey.Items.Add(item);
      newMonkey.Operation = Operation;
      newMonkey.Test = Test;
      newMonkey.OnTestTrue = OnTestTrue;
      newMonkey.OnTestFlase = OnTestFlase;
      newMonkey.InspectedItems = InspectedItems;
      return newMonkey;
    }

    public void SetItems(List<string> items)
    {
      Items.Clear();
      for (int i = 0; i < items.Count; i++) Items.Add(int.Parse(items[i]));
    }
  }
}