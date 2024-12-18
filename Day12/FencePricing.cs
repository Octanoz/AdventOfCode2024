using AdventUtilities;

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
}
