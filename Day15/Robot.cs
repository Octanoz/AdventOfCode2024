using System.Diagnostics;
using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class Robot(Coord position, char[,] map, List<Direction> moves, List<Box> boxes)
{
    const char Wall = '#';
    private readonly char[,] map = map;
    private readonly List<Direction> moves = moves;
    private readonly MapUpdater mapUpdater = new(boxes);
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

    public void Move(Coord move, Direction dir)
    {
        bool boxMoved = false;
        var mapSpan = map.AsSpan2D();

        Box? nbBox = FindBoxNeighbour(move, boxes, out Coord target);
        if (mapSpan.GetValueAt(target) is Wall)
            return;

        if (nbBox is not null)
        {
            nbBox.Move(move, dir, map, boxes);
            if (nbBox.Position == target)
            {
                return;
            }

            boxMoved = true;
        }

        lastPosition = Position;
        Position = target;

        mapUpdater.UpdateBoxesMap(mapSpan, lastPosition, Position, dir, boxMoved);
    }

    private Box? FindBoxNeighbour(Coord move, List<Box> boxes, out Coord neighbourCoord)
    {
        neighbourCoord = Position + move;
        var target = neighbourCoord;
        return boxes.Find(box => box.Occupies(target));
    }

    public (List<Box>, char[][]) ProcessMoves()
    {
        foreach (var direction in moves)
        {
            Coord target = directionMap[direction];
            Move(target, direction);
        }

        return (boxes, map.Convert2DToJagged());
    }
}
