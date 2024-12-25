using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day12;

public class Perimeter2D(Coord firstCoord)
{
    public char Id { get; private set; }
    public HashSet<Coord> Region { get; private set; } = [firstCoord];
    public List<Coord> Outline { get; set; } = [];
    public bool HasExpanded { get; private set; } = false;
    public int Sides { get; private set; } = 0;

    public void ExpandRegion(Span2D<char> map)
    {
        var (maxRow, maxCol) = (map.Height, map.Width);
        char targetLetter = map[firstCoord.Row, firstCoord.Col];
        Id = targetLetter;

        HashSet<(Coord, Coord)> edges = [];
        Queue<Coord> queue = [];
        queue.Enqueue(firstCoord);
        while (queue.Count is not 0)
        {
            var current = queue.Dequeue();
            foreach (var neighbour in current.Neighbours.Where(nb => !queue.Contains(nb)))
            {
                if (!WithinBounds(neighbour, maxRow, maxCol) || map[neighbour.Row, neighbour.Col] != targetLetter)
                {
                    edges.Add((current, neighbour));
                }
                else if (Region.Add(neighbour))
                {
                    queue.Enqueue(neighbour);
                }
            }
        }

        HasExpanded = true;
        ProcessEdges(edges, map);
    }

    private void ProcessEdges(HashSet<(Coord start, Coord end)> edges, Span2D<char> map)
    {
        if (!HasExpanded)
            return;

        Outline = edges.SelectMany(edge => (Coord[])[edge.start, edge.end]).Distinct().ToList();
        Sides = CalculateSides(map, edges);
    }

    public int CalculateSides(Span2D<char> map, HashSet<(Coord start, Coord end)> edges)
    {
        HashSet<Coord> visited = [];

        //Pillar is single cell with just one neighbour in the region list, aka perfect starting point
        if (!IsPillar(firstCoord) && Region.Any(IsPillar))
        {
            firstCoord = Region.Where(IsPillar).OrderBy(c => c.Row).First();
        }

        var (dir, sides, travelClockwise) = InitializeTravel(firstCoord);

        Coord current = firstCoord;
        while (true)
        {
            visited.Add(current);

            if (current == firstCoord && sides >= 4)
                break;

            if (PreferredCoordAvailable(current, dir, travelClockwise))
            {
                sides++;
                dir = DirectionForPreferredPath(dir, travelClockwise);
            }

            Coord next = NextCoord(current, dir);
            if (!Region.Contains(next))
            {
                sides++;
                dir = DirectionForBlockedPath(dir, travelClockwise);

                continue;
            }

            current = next;
        }

        edges = CleanEdges(edges); //Remove all coordinates outside the region

        foreach (var (_, end) in edges.Where(edge => !visited.Contains(edge.start)).OrderByDescending(e => edges.Count(x => x.end == e.end)))
        {
            sides += FindInnerSides(map, end, visited);
        }

        return sides;
    }

    private int FindInnerSides(Span2D<char> map, Coord current, HashSet<Coord> visited)
    {
        if (!visited.Add(current))
            return 0;

        //single cell
        if (current.Neighbours.All(Region.Contains))
        {
            return 4;
        }

        //Navigate to a corner to ensure the expand process is successful
        while (!Region.Contains(current.Up) && !Region.Contains(current.Left))
        {
            if (!Region.Contains(current.Up))
            {
                current = current.Up;
            }

            if (!Region.Contains(current.Left))
            {
                current = current.Left;
            }
        }

        Perimeter2D innerPerimeter = new(current);
        innerPerimeter.ExpandRegion(map);
        visited.UnionWith(innerPerimeter.Region);

        if (!innerPerimeter.Outline.All(coord => coord.Neighbours.Any(nb => Region.Contains(nb))))
        {
            return 0;
        }

        return innerPerimeter.Sides;
    }

    private static Direction DirectionForPreferredPath(Direction dir, bool clockwise) =>
        clockwise
            ? (Direction)(((int)dir + 3) & 3)
            : (Direction)(((int)dir + 1) & 3);

    private static Direction DirectionForBlockedPath(Direction dir, bool clockwise) =>
        clockwise
            ? (Direction)(((int)dir + 1) & 3)
            : (Direction)(((int)dir + 3) & 3);

    private bool PreferredCoordAvailable(Coord coord, Direction travelling, bool travelClockwise = false) => travelClockwise switch
    {
        false => travelling switch
        {
            Direction.Up when Region.Contains(coord.Right) => true,
            Direction.Right when Region.Contains(coord.Down) => true,
            Direction.Down when Region.Contains(coord.Left) => true,
            Direction.Left when Region.Contains(coord.Up) => true,
            _ => false
        },

        true => travelling switch
        {
            Direction.Up when Region.Contains(coord.Left) => true,
            Direction.Right when Region.Contains(coord.Up) => true,
            Direction.Down when Region.Contains(coord.Right) => true,
            Direction.Left when Region.Contains(coord.Down) => true,
            _ => false
        }
    };

    private static Coord NextCoord(Coord coord, Direction travelling) => travelling switch
    {
        Direction.Up => coord.Up,
        Direction.Right => coord.Right,
        Direction.Down => coord.Down,
        _ => coord.Left
    };

    private (Direction direction, int initialSides, bool travelClockwise) InitializeTravel(Coord start)
    {
        if (IsPillar(start))
        {
            int nbIndex = start.Neighbours.Index().First(elem => Region.Contains(elem.Item)).Index;
            Direction direction = (Direction)nbIndex;

            bool travelClockwise = direction is Direction.Up or Direction.Left;
            int initialSides = 2;

            return (direction, initialSides, travelClockwise);
        }

        return (Direction.Down, 1, false);
    }

    private HashSet<(Coord, Coord)> CleanEdges(HashSet<(Coord start, Coord end)> edges)
    {
        edges.RemoveWhere(edge => edge.end.Row < Region.Min(c => c.Row)
                               || edge.end.Row > Region.Max(c => c.Row)
                               || edge.end.Col < Region.Min(c => c.Col)
                               || edge.end.Col > Region.Max(c => c.Col));

        return edges;
    }

    private bool IsPillar(Coord coord) => coord.Neighbours.Count(Region.Contains) == 1;
    public static bool WithinBounds(Coord c, int maxRow, int maxCol) => c.Row >= 0 && c.Row < maxRow
                                                                     && c.Col >= 0 && c.Col < maxCol;
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
