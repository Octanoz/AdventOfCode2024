using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day12;

public class Perimeter2D(Coord firstCoord)
{
    public char Id { get; private set; }
    public HashSet<Coord> Region { get; private set; } = [firstCoord];
    public List<Coord> OuterPerimeter { get; set; } = [];
    public HashSet<Coord> Unconnected { get; set; } = [];
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
            foreach (var neighbour in current.Neighbours.Where(nb => !queue.Contains(nb)))
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
            Unconnected.Clear();
            Sides = 4;
            return;
        }

        ProcessPillars(); //Single cell lines

        ProcessCorners(); // 2 neighbours in the Region list, the others out of bounds or wrong letter on the map

        Unconnected = Unconnected.Except(OuterPerimeter).ToHashSet();
    }

    private void ProcessPillars()
    {
        HashSet<Coord> visited = [];
        var pillars = Unconnected.Where(IsPillar);
        foreach (var pillar in pillars)
        {
            if (!visited.Add(pillar))
                continue;

            Sides += 3;

            OuterPerimeter.Add(pillar);

            if (!pillar.Neighbours.Any(Unconnected.Contains))
                continue;

            var (current, dir) = PillarProcessDirection(pillar);

            while (current.Neighbours.Count(Region.Contains) is 2)
            {
                if (!visited.Add(current))
                    break;

                OuterPerimeter.Add(current);

                Coord next = current.Neighbours.Skip((int)dir).First();

                current = next;
            }

            if (IsPillar(current))
            {
                Sides++;
                OuterPerimeter.Add(current);
                visited.Add(current);
            }
        }
    }

    private (Coord, Direction) PillarProcessDirection(Coord current)
    {
        var (index, coord) = current.Neighbours.Index().First(elem => Region.Contains(elem.Item));

        return (coord, (Direction)index);
    }

    private void ProcessCorners()
    {
        Dictionary<Coord, int> corners = [];
        foreach (var corner in Unconnected.Where(IsCorner))
        {
            corners[corner] = corner.Neighbours.Any(IsCorner) ? 1 : 2;
            OuterPerimeter.Add(corner);
        }

        foreach (var corner in corners.Keys)
        {
            if (corner.Neighbours.Any(IsCorner))
            {
                corners[corner] = 1;
            }

            Sides += corners[corner];

            foreach (var neighbour in corner.Neighbours.Where(nb => Region.Contains(nb) && !OuterPerimeter.Contains(nb)))
            {
                Direction dir = neighbour switch
                {
                    var c when c == corner.Up => new(),
                    var c when c == corner.Right => (Direction)1,
                    var c when c == corner.Down => (Direction)2,
                    _ => (Direction)3
                };

                OuterPerimeter.Add(neighbour);
                AddIfWithinRegion(neighbour, dir, corners);
            }
        }

        OuterPerimeter = OuterPerimeter.Distinct().ToList();
    }

    private void AddIfWithinRegion(Coord current, Direction direction, Dictionary<Coord, int> corners)
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
            AddIfWithinRegion(neighbour, direction, corners);
        }
        else if (corners.TryGetValue(current, out int storedValue))
        {
            corners[current] = int.Max(0, storedValue - 1);
        }
    }

    private void FindInnerRegions(Span2D<char> map)
    {
        if (Region.Count < 8 || Unconnected.Count < 4)
            return;

        List<Coord> missingNeighbours = Unconnected.SelectMany(c => c.Neighbours.Where(n => !Region.Contains(n))).Distinct().ToList();
        HashSet<Coord> visited = [];

        foreach (var coord in missingNeighbours.Where(c => !visited.Contains(c)))
        {
            if (coord.Neighbours.All(nb => Unconnected.Contains(nb) || OuterPerimeter.Contains(nb)))
            {
                Perimeter2D innerPerimeter = new(coord);
                innerPerimeter.ExpandRegion(map);
                InnerRegions.Add(innerPerimeter);
                visited.Add(coord);

                foreach (var neighbour in coord.Neighbours.Where(neighbour => neighbour.Neighbours.All(nb => Region.Contains(nb) || innerPerimeter.OuterPerimeter.Contains(nb))))
                {
                    Unconnected.Remove(neighbour);
                }

                Sides += 4;

                continue;
            }

            if (FormsLoop(coord, missingNeighbours, visited))
            {
                Perimeter2D innerPerimeter = new(coord);
                innerPerimeter.ExpandRegion(map);
                InnerRegions.Add(innerPerimeter);
                Sides += innerPerimeter.Sides;

                var touchingCoords = innerPerimeter.OuterPerimeter
                                     .SelectMany(c => c.Neighbours
                                         .Where(nb => !innerPerimeter.Region.Contains(nb)));

                foreach (var element in touchingCoords)
                {
                    Unconnected.Remove(element);
                }
            }
        }

        CleanUnconnected();

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

    private void CleanUnconnected()
    {
        var missesNeighbour = Unconnected.ToList();
        foreach (var strayCell in missesNeighbour)
        {
            if (strayCell.Neighbours.All(nb => Region.Contains(nb) || InnerRegions.Any(r => r.OuterPerimeter.Contains(nb))))
            {
                Unconnected.Remove(strayCell);
            }
        }
    }

    private bool IsPillar(Coord coord) => coord.Neighbours.Count(Region.Contains) == 1;

    private bool IsCorner(Coord coord)
    {
        if (coord.Neighbours.Count(Region.Contains) == 2)
        {
            var storedNeighbours = coord.Neighbours.Index()
                                                   .Where(elem => Region.Contains(elem.Item))
                                                   .ToArray();

            return storedNeighbours switch
            {
            [(0, _), (1, _)] => true,
            [(1, _), (0, _)] => true,
            [(1, _), (2, _)] => true,
            [(2, _), (1, _)] => true,
            [(2, _), (3, _)] => true,
            [(3, _), (2, _)] => true,
            [(3, _), (0, _)] => true,
            [(0, _), (3, _)] => true,
                _ => false
            };
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
