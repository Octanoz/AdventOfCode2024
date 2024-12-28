using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class Robot(Coord position, char[,] map, List<Direction> moves, List<Coord> walls, List<IMovable> boxes)
{
    const char Wall = '#';
    private readonly char[,] map = map;
    private readonly List<Direction> moves = moves;
    private MapUpdater mapUpdater = new(boxes, walls);
    private readonly int boxCount = boxes.Count;
    private Coord lastPosition = position;

    private static readonly Dictionary<Direction, Coord> directionMap = new()
    {
        [Direction.Up] = Coord.Zero.Up,
        [Direction.Right] = Coord.Zero.Right,
        [Direction.Down] = Coord.Zero.Down,
        [Direction.Left] = Coord.Zero.Left
    };

    public Coord Position { get; set; } = position;

    public bool IsBlocked(Coord move, Direction dir)
    {
        Box? nbBox = FindBoxNeighbour(move, boxes, out Coord target);

        if (map[target.Row, target.Col] is Wall)
            return true;

        return nbBox?.IsBlocked(move, dir, map, boxes) ?? false;
    }

    public void Move(Coord move, Direction dir, List<IMovable> boxes)
    {
        bool boxMoved = false;
        using StreamWriter swBlocked = new(Path.Combine(InputData.GetSolutionDirectory(), "Day15/Notes/output.txt"), true);

        MonitorBoxes(dir);

        var mapSpan = map.AsSpan2D();
        Box? nbBox = FindBoxNeighbour(move, boxes, out Coord target);
        if (mapSpan.GetValueAt(target) is Wall)
            return;

        if (nbBox is not null)
        {
            nbBox.Move(move, dir, map, boxes);
            if (nbBox.Position == target)
            {
                swBlocked.WriteLine($"Unable to move this turn");
                swBlocked.WriteLine(dir switch
                {
                    Direction.Up => "\nMOVING UP\n--------\n",
                    Direction.Right => "\nMOVING RIGHT\n--------\n",
                    Direction.Down => "\nMOVING DOWN\n--------\n",
                    Direction.Left => "\vMOVING LEFT\n--------\n",
                    _ => throw new ArgumentException($"Unknown direction {dir} found.")
                });

                return;
            }

            boxMoved = true;
        }

        lastPosition = Position;
        Position = target;

        mapUpdater.UpdateBoxesMap(mapSpan, lastPosition, Position, dir, boxMoved);

        mapSpan.Draw2DGridTight();
        /* if (dir is Direction.Left or Direction.Right)
        {
            var rowSpan = dir is Direction.Right
                        ? mapSpan.GetRowSpan(Position.Row)[Position.Col..]
                        : mapSpan.GetRowSpan(Position.Row)[..(Position.Col + 1)];

            if (dir is Direction.Right)
            {
                mapUpdater.UpdateMapRight(rowSpan, lastPosition);
            }
            else mapUpdater.UpdateMapLeft(rowSpan, lastPosition);
        }
        else if (dir is Direction.Up)
        {
            mapUpdater.UpdateMapUp(mapSpan, lastPosition);
        }
        else mapUpdater.UpdateMapDown(mapSpan, lastPosition);


        Position = target; */
    }

    private void MonitorBoxes(Direction dir)
    {
        using StreamWriter sw = new(Path.Combine(InputData.GetSolutionDirectory(), "Day15/output.txt"));
        int boxesCounted = map.Count('O');

        do
        {
            switch (boxesCounted)
            {
                case var bc when bc == boxCount:
                    sw.WriteLine($"Found {boxCount} boxes in the map as expected. No further action taken.");
                    break;
                case var bc when bc < boxCount:
                    sw.WriteLine($"Found {boxesCounted} boxes in the map. Scanning the boxes list for missing elements.");
                    foreach (var box in boxes.OfType<Box>())
                    {
                        if (map[box.Position.Row, box.Position.Col] == '.')
                        {
                            sw.WriteLine($"({box.Position.Row}, {box.Position.Col}) was found missing.");
                            map[box.Position.Row, box.Position.Col] = 'O';
                        }
                    }
                    break;
                case var bc when bc > boxCount:
                    sw.WriteLine($"Found {boxesCounted} boxes which is over the maximum of 21 boxes that should exist in the map. Scrubbing map.");
                    for (int i = 0; i < map.GetLength(0); i++)
                    {
                        for (int j = 0; j < map.GetLength(1); j++)
                        {
                            if (map[i, j] == 'O' && !boxes.Exists(box => box.Occupies(new(i, j))))
                            {
                                sw.WriteLine($"Found extra box at ({i}, {j})");
                                map[i, j] = '.';
                            }
                        }
                    }
                    break;
            }

            boxesCounted = map.Count('O');
            if (boxesCounted != boxCount)
            {
                sw.WriteLine($"Still not at the correct amount of boxes, looping back to beginning of the correction process.");
            }
        } while (boxesCounted != boxCount);


        sw.WriteLine($"Current map:\n{map.Write2DGridTight()}");

        sw.WriteLine(dir switch
        {
            Direction.Up => "\nMOVING UP\n--------\n",
            Direction.Right => "\nMOVING RIGHT\n--------\n",
            Direction.Down => "\nMOVING DOWN\n--------\n",
            Direction.Left => "\vMOVING LEFT\n--------\n",
            _ => throw new ArgumentException($"Unknown direction {dir} found.")
        });
    }

    private Box? FindBoxNeighbour(Coord move, List<IMovable> boxes, out Coord neighbourCoord)
    {
        neighbourCoord = Position + move;
        var target = neighbourCoord;
        return (Box?)boxes.Find(box => box.Occupies(target));
    }

    private WideBox? FindWideBoxNeighbour(Coord move, List<IMovable> boxes, out Coord neighbourCoord)
    {
        neighbourCoord = Position + move;
        var target = neighbourCoord;
        return (WideBox?)boxes.Find(box => box.Occupies(target));
    }

    public (List<IMovable>, char[][]) ProcessMoves()
    {
        foreach (var direction in moves)
        {
            Coord target = directionMap[direction];
            Move(target, direction, boxes);
        }

        return (boxes, map.Convert2DToJagged());
    }
}
