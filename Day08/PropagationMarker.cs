namespace Day08;

using AdventUtilities;

public class PropagationMarker(string[] input)
{
    private readonly string[] input = input;
    private Dictionary<char, List<Coord>> antennae = [];
    private readonly HashSet<Coord> antinodes = [];

    public int PartOne()
    {
        FindAntennae();
        FindAntinodes();

        return antinodes.Count;
    }

    public int PartTwo()
    {
        FindAntennae();
        FindAntinodes2();

        char[][] matrix = input.JaggedCharArray();
        foreach (var coord in antinodes)
        {
            matrix[coord.Row][coord.Col] = '#';
        }

        matrix.DrawJaggedGridTight();

        foreach (var antennaList in antennae.Values)
        {
            antennaList.ForEach(ant => antinodes.Add(ant));
        }

        return antinodes.Count;
    }

    public Dictionary<char, List<Coord>> FindAntennae() =>
        antennae = input.Index().SelectMany(row => row.Item.Index()
                                    .Select(col => (col.Item, new Coord(row.Index, col.Index)))
                                    .Where(t => t.Item is not '.'))
                                .GroupBy(t => t.Item)
                                .ToDictionary(g => g.Key, g => g.Select(g => g.Item2).ToList());

    private void FindAntinodes()
    {
        foreach (var antennaList in antennae.Values)
        {
            for (int i = 0; i < antennaList.Count; i++)
            {
                Coord current = antennaList[i];
                for (int j = i + 1; j < antennaList.Count; j++)
                {
                    Coord other = antennaList[j];
                    Coord diffCoord = current - other;

                    Coord before = diffCoord + current;
                    Coord after = other - diffCoord;

                    if (WithinBounds(before))
                        antinodes.Add(before);

                    if (WithinBounds(after))
                        antinodes.Add(after);
                }
            }
        }
    }

    private void FindAntinodes2()
    {
        foreach (var antennaList in antennae.Values)
        {
            for (int i = 0; i < antennaList.Count; i++)
            {
                Coord first = antennaList[i];
                for (int j = i + 1; j < antennaList.Count; j++)
                {
                    Coord second = antennaList[j];
                    Coord diffCoord = first - second;

                    Coord current = diffCoord + first;
                    while (WithinBounds(current))
                    {
                        antinodes.Add(current);
                        current = diffCoord + current;
                    }

                    current = second - diffCoord;
                    while (WithinBounds(current))
                    {
                        antinodes.Add(current);
                        current -= diffCoord;
                    }
                }
            }
        }
    }

    private bool WithinBounds(Coord c) => c.Row >= 0 && c.Row < input.Length
                                       && c.Col >= 0 && c.Col < input[0].Length;

    public int CountAntiNodes() => antinodes.Count;
}
