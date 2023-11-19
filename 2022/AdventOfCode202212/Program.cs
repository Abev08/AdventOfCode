internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");
    // Parse input
    Node? start = null, end = null, end2 = null;
    Node[,] grid = new Node[input[0].Length, input.Length];
    Node[,] grid2 = new Node[input[0].Length, input.Length];
    for (int j = 0; j < input.Length; j++)
    {
      for (int i = 0; i < input[0].Length; i++)
      {
        grid[i, j] = new Node(i, j, input[j][i] - '`');
        grid2[i, j] = new Node(i, j, input[j][i] - '`');
        if (input[j][i] == 'S') start = grid[i, j];
        if (input[j][i] == 'E') { end = grid[i, j]; end2 = grid2[i, j]; }
      }
    }
    if (start is null || end is null) throw new Exception("Start or End not found");
    start.Height = 1;
    start.ValueFromStart = 0;
    end.Height = 'z' - '`';
    end2.Height = 'z' - '`';

    // Part one
    List<Node> nodesToSearch = new();
    nodesToSearch.Add(start);
    Node currentNode;
    while (nodesToSearch.Count > 0)
    {
      currentNode = nodesToSearch[0];
      nodesToSearch.RemoveAt(0);
      Search(currentNode, grid, nodesToSearch, end);
    }
    if (end.ShortestNode is null) throw new Exception("Path not found");
    // Calculate steps
    currentNode = end;
    int steps = 0;
    while (currentNode.ShortestNode != null)
    {
      currentNode = currentNode.ShortestNode;
      steps++;
    }
    PrintPath(end, input);
    Console.WriteLine("Part one answer -> Steps needed to reach the end: " + steps);

    // Part two, search the grid starting from end and then look through grid to find node with Height == 1 and smallest ValueFromStart (steps)
    nodesToSearch.Clear();
    nodesToSearch.Add(end2);
    end2.ValueFromStart = 0;
    while (nodesToSearch.Count > 0)
    {
      currentNode = nodesToSearch[0];
      nodesToSearch.RemoveAt(0);
      Search(currentNode, grid2, nodesToSearch, end2, true);
    }
    for (int j = 0; j < input.Length; j++)
    {
      for (int i = 0; i < input[0].Length; i++)
      {
        if (grid2[i, j].Height == 1)
        {
          if (grid2[i, j].ValueFromStart < steps)
          {
            steps = grid2[i, j].ValueFromStart;
            currentNode = grid2[i, j];
          }
        }
      }
    }
    PrintPath(currentNode, input);
    Console.WriteLine("Part two answer -> Steps needed to reach the end from any level \"a\": " + steps);
  }

  static void Search(Node currentNode, Node[,] grid, List<Node> nodesToSearch, Node end, bool partTwo = false)
  {
    int maxHeightDifference = -1;
    Node? nextNode = null;
    for (int i = 0; i < 4; i++)
    {
      nextNode = null;
      switch (i)
      {
        case 0: // Left
          if (currentNode.Position.X > 0) nextNode = grid[currentNode.Position.X - 1, currentNode.Position.Y];
          break;
        case 1: // Right
          if (currentNode.Position.X < grid.GetUpperBound(0)) nextNode = grid[currentNode.Position.X + 1, currentNode.Position.Y];
          break;
        case 2: // Top
          if (currentNode.Position.Y > 0) nextNode = grid[currentNode.Position.X, currentNode.Position.Y - 1];
          break;
        case 3: // Bottom
          if (currentNode.Position.Y < grid.GetUpperBound(1)) nextNode = grid[currentNode.Position.X, currentNode.Position.Y + 1];
          break;
      };
      if (nextNode is null) continue;
      if (!partTwo && currentNode.Height - nextNode.Height >= maxHeightDifference)
      {
        if (currentNode.ValueFromStart + 1 < nextNode.ValueFromStart)
        {
          nextNode.ShortestNode = currentNode;
          nextNode.ValueFromStart = currentNode.ValueFromStart + 1;
          AddNoteToSearch(nextNode, nodesToSearch);
        }
      }
      else if (partTwo && nextNode.Height - currentNode.Height >= maxHeightDifference) // In part two we look from end so height comparasion have to be swapped
      {
        if (currentNode.ValueFromStart + 1 < nextNode.ValueFromStart)
        {
          nextNode.ShortestNode = currentNode;
          nextNode.ValueFromStart = currentNode.ValueFromStart + 1;
          AddNoteToSearch(nextNode, nodesToSearch);
        }
      }
    }
  }

  static void AddNoteToSearch(Node newNode, List<Node> nodesToSearch)
  {
    if (nodesToSearch.Contains(newNode)) return; // Don't add the same node twice
    nodesToSearch.Add(newNode);
  }

  static void PrintPath(Node end, string[] input)
  {
    string[] arr = new string[input.Length];
    Array.Copy(input, arr, input.Length);
    Node currentNode = end;

    while (currentNode.ShortestNode != null)
    {
      arr[currentNode.ShortestNode.Position.Y] = arr[currentNode.ShortestNode.Position.Y].Remove(currentNode.ShortestNode.Position.X, 1);

      string symbol = ".";
      // if (currentNode.Position.X > currentNode.ShortestNode.Position.X) symbol = ">";
      // if (currentNode.Position.X < currentNode.ShortestNode.Position.X) symbol = "<";
      // if (currentNode.Position.Y > currentNode.ShortestNode.Position.Y) symbol = "v";
      // if (currentNode.Position.Y < currentNode.ShortestNode.Position.Y) symbol = "^";

      arr[currentNode.ShortestNode.Position.Y] = arr[currentNode.ShortestNode.Position.Y].Insert(currentNode.ShortestNode.Position.X, symbol);
      currentNode = currentNode.ShortestNode;
    }

    foreach (string s in arr) Console.WriteLine(s);
  }

  class Node
  {
    public int Height;
    public int ValueFromStart = int.MaxValue;
    public Node? ShortestNode = null;
    public Vector2 Position;

    public Node(int x, int y, int height)
    {
      Position = new Vector2(x, y);
      Height = height;
    }
  }

  class Vector2
  {
    public int X, Y;

    public Vector2(int x, int y) { X = x; Y = y; }

    public override string ToString() { return $"{X}, {Y}"; }
  }
}