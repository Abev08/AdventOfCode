internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Create file structure
    Element root = new Element() { Name = "root" };
    Element? currentElement = null;
    int indexOf;
    string commandArg;
    for (int i = 0; i < input.Length; i++)
    {
      // Command
      if (input[i].StartsWith('$'))
      {
        // Look for "cd" command
        indexOf = input[i].IndexOf("$ cd ");
        if (indexOf >= 0)
        {
          indexOf += "$ cd ".Length;
          commandArg = input[i].Substring(indexOf);

          if (commandArg == "/") currentElement = root;
          else if (commandArg == "..")
          {
            if (currentElement is null) throw new Exception("Something went wrong. Current element is null");
            if (currentElement.Parent is null) throw new Exception("Something went wrong. Current element parent is null");
            currentElement = currentElement.Parent;
          }
          else
          {
            if (currentElement is null) throw new Exception("Something went wrong. Current element is null");
            currentElement = currentElement.GetElementWithName(commandArg);
            if (currentElement is null) throw new Exception("Something went wrong. Directory not found");
          }
        }
        // Other commands, actually not needed to do anything
      }
      // Output
      else
      {
        if (input[i].StartsWith("dir"))
        {
          // Directory found
          if (currentElement is null) throw new Exception("Something went wrong. Current element is null");
          indexOf = input[i].IndexOf(' ') + 1;
          currentElement.AddDirectory(input[i].Substring(indexOf));
        }
        else
        {
          // File found
          if (currentElement is null) throw new Exception("Something went wrong. Current element is null");
          indexOf = input[i].IndexOf(' ');
          currentElement.AddFile(input[i].Substring(indexOf + 1), int.Parse(input[i].Substring(0, indexOf)));
        }
      }
    }

    // Part one
    int maxSize = 100000;
    List<int> directoriesSizes = new();
    CalculateSize(root, directoriesSizes, maxSize);
    int sumOfSizes = 0;
    for (int i = 0; i < directoriesSizes.Count; i++) sumOfSizes += directoriesSizes[i];
    Console.WriteLine($"Part one answer -> Sum of sizes of directories with total size less than {maxSize} is {sumOfSizes}");

    // Part two
    int totalDiskSpace = 70000000;
    int requiredSpace = 30000000;
    int missingSpace = -((totalDiskSpace - root.DirectorySize.Value) - requiredSpace);
    currentElement = root;
    FindRequiredDirectory(ref currentElement, root, missingSpace);
    Console.WriteLine($"Part two answer -> Size of smallest directory that would be enough for update {currentElement.DirectorySize}");
  }

  static void FindRequiredDirectory(ref Element smallestDirectory, Element directory, int missingSpace)
  {
    for (int i = 0; i < directory.Elements.Count; i++)
    {
      if (directory.Elements[i].DirectorySize.HasValue)
      {
        // If DirectorySize has a value then it's a directory. Check if it's size is smaller than smallestDirectory
        if (directory.Elements[i].DirectorySize > missingSpace)
        {
          // Check for internal directories
          if (directory.Elements[i].DirectorySize < smallestDirectory.DirectorySize) smallestDirectory = directory.Elements[i];
          FindRequiredDirectory(ref smallestDirectory, directory.Elements[i], missingSpace);
        }
      }
    }
  }

  static int CalculateSize(Element directory, List<int> sizes, int maxSize)
  {
    int size = 0;

    for (int i = 0; i < directory.Elements.Count; i++)
    {
      if (directory.Elements[i].Size.HasValue == false)
      {
        // It's direcotry, calculate it's size
        size += CalculateSize(directory.Elements[i], sizes, maxSize);
      }
      else size += directory.Elements[i].Size.Value;
    }

    // If reached this point it was directory. Check it's size and add to sizes if less than maxSize
    if (size < maxSize) sizes.Add(size);
    directory.DirectorySize = size;

    return size;
  }
}


class Element
{
  public Element? Parent;
  public List<Element> Elements;
  public int? Size;
  public string Name;
  public int? DirectorySize;

  public Element()
  {
    Elements = new();
    Size = null;
    Name = string.Empty;
    DirectorySize = null;
  }

  public Element? GetElementWithName(string name)
  {
    for (int i = 0; i < Elements.Count; i++)
    {
      if (Elements[i].Name == name) return Elements[i];
    }

    return null;
  }

  public void AddDirectory(string name)
  {
    // Check if directory already exist
    if (GetElementWithName(name) is null)
    {
      // Directory doesn't exist - add new one
      Elements.Add(new Element() { Name = name, Parent = this });
    }
  }

  public void AddFile(string name, int size)
  {
    // Check if file already exist
    if (GetElementWithName(name) is null)
    {
      // File doesn't exist - add new one
      Elements.Add(new Element() { Name = name, Parent = this, Size = size });
    }
  }
}