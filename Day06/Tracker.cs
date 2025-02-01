
namespace Day06;

using AdventUtilities;


public class Tracker(string[] input)
{
    private readonly string[] input = input;

    const char Guard = 'G';
    const char North = '^', East = '>', South = 'v', West = '<', Obstacle = '#', Inserted = 'O', Open = '.';

    public int PartOne()
    {
        Coord guard = GetGuardPosition(input);
        char[][] traversedMap = TraverseMap(input, guard);

        return traversedMap.SelectMany(x => x).Count(c => c is not Open and not Obstacle);
    }

    private static char[][] TraverseMap(string[] input, Coord current)
    {
        int maxRow = input.Length;
        int maxCol = input[0].Length;
        char[][] map = input.JaggedCharArray();
        map[current.Row][current.Col] = Guard;
        Direction dir = new();

        while (WithinBounds(current, maxRow, maxCol))
        {
            MarkDirection(map, dir, current);

            Coord nextCell = current.Neighbours.Skip((int)dir).First();
            if (WithinBounds(nextCell, maxRow, maxCol) && map[nextCell.Row][nextCell.Col] is Obstacle or Inserted)
            {
                dir = TurnRight(dir);
                continue;
            }

            current = current.Neighbours.Skip((int)dir).First();
        }

        return map;
    }

    public int PartTwo()
    {
        Coord guard = GetGuardPosition(input);

        char[][] map = input.JaggedCharArray();
        List<Coord> potentialObstacles = ScanMap(map, guard);

        int loops = 0;
        foreach (var insert in potentialObstacles.Distinct())
        {
            if (CausesLoop((char[][])map.Clone(), guard, new(insert.Row, insert.Col)))
            {
                loops++;
            }
        }

        return loops;
    }

    private static List<Coord> ScanMap(char[][] map, Coord current)
    {
        int maxRow = map.Length;
        int maxCol = map[0].Length;

        List<Coord> obstacleScenarios = [];
        map[current.Row][current.Col] = Guard;
        Direction dir = new();

        while (WithinBounds(current, maxRow, maxCol))
        {
            Coord nextCell = current.Neighbours.Skip((int)dir).First();
            if (WithinBounds(nextCell, maxRow, maxCol) && map[nextCell.Row][nextCell.Col] is Obstacle or Inserted)
            {
                dir = TurnRight(dir);
                continue;
            }
            else
            {
                MarkDirection(map, dir, current);

                Span<char> rightSpan = dir switch
                {
                    Direction.Up when current.Col < maxCol - 1 => map[current.Row].AsSpan()[(current.Col + 1)..],
                    Direction.Down when current.Col > 0 => map[current.Row].AsSpan()[..current.Col],
                    Direction.Left when current.Row > 0 => map.Take(current.Row)
                                                              .Select(row => row[current.Col])
                                                              .ToArray(),
                    Direction.Right when current.Row < maxRow - 1 => map.Skip(current.Row + 1)
                                                                        .Select(row => row[current.Col])
                                                                        .ToArray(),

                    _ => []
                };

                if (rightSpan.Contains(Obstacle) && WithinBounds(nextCell, maxRow, maxCol) && CurrentChar(map, nextCell) is not Obstacle and not Guard)
                {
                    Coord newObstacle = dir switch
                    {
                        Direction.Up => current.Up,
                        Direction.Right => current.Right,
                        Direction.Down => current.Down,
                        _ => current.Left
                    };

                    obstacleScenarios.Add(newObstacle);
                }
            }

            current = current.Neighbours.Skip((int)dir).First();
        }

        return obstacleScenarios;
    }

    private static bool CausesLoop(char[][] currentMap, Coord guard, Coord insert)
    {
        int maxRow = currentMap.Length;
        int maxCol = currentMap[0].Length;
        CleanMap(currentMap);

        if (insert == guard)
            return false;

        HashSet<(Coord, Direction)> visited = [];
        Coord current = new(guard.Row, guard.Col);

        currentMap[insert.Row][insert.Col] = Inserted;
        currentMap[current.Row][current.Col] = Guard;
        Direction dir = new();

        while (WithinBounds(current, maxRow, maxCol))
        {
            Coord nextCell = current.Neighbours.Skip((int)dir).First();
            if (WithinBounds(nextCell, maxRow, maxCol) && currentMap[nextCell.Row][nextCell.Col] is Obstacle or Inserted)
            {
                if (!visited.Add((current, dir)))
                {
                    return true;
                }

                dir = TurnRight(dir);
                continue;
            }

            MarkDirection(currentMap, dir, current);

            current = current.Neighbours.Skip((int)dir).First();
        }

        return false;
    }

    private static void MarkDirection(char[][] map, Direction dir, Coord current) => map[current.Row][current.Col] = dir switch
    {
        _ when CurrentChar(map, current) is Guard => Guard,
        _ when CurrentChar(map, current) is not Open => '+',
        Direction.Up => North,
        Direction.Right => East,
        Direction.Down => South,
        _ => West,
    };

    private static void CleanMap(char[][] map)
    {
        Array.ForEach(map, arr =>
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] is not Guard and not Obstacle)
                {
                    arr[i] = Open;
                }
            }
        });
    }

    private static Direction TurnRight(Direction dir) => (Direction)(((int)dir + 1) % 4);

    private static char CurrentChar(char[][] map, Coord current) => map[current.Row][current.Col];

    private static Coord GetGuardPosition(ReadOnlySpan<string> input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].Contains(North))
            {
                return new(i, input[i].IndexOf(North));
            }
        }

        return new(-1, -1);
    }


    static bool WithinBounds(Coord c, int maxRow, int maxCol) => c.Row >= 0 && c.Row < maxRow
                                                              && c.Col >= 0 && c.Col < maxCol;
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

