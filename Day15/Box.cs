using AdventUtilities;

namespace Day15;

public class Box(Coord position) : IMovable
{
    public Coord Position { get; set; } = position;

    public bool IsBlocked(Coord move, Direction dir, char[,] map, List<IMovable> boxes)
    {
        Box? nbBox = FindNeighbor(move, boxes, out Coord nbCoord);

        if (nbBox is null)
        {
            if (map[nbCoord.Row, nbCoord.Col] is '#')
            {
                return true;
            }

            return false;
        }

        return nbBox.IsBlocked(move, dir, map, boxes);
    }

    public void Move(Coord move, Direction dir, char[,] map, List<IMovable> boxes)
    {
        if (!IsBlocked(move, dir, map, boxes))
        {
            Box? nbBox = FindNeighbor(move, boxes, out Coord nbCoord);

            if (nbBox is not null)
            {
                nbBox.Move(move, dir, map, boxes);

                if (nbBox.Position == nbCoord)
                {
                    return;
                }
            }

            Position = nbCoord;
        }
    }

    private Box? FindNeighbor(Coord move, List<IMovable> boxes, out Coord nbCoord)
    {
        nbCoord = Position + move;
        var target = nbCoord;
        return (Box?)boxes.Find(box => box.Occupies(target));
    }

    public bool Occupies(Coord coord) => Position == coord;
    public bool OccupiesAny(IEnumerable<Coord> targetCoords) => targetCoords.Any(Occupies);
    public bool OccupiesAnyColInRowBefore(int row, int targetCol) => Position.Row == row && Position.Col < targetCol;
    public bool OccupiesAnyColInRowAfter(int row, int targetCol) => Position.Row == row && Position.Col > targetCol;
    public bool OccupiesAnyRowInColBefore(int col, int targetRow) => Position.Col == col && Position.Row < targetRow;
    public bool OccupiesAnyRowInColAfter(int col, int targetRow) => Position.Col == col && Position.Row > targetRow;
}
