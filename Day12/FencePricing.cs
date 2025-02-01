using AdventUtilities;

using CommunityToolkit.HighPerformance;

namespace Day12;

public static class FencePricing
{
    public static int PartOne(string[] input)
    {
        char[][] map = input.JaggedCharArray();
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
        char[,] inputMap = input.New2DGrid<char>();
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

        return price;
    }
}
