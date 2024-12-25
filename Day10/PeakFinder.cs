using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day10;

public static class PeakFinder
{
    private static readonly char[] numbers = Enumerable.Range(0, 10)
                                                       .Select(n => (char)(n + '0'))
                                                       .ToArray();
    public static int PartOne(string[] input)
    {
        IEnumerable<Coord> trailheads = input.Index().SelectMany(row => row.Item.Index()
                                                                                .Where(col => col.Item is '0')
                                                                                .Select(col => new Coord(row.Index, col.Index)));

        char[,] map = GridExtensions.New2DGridWithDimensions<char>(input, out int maxRow, out int maxCol);

        Dictionary<Coord, HashSet<Coord>> trailheadRoutes = [];
        Span2D<char> mapSpan = map;
        Queue<(Coord, Coord)> trails = [];

        foreach (var th in trailheads)
        {
            trails.Enqueue((th, th));
            trailheadRoutes[th] = [th];
        }

        int score = 0;
        while (trails.Count is not 0)
        {
            var (currentCoord, origin) = trails.Dequeue();

            char currentDigit = mapSpan[currentCoord.Row, currentCoord.Col];
            if (currentDigit is '9')
            {
                score++;
                continue;
            }

            int index = Array.IndexOf(numbers, currentDigit);

            char targetDigit = numbers[index + 1];
            foreach (var nb in currentCoord.Neighbours.Where(n => WithinBounds(n, maxRow, maxCol)))
            {
                if (mapSpan[nb.Row, nb.Col] == targetDigit && trailheadRoutes[origin].Add(nb))
                {
                    trails.Enqueue((nb, origin));
                }
            }
        }

        return score;
    }

    public static int PartTwo(string[] input)
    {
        IEnumerable<Coord> trailheads =
            input.Index().SelectMany(row => row.Item.Index()
                                                    .Where(col => col.Item is '0')
                                                    .Select(col => new Coord(row.Index, col.Index)));

        char[,] map = GridExtensions.New2DGridWithDimensions<char>(input, out int maxRow, out int maxCol);

        Span2D<char> mapSpan = map;
        Queue<(Coord, Coord)> trails = [];

        foreach (var th in trailheads)
        {
            trails.Enqueue((th, th));
        }

        int score = 0;
        while (trails.Count is not 0)
        {
            var (currentCoord, origin) = trails.Dequeue();

            char currentDigit = mapSpan[currentCoord.Row, currentCoord.Col];
            if (currentDigit is '9')
            {
                score++;
                continue;
            }

            int index = Array.IndexOf(numbers, currentDigit);
            char targetDigit = numbers[index + 1];
            foreach (var nb in currentCoord.Neighbours.Where(n => WithinBounds(n, maxRow, maxCol)))
            {
                if (mapSpan[nb.Row, nb.Col] == targetDigit)
                {
                    trails.Enqueue((nb, origin));
                }
            }
        }

        return score;
    }

    private static bool WithinBounds(Coord c, int maxRow, int maxCol) => c.Row >= 0 && c.Row < maxRow
                                                                       && c.Col >= 0 && c.Col < maxCol;
}
