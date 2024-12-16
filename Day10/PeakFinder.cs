using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day10;

public class PeakFinder
{
    public static int PartOne(string[] input)
    {
        IEnumerable<Coord> trailheads = input.Index().SelectMany(row => row.Item.Index()
                                                                                .Where(col => col.Item is '0')
                                                                                .Select(col => new Coord(row.Index, col.Index)));

        char[,] map = GridExtensions.New2DGridWithDimensions<char>(input, out int maxRow, out int maxCol);

        Span2D<char> mapSpan = map;


    }
}
