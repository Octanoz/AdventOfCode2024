using AdventUtilities;

namespace Day12;

public class Perimeter2D(Coord firstCoord)
{
    public char Id { get; private set; }
    public HashSet<Coord> Region { get; private set; } = [firstCoord];
    public int FencePrice { get; private set; }
    public bool HasExpanded { get; private set; } = false;

    public void ExpandRegion(char[,] map)
    {
        int maxRow = map.GetLength(0);
        int maxCol = map.GetLength(1);
        char targetLetter = map[firstCoord.Row, firstCoord.Col];
        Id = targetLetter;

        Queue<Coord> queue = [];
        queue.Enqueue(firstCoord);
        while (queue.Count is not 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbour in current.Neighbours.Where(nb => WithinBounds(nb, maxRow, maxCol)))
            {
                if (map[neighbour.Row, neighbour.Col] == targetLetter && Region.Add(new(neighbour.Row, neighbour.Col)))
                {
                    queue.Enqueue(neighbour);
                }
            }
        }

        HasExpanded = true;
    }

    private static bool WithinBounds(Coord c, int maxRow, int maxCol) => WithinBounds(c.Row, c.Col, maxRow, maxCol);

    private static bool WithinBounds(int a, int b, int maxA, int maxB) => a >= 0 && a < maxA
                                                                       && b >= 0 && b < maxB;
}
