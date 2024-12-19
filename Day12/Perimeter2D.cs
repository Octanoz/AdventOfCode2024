using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day12;

public class Perimeter2D(Coord firstCoord)
{
    public char Id { get; private set; }
    public HashSet<Coord> Region { get; private set; } = [firstCoord];
    public List<Coord> OuterPerimeter { get; set; } = [];
    public List<Coord> Unconnected { get; set; } = [];
    public List<Perimeter2D> InnerRegions { get; private set; } = [];
    public bool HasExpanded { get; private set; } = false;
    public int Sides { get; private set; } = 0;

    public void ExpandRegion(Span2D<char> map)
    {
        var (maxRow, maxCol) = (map.Height, map.Width);
        char targetLetter = map[firstCoord.Row, firstCoord.Col];
        Id = targetLetter;

        Queue<Coord> queue = [];
        queue.Enqueue(firstCoord);
        while (queue.Count is not 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbour in current.Neighbours)
            {
                if (!WithinBounds(neighbour, maxRow, maxCol) || map[neighbour.Row, neighbour.Col] != targetLetter)
                {
                    Unconnected.Add(current);
                }
                else if (Region.Add(neighbour))
                {
                    queue.Enqueue(neighbour);
                }
            }
        }

        HasExpanded = true;
        SetOuterPerimeter();
        FindInnerRegions(map);
    }

    private void SetOuterPerimeter()
    {
        if (!HasExpanded)
            return;

        if (Region.Count is 1)
        {
            OuterPerimeter = new(Region);
            return;
        }

        foreach (var pillar in Unconnected.Where(coord => coord.Neighbours.Count(nb => Region.Contains(nb)) is 1))
        {
            Sides += 3;

            OuterPerimeter.Add(pillar);
            Unconnected.Remove(pillar);
            Coord current = pillar.Neighbours.First(Unconnected.Contains);

            while (current.Neighbours.Count(Unconnected.Contains) is 1)
            {
                var previous = current;
                OuterPerimeter.Add(previous);
                Unconnected.Remove(previous);
                current = previous.Neighbours.First(Unconnected.Contains);
            }

            if (current.Neighbours.Count(Unconnected.Contains) is 0)
            {
                Sides++;
                OuterPerimeter.Add(current);
                Unconnected.Remove(current);
            }
        }

        foreach (var corner in Unconnected.Where(coord => coord.Neighbours.Count(nb => Region.Contains(nb)) is 2))
        {
            OuterPerimeter.Add(corner);
            foreach (var element in corner.Neighbours.Index().Where(elem => Region.Contains(elem.Item)))
            {
                Sides++;
                OuterPerimeter.Add(element.Item);
                AddIfWithinRegion(element.Item, (Direction)element.Index);
            }
        }

        OuterPerimeter = OuterPerimeter.Distinct().ToList();
        Unconnected = Unconnected.Except(OuterPerimeter).ToList();
    }

    private void AddIfWithinRegion(Coord current, Direction direction)
    {
        if (current.Neighbours.All(Region.Contains))
        {
            OuterPerimeter.Remove(current);
            return;
        }

        Coord[] neighbours = current.Neighbours.ToArray();
        if (Region.Contains(neighbours[(int)direction]))
        {
            Coord neighbour = neighbours[(int)direction];
            OuterPerimeter.Add(neighbour);
            AddIfWithinRegion(neighbour, direction);
        }
        else if (neighbours.Count(nb => Region.Contains(nb)) >= 2)
        {
            Sides--;
        }
    }

    private void FindInnerRegions(Span2D<char> map)
    {
        if (Unconnected.Count < 4)
            return;

        List<Coord> missingNeighbours = Unconnected.SelectMany(c => c.Neighbours.Where(n => !Region.Contains(n))).ToList();
        HashSet<Coord> visited = [];

        foreach (var coord in missingNeighbours.Where(c => !visited.Contains(c)))
        {
            if (coord.Neighbours.All(nb => Unconnected.Contains(nb)))
            {
                Perimeter2D innerPerimeter = new(coord);
                innerPerimeter.ExpandRegion(map);
                InnerRegions.Add(innerPerimeter);
                visited.Add(coord);

                Unconnected.RemoveAll(c => coord.Neighbours.Contains(c));
                Sides += 4;

                continue;
            }

            if (FormsLoop(coord, missingNeighbours, visited))
            {
                Perimeter2D innerPerimeter = new(coord);
                innerPerimeter.ExpandRegion(map);
                InnerRegions.Add(innerPerimeter);
                Sides += innerPerimeter.Sides;

                var touchingCoords = innerPerimeter.OuterPerimeter.SelectMany(c => c.Neighbours.Where(nb => !innerPerimeter.Region.Contains(nb)));

                foreach (var element in touchingCoords)
                {
                    Unconnected.Remove(element);
                }
            }
        }

        if (Unconnected.Count is not 0)
        {
            Sides += CountSides();
        }
    }

    private int CountSides()
    {
        int sides = 0;
        HashSet<Coord> visited = [];
        Stack<Coord> stack = [];
        foreach (var coord in Unconnected.Where(c => !visited.Contains(c)))
        {
            sides++;
            stack.Push(coord);

            while (stack.Count is not 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current))
                    continue;

                foreach (var neighbour in current.Neighbours.Where(nb => Unconnected.Contains(nb)))
                {
                    stack.Push(neighbour);
                }
            }
        }

        if (sides is 1)
        {
            throw new InvalidDataException($"Edges of the region not belonging to an inner region, if any, should be greater than 1");
        }

        return sides;
    }

    private static bool FormsLoop(Coord start, List<Coord> missing, HashSet<Coord> visited)
    {
        Stack<Coord> stack = [];
        foreach (var neighbour in start.Neighbours.Where(nb => missing.Contains(nb)))
        {
            stack.Push(neighbour);
        }

        visited.Add(start);

        while (stack.Count is not 0)
        {
            var current = stack.Pop();

            if (current == start)
                return true;

            if (!visited.Add(current))
                continue;

            foreach (var neighbour in current.Neighbours.Where(nb => missing.Contains(nb)))
            {
                stack.Push(neighbour);
            }
        }

        return false;
    }

    public static bool WithinBounds(Coord c, int maxRow, int maxCol) => c.Row >= 0 && c.Row < maxRow
                                                                      && c.Col >= 0 && c.Col < maxCol;

    public static bool WithinBounds(Coord c, Span2D<char> map) => c.Row >= 0 && c.Row < map.Height
                                                                && c.Col >= 0 && c.Col < map.Width;


}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
