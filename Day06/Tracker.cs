
namespace Day06;

using System.Diagnostics;
using AdventUtilities;


public class Tracker(string[] input)
{
    private readonly string[] input = input;
    private static readonly Queue<Coord> loopQueue = [];

    const char Guard = 'G';
    const char North = '^', East = '>', South = 'v', West = '<', Obstacle = '#', Inserted = 'O', Open = '.';

    public int PartOne()
    {
        Coord guard = GetGuardPosition(input);

        char[][] traversedMap = TraverseMap(input, guard);

        return traversedMap.SelectMany(x => x).Count(c => c is North or East or South or West or Guard);
    }

    public int PartTwo(string filePath)
    {
        Coord guard = GetGuardPosition(input);

        List<Coord> potentialObstacles = ScanMap(input, guard);
        List<Coord> validObstacles = FindValidObstacles(potentialObstacles, input, guard, filePath);

        return validObstacles.Count;
    }

    private static List<Coord> FindValidObstacles(List<Coord> potentialObstacles, string[] input, Coord guard, string filePath)
    {
        List<Coord> winners = [];
        int maxRow = input.Length;
        int maxCol = input[0].Length;

        foreach (var rock in potentialObstacles.Where(r => WithinBounds(r, maxRow, maxCol)))
        {
            Dictionary<Coord, int> cornerCount = [];
            HashSet<Coord> visited = [];
            Coord current = new(guard.Row, guard.Col);
            char[][] map = GridExtensions.JaggedCharArray(File.ReadAllLines(filePath));

            map[rock.Row][rock.Col] = Inserted;
            map[current.Row][current.Col] = Guard;
            Direction dir = new();

            while (WithinBounds(current, maxRow, maxCol))
            {
                map[current.Row][current.Col] = dir switch
                {
                    _ when map[current.Row][current.Col] is Guard => Guard,
                    _ when map[current.Row][current.Col] is not Open => '+',
                    Direction.Up => North,
                    Direction.Right => East,
                    Direction.Down => South,
                    _ => West,
                };

                Span<Coord> neighbourSpan = current.Neighbours.ToArray();
                Coord nextCell = neighbourSpan[(int)dir];
                if (WithinBounds(nextCell, maxRow, maxCol) && map[nextCell.Row][nextCell.Col] is Obstacle or Inserted)
                {
                    if (!visited.Add(current))
                    {
                        cornerCount[current] = cornerCount.TryGetValue(current, out var counter) ? counter + 1 : 1;

                        if (cornerCount.Values.Count(c => c >= 6) >= 4)
                        {
                            winners.Add(rock);
                            break;
                        }
                    }

                    dir = TurnRight(dir);
                }

                current = dir switch
                {
                    Direction.Up => current.Up,
                    Direction.Right => current.Right,
                    Direction.Down => current.Down,
                    _ => current.Left
                };
            }

            map[rock.Row][rock.Col] = Open;
        }

        return winners;
    }

    private List<Coord> ScanMap(string[] input, Coord current)
    {
        int maxRow = input.Length;
        int maxCol = input[0].Length;
        List<Coord> obstacleCoords = [];
        char[][] map = input.Select(s => s.ToCharArray()).ToArray();
        map[current.Row][current.Col] = Guard;
        Direction dir = new();

        while (WithinBounds(current, maxRow, maxCol))
        {
            map[current.Row][current.Col] = dir switch
            {
                _ when map[current.Row][current.Col] is Guard => Guard,
                _ when map[current.Row][current.Col] is not Open => '+',
                Direction.Up => North,
                Direction.Right => East,
                Direction.Down => South,
                _ => West,
            };

            Span<Coord> neighbourSpan = current.Neighbours.ToArray();
            Coord nextCell = neighbourSpan[(int)dir];
            if (WithinBounds(nextCell, maxRow, maxCol) && map[nextCell.Row][nextCell.Col] is Obstacle or Inserted)
            {
                dir = TurnRight(dir);
                continue;
            }
            else
            {
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

                if (rightSpan.Contains(Obstacle) && WithinBounds(nextCell, maxRow, maxCol) && map[nextCell.Row][nextCell.Col] is not Obstacle)
                {
                    obstacleCoords.Add(dir switch
                    {
                        Direction.Up => current.Up,
                        Direction.Right => current.Right,
                        Direction.Down => current.Down,
                        _ => current.Left
                    });
                }
            }

            current = dir switch
            {
                Direction.Up => current.Up,
                Direction.Right => current.Right,
                Direction.Down => current.Down,
                _ => current.Left
            };
        }

        return obstacleCoords;
    }

    private static bool IsLoopDetected(Queue<Coord> recent, Dictionary<Coord, int> cornerCount, Coord current,
                                        int windowSize, int cornerThreshold)
    {
        if (recent.Count == windowSize)
            recent.Dequeue();

        recent.Enqueue(current);

        cornerCount[current] = cornerCount.TryGetValue(current, out var counter) ? counter + 1 : 1;

        if (cornerCount.Values.Count(c => c >= cornerThreshold) >= cornerThreshold)
        {
            int half = recent.Count / 2;
            return recent.Count == windowSize && recent.Take(half).SequenceEqual(recent.Skip(half));
        }

        return false;
    }



    private char[][] TraverseMap(string[] input, Coord current)
    {
        int maxRow = input.Length;
        int maxCol = input[0].Length;
        char[][] map = input.Select(s => s.ToCharArray()).ToArray();
        map[current.Row][current.Col] = Guard;
        Direction dir = new();

        while (WithinBounds(current, maxRow, maxCol))
        {
            map[current.Row][current.Col] = dir switch
            {
                _ when map[current.Row][current.Col] is Guard => Guard,
                Direction.Up => North,
                Direction.Right => East,
                Direction.Down => South,
                _ => West,
            };

            Span<Coord> neighbourSpan = current.Neighbours.ToArray();
            Coord nextCell = neighbourSpan[(int)dir];
            if (WithinBounds(nextCell, maxRow, maxCol) && map[nextCell.Row][nextCell.Col] is Obstacle or Inserted)
            {
                dir = TurnRight(dir);
                continue;
            }

            current = dir switch
            {
                Direction.Up => current.Up,
                Direction.Right => current.Right,
                Direction.Down => current.Down,
                _ => current.Left
            };
        }

        // GridExtensions.DrawJaggedGridTight(map);

        return map;
    }

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

    private static Direction TurnRight(Direction dir) => (Direction)(((int)dir + 1) % 4);

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
