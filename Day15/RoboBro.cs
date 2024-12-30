using System.Text;
using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class RoboBro(Coord position, char[,] map, List<Direction> moves, List<WideBox> boxes)
{
    const char Wall = '#';
    private readonly char[,] map = map;
    private readonly List<Direction> moves = moves;
    private readonly WideMapUpdater mapUpdater = new(boxes);
    private readonly int boxCount = boxes.Count;
    private Coord lastPosition = position;

    private readonly Queue<string> recentMoves = [];

    private static readonly Dictionary<Direction, Coord> directionMap = new()
    {
        [Direction.Up] = Coord.Zero.Up,
        [Direction.Right] = Coord.Zero.Right,
        [Direction.Down] = Coord.Zero.Down,
        [Direction.Left] = Coord.Zero.Left
    };

    public Coord Position { get; set; } = position;

    public void Move(Span2D<char> mapSpan, Coord move, Direction dir)
    {
        bool boxMoved = false;
        Coord target = Position + move;
        if (mapSpan.GetValueAt(target) is Wall)
            return;

        WideBox? nbBox = FindNeighbour(target, boxes);
        if (nbBox is not null)
        {
            nbBox.Move(move, dir, map, boxes);

            if (nbBox.Occupies(target))
            {
                foreach (var movedBox in boxes.Where(b => b.Moved))
                {
                    mapUpdater.WideBoxMoveUpdate(map.AsSpan2D(), movedBox, movedBox.GetMapData(true));
                }
                return;
            }

            boxMoved = true;
        }

        lastPosition = Position;
        Position = target;

        mapUpdater.UpdateMap(mapSpan, lastPosition, Position, boxMoved);
    }

    private WideBox? FindNeighbour(Coord target, List<WideBox> boxes) => boxes.Find(box => box.Occupies(target));

    public (List<WideBox>, char[][]) ProcessMoves()
    {
        var mapSpan = map.AsSpan2D();
        boxes.ForEach(box => box.CheckLockedPosition(map.AsSpan2D(), boxes));

        StringBuilder sb = new();
        int moveCounter = 0;
        foreach (var direction in moves)
        {
            if (moveCounter % 10 == 0)
                LogMoves();

            while (recentMoves.Count > 10)
                recentMoves.Dequeue();

            sb.AppendLine($"Move {++moveCounter}: Direction is {direction}.\nThe robot is currently at coordinates {Position}");
            Coord storedPosition = Position;
            Coord target = directionMap[direction];
            Move(mapSpan, target, direction);

            if (Position == storedPosition)
            {
                sb.AppendLine($"Loop finished at same position as at the start of the loop: {storedPosition}");
            }
            else
            {
                sb.AppendLine($"Loop finished at {Position}\nCurrent map:\n");

                recentMoves.Enqueue(sb.ToString());
                recentMoves.Enqueue(map.Write2DGridTight());
                sb.Clear();
            }
        }

        return (boxes, map.Convert2DToJagged());
    }

    private void LogMoves()
    {
        File.WriteAllLines(Path.Combine(InputData.GetSolutionDirectory(), "Day15/Notes/output.txt"), recentMoves);
    }

    private void LogFailure(Exception ex)
    {
        string filePath = Path.Combine(InputData.GetSolutionDirectory(), "Day15/Notes/output.txt");
        string errorLog = $"\nNull reference: {ex.Message}\nStack trace:\n{ex.StackTrace}";
        File.AppendAllLines(filePath, recentMoves);
        File.AppendAllText(filePath, errorLog);
    }
}
