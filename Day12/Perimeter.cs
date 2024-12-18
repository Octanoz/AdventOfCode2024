using System.Transactions;
using AdventUtilities;

namespace Day12;

public class Perimeter(Coord firstCoord)
{
    public char Id { get; private set; }
    public HashSet<Coord> Region { get; private set; } = [firstCoord];
    public int FencePrice { get; private set; }
    public bool HasExpanded { get; private set; } = false;

    public void ExpandRegion(char[][] map)
    {
        int maxRow = map.Length;
        int maxCol = map[0].Length;
        char targetLetter = map[firstCoord.Row][firstCoord.Col];
        Id = targetLetter;

        Queue<Coord> queue = [];
        queue.Enqueue(firstCoord);
        while (queue.Count is not 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbour in current.Neighbours.Where(nb => WithinBounds(nb, maxRow, maxCol)))
            {
                if (map[neighbour.Row][neighbour.Col] == targetLetter && Region.Add(neighbour))
                {
                    queue.Enqueue(neighbour);
                }
            }
        }

        HasExpanded = true;
        CalculateFencePrice();
    }

    private void CalculateFencePrice()
    {
        if (!HasExpanded)
            return;

        Dictionary<int, List<Coord>> rows = [];

        int regionValue = Region.Count;

        int totalPerimeter = regionValue * 4;
        foreach (var cell in Region)
        {
            foreach (var neighbour in cell.Neighbours.Where(nb => Region.Contains(nb)))
            {
                totalPerimeter--;
            }
        }

        FencePrice = regionValue * totalPerimeter;
    }

    private static bool WithinBounds(Coord c, int maxRow, int maxCol) => WithinBounds(c.Row, c.Col, maxRow, maxCol);

    private static bool WithinBounds(int a, int b, int maxA, int maxB) => a >= 0 && a < maxA
                                                                       && b >= 0 && b < maxB;
}
