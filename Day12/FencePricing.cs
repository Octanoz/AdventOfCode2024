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

        int totalPrice = 0;
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

                totalPrice += AddToTotal(currentPerimeter, visited);
            }
        }

        return totalPrice;
    }

    private static int AddToTotal(Perimeter2D perimeter, HashSet<Coord> visited)
    {
        int price = perimeter.Sides * perimeter.Region.Count;
        Console.WriteLine($"{perimeter.Id}: {perimeter.Region.Count} x {perimeter.Sides}\n");
        visited.UnionWith(perimeter.Region);

        foreach (var innerRegion in GetInnerRegions(perimeter, visited))
        {
            price += innerRegion.Sides * innerRegion.Region.Count;
            Console.WriteLine($"{innerRegion.Id}: {innerRegion.Region.Count} x {innerRegion.Sides}\n");
        }

        return price;
    }

    private static IEnumerable<Perimeter2D> GetInnerRegions(Perimeter2D outer, HashSet<Coord> visited)
    {
        foreach (var inner in outer.InnerRegions)
        {
            foreach (var deeper in GetInnerRegions(inner, visited))
            {
                yield return deeper;
            }

            visited.UnionWith(inner.Region);
            yield return inner;
        }
    }
}
