internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Part one
    string[] sectionOne, sectionTwo;
    int num = 0, num2 = 0;
    for (int i = 0; i < input.Length; i++)
    {
      sectionOne = input[i].Substring(0, input[i].IndexOf(',')).Split('-');
      sectionTwo = input[i].Substring(input[i].IndexOf(',') + 1).Split('-');
      if ((int.Parse(sectionOne[0]) <= int.Parse(sectionTwo[0])) && (int.Parse(sectionOne[1]) >= int.Parse(sectionTwo[1]))) num++; // Section one contained in section two
      else if ((int.Parse(sectionTwo[0]) <= int.Parse(sectionOne[0])) && (int.Parse(sectionTwo[1]) >= int.Parse(sectionOne[1]))) num++; // Section two contained in section one
      // Part two
      if ((int.Parse(sectionOne[0]) <= int.Parse(sectionTwo[1])) && (int.Parse(sectionOne[1]) >= int.Parse(sectionTwo[0]))) num2++; // Section one overlaped in section two
      else if ((int.Parse(sectionTwo[0]) <= int.Parse(sectionOne[1])) && (int.Parse(sectionTwo[1]) >= int.Parse(sectionOne[0]))) num2++; // Section two overlaped in section one
    }
    Console.WriteLine("Part one answer -> Assignment pairs that contains each other: " + num);

    // Part two
    Console.WriteLine("Part two answer -> Assignment pairs that overlap each other: " + num2);
  }
}