using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day12;

public static class FencePricing
{
    public static int PartOne(string[] input)
    {
        char[][] map = GridExtensions.JaggedCharArray(input);
        HashSet<Coord> visited = [];
        List<Perimeter> perimeters = [];

        int totalFencePrice = 0;
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                Coord current = new(row, col);
                if (visited.Add(current))
                {
                    Perimeter currentPerimeter = new(current);
                    perimeters.Add(currentPerimeter);
                    currentPerimeter.ExpandRegion(map);

                    visited.UnionWith(currentPerimeter.Region);
                }
            }
        }

        checked
        {
            perimeters.ForEach(p => totalFencePrice += p.FencePrice);
        }

        return totalFencePrice;
    }

    public static int PartTwo(string[] input)
    {
        char[,] inputMap = GridExtensions.New2DGrid<char>(input);
        Span2D<char> map = inputMap;
        HashSet<Coord> visited = [];
        List<Perimeter2D> perimeters = [];

        int totalSides = 0;
        for (int row = 0; row < map.Height; row++)
        {
            for (int col = 0; col < map.Width; col++)
            {
                Coord current = new(row, col);
                if (!visited.Add(current))
                    continue;

                Perimeter2D currentPerimeter = new(current);
                perimeters.Add(currentPerimeter);
                currentPerimeter.ExpandRegion(map);
                visited.UnionWith(currentPerimeter.Region);

                foreach (var innerRegion in GetInnerRegions(currentPerimeter))
                {
                    perimeters.Add(innerRegion);
                    visited.UnionWith(innerRegion.Region);
                    totalSides += innerRegion.Sides;
                }

                totalSides += currentPerimeter.Sides;
            }
        }

        return totalSides * 4;
    }

    private static IEnumerable<Perimeter2D> GetInnerRegions(Perimeter2D outer)
    {
        foreach (var innerRegion in outer.InnerRegions)
        {
            foreach (var inner in GetInnerRegions(innerRegion))
            {
                yield return inner;
            }

            yield return innerRegion;
        }
    }
}
