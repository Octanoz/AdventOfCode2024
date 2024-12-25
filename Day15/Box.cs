using AdventUtilities;

namespace Day15;

public class Box(Coord position, bool blockedRow = false, bool blockedCol = false)
{
    public Coord Position { get; set; } = position;
    public bool BlockedRow { get; private set; } = blockedRow;
    public bool BlockedCol { get; private set; } = blockedCol;

    public bool IsBlocked(Direction dir, char[][] map, List<Box> boxes)
    {
        if (dir is Direction.Left or Direction.Right && BlockedRow
          || dir is Direction.Up or Direction.Down && BlockedCol)
            return true;

        if (Position.Neighbours.ElementAt((int)dir) is Box)
        {

        }

    }
}
