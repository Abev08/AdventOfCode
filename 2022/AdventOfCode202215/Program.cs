internal class Program
{
  private static void Main(string[] args)
  {
    string[] input = File.ReadAllLines(@"input.txt");

    // Parse input
    List<Sensor> sensors = new();
    Vec2 min = new Vec2(long.MaxValue), max = new Vec2(long.MinValue);
    for (int i = 0; i < input.Length; i++)
    {
      if (string.IsNullOrEmpty(input[i])) continue;
      sensors.Add(new Sensor(input[i]));
      // Get grid size
      if (sensors[^1].Position.X < min.X) min.X = sensors[^1].Position.X;
      if (sensors[^1].Position.X > max.X) max.X = sensors[^1].Position.X;
      if (sensors[^1].Position.Y < min.Y) min.Y = sensors[^1].Position.Y;
      if (sensors[^1].Position.Y > max.Y) max.Y = sensors[^1].Position.Y;
      if (sensors[^1].Beacon.X < min.X) min.X = sensors[^1].Beacon.X;
      if (sensors[^1].Beacon.X > max.X) max.X = sensors[^1].Beacon.X;
      if (sensors[^1].Beacon.Y < min.Y) min.Y = sensors[^1].Beacon.Y;
      if (sensors[^1].Beacon.Y > max.Y) max.Y = sensors[^1].Beacon.Y;
    }

    // Part one
    long notPossibleBeaconPositions = 0;
    Dictionary<long, byte> rowMap = new(); // 1 == sensor, 2 == beacon, 3 == scanned area
    long row = 2000000; // input1 = 10, input = 2000000
    for (int i = 0; i < sensors.Count; i++)
    {
      if (sensors[i].Position.Y == row)
      {
        if (!rowMap.TryGetValue((sensors[i].Position.X), out _)) rowMap.Add((sensors[i].Position.X), 1);
      }
      if (sensors[i].Beacon.Y == row)
      {
        if (!rowMap.TryGetValue((sensors[i].Beacon.X), out _)) rowMap.Add((sensors[i].Beacon.X), 2);
      }
      ScanArea(sensors[i], rowMap, row, min);
    }
    foreach (var item in rowMap.AsEnumerable()) if (item.Value == 3) notPossibleBeaconPositions++;
    Console.WriteLine($"Part one answer -> Amount of positions at row {row} that distress beacon can't be present: " + notPossibleBeaconPositions);

    // Part two
    long maxPosition = 4000000;
  }

  static void ScanArea(Sensor sensor, Dictionary<long, byte> rowMap, long row, Vec2 minPos)
  {
    long distance = Math.Abs((sensor.Position.X - sensor.Beacon.X)) + Math.Abs((sensor.Position.Y - sensor.Beacon.Y));
    if (distance < Math.Abs(row - sensor.Position.Y)) return; // The row can't be reached by sensor

    Vec2 scanPos;
    scanPos.Y = row - sensor.Position.Y;
    scanPos.X = distance - Math.Abs(scanPos.Y);
    if (Math.Abs(scanPos.X) + Math.Abs(scanPos.Y) > distance) throw new Exception("Distance calculation not working");

    while (scanPos.X >= 0)
    {
      if (Math.Abs(scanPos.X) + Math.Abs(scanPos.Y) > distance) throw new Exception("Distance calculation not working");
      if (!rowMap.TryGetValue(sensor.Position.X - scanPos.X - minPos.X, out _)) rowMap.Add((sensor.Position.X - scanPos.X - minPos.X), 3);
      if (!rowMap.TryGetValue(sensor.Position.X + scanPos.X - minPos.X, out _)) rowMap.Add((sensor.Position.X + scanPos.X - minPos.X), 3);
      scanPos.X--;
    }
  }

  class Sensor
  {
    public Vec2 Position;
    public Vec2 Beacon;

    public Sensor(string input)
    {
      int index = input.IndexOf("x=") + "x=".Length;
      Position.X = int.Parse(input.Substring(index, input.IndexOf(',', index) - index));
      index = input.IndexOf("y=", index) + "y=".Length;
      Position.Y = int.Parse(input.Substring(index, input.IndexOf(':', index) - index));
      index = input.IndexOf("x=", index) + "x=".Length;
      Beacon.X = int.Parse(input.Substring(index, input.IndexOf(',', index) - index));
      index = input.IndexOf("y=", index) + "y=".Length;
      Beacon.Y = int.Parse(input.Substring(index));
    }
  }

  struct Vec2
  {
    public long X, Y;
    public Vec2(long value) { X = Y = value; }
    public override string ToString() { return $"{X}, {Y}"; }
  }
}