using System.Runtime.CompilerServices;
using AdventUtilities;
using Coord = AdventUtilities.DOF8.Coord;

namespace Day4;

public class WordFinder(string target = "")
{
    private readonly string target = target;

    public int FindTarget(Span<string> input)
    {
        char lastLetter = target[^1];
        int maxRow = input.Length;
        int maxCol = input[0].Length;
        int targetCounter = 0;

        Queue<(Coord, Direction)> finders = GetStartLetters(input, maxRow, maxCol);
        while (finders.Count is not 0)
        {
            var (current, currentDirection) = finders.Dequeue();

            char currentLetter = input[current.Row][current.Col];
            if (currentLetter == lastLetter)
            {
                targetCounter++;
                continue;
            }

            char targetLetter = target[target.IndexOf(currentLetter) + 1];
            Coord nextCoord = current.Neighbours.Skip((int)currentDirection - 1).First();
            if (WithinBounds(nextCoord, maxRow, maxCol) && input[nextCoord.Row][nextCoord.Col] == targetLetter)
            {
                finders.Enqueue((nextCoord, currentDirection));
            }
        }

        return targetCounter;
    }

    public static int FindCrosses(string[] input)
    {
        int maxRow = input.Length - 1;
        int maxCol = input[0].Length - 1;
        int targetCounter = 0;

        List<Coord> aList = input.Index().SelectMany(row => row.Item.Index()
                                                .Where(col => col.Item is 'A'
                                                    && WithinInner(row.Index, maxRow, col.Index, maxCol))
                                                .Select(col => new Coord(row.Index, col.Index)))
                                                .ToList();

        foreach (var aCoord in aList.Where(coord => IsCross(input, coord)))
        {
            targetCounter++;
        }

        return targetCounter;
    }

    private static bool IsCross(string[] input, Coord coord)
    {
        Coord[] nbArray = [coord.UpLeft, coord.UpRight, coord.DownLeft, coord.DownRight];
        Span<char> neighbourSpan = nbArray.Select(n => input[n.Row][n.Col]).ToArray();

        return neighbourSpan switch
        {
        ['M', 'M', 'S', 'S'] => true,
        ['M', 'S', 'M', 'S'] => true,
        ['S', 'S', 'M', 'M'] => true,
        ['S', 'M', 'S', 'M'] => true,
            _ => false
        };
    }

    private Queue<(Coord, Direction)> GetStartLetters(Span<string> input, int maxRow, int maxCol)
    {
        Queue<(Coord, Direction)> queue = [];
        for (int i = 0; i < input.Length; i++)
        {
            int index = input[i].IndexOf(target[0]);
            while (index is not -1)
            {
                Coord coord = new(i, index);
                ExploreAllDOF(input, coord, target[1]);
                index = input[i].IndexOf(target[0], index + 1);
            }
        }

        return queue;

        void ExploreAllDOF(Span<string> input, Coord coord, char targetLetter)
        {
            foreach (var neighbour in coord.Neighbours)
            {
                if (WithinBounds(neighbour, maxRow, maxCol) && input[neighbour.Row][neighbour.Col] == targetLetter)
                {
                    ReadOnlySpan<Coord> neighbourSpan = coord.Neighbours.ToArray();
                    Direction currentDirection = (Direction)(neighbourSpan.IndexOf(neighbour) + 1);
                    queue.Enqueue((neighbour, currentDirection));
                }
            }
        }
    }

    private static bool WithinBounds(Coord c, int maxRow, int maxCol) => c.Row >= 0 && c.Row < maxRow && c.Col >= 0 && c.Col < maxCol;

    private static bool WithinInner(int row, int maxRow, int col, int maxCol) => row > 0 && row < maxRow && col > 0 && col < maxCol;

    public enum Direction
    {
        None,
        UpLeft,
        Up,
        UpRight,
        Left,
        Right,
        DownLeft,
        Down,
        DownRight
    }
}
