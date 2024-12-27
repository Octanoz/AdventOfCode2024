using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class Robot(Coord position, char[,] map, List<Direction> moves, List<Box> boxes, List<WideBox> wideBoxes)
{
    const char Wall = '#';
    private readonly char[,] map = map;
    private readonly List<Direction> moves = moves;
    private readonly MapUpdater mapUpdater = new(boxes);
    private readonly int boxCount = boxes.Count;

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
        Box? nbBox = FindNeighbor(move, boxes, out Coord target);

        if (map[target.Row, target.Col] is Wall)
            return true;

        return nbBox?.IsBlocked(move, dir, map, boxes) ?? false;
    }

    public void Move(Coord move, Direction dir, List<Box> boxes)
    {
        using StreamWriter swBlocked = new(Path.Combine(InputData.GetSolutionDirectory(), "Day15/Notes/output.txt"), true);
        if (IsBlocked(move, dir))
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

        MonitorBoxes(dir);

        Box? nbBox = FindNeighbor(move, boxes, out Coord target);

        if (nbBox is not null)
        {
            nbBox.Move(move, dir, map, boxes);
            if (nbBox.Position == target)
                return;
        }

        var mapSpan = map.AsSpan2D();

        if (dir is Direction.Left or Direction.Right)
        {
            var rowSpan = dir is Direction.Right
                        ? mapSpan.GetRowSpan(Position.Row)[Position.Col..]
                        : mapSpan.GetRowSpan(Position.Row)[..(Position.Col + 1)];

            if (dir is Direction.Right)
            {
                mapUpdater.UpdateMapRight(rowSpan, Position);
            }
            else mapUpdater.UpdateMapLeft(rowSpan, Position);
        }
        else if (dir is Direction.Up)
        {
            mapUpdater.UpdateMapUp(mapSpan, Position);
        }
        else mapUpdater.UpdateMapDown(mapSpan, Position);


        Position = target;
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
                    foreach (var box in boxes)
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
                            if (map[i, j] == 'O' && !boxes.Exists(box => box.Position.Row == i && box.Position.Col == j))
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

    private Box? FindNeighbor(Coord move, List<Box> boxes, out Coord neighborCoord)
    {
        neighborCoord = Position + move;
        var target = neighborCoord;
        return boxes.Find(box => box.Position == target);
    }

    public (List<Box>, char[][]) ProcessMoves()
    {
        foreach (var direction in moves)
        {
            Coord target = directionMap[direction];
            Move(target, direction, boxes);
        }

        return (boxes, map.Convert2DToJagged());
    }
}
