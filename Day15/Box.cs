using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class Box(Coord position)
{
    private Coord lastPosition = position;
    public Coord Position { get; set; } = position;
    public bool Moved { get; set; } = false;

    public void Move(Coord move, Direction dir, Span2D<char> mapSpan, List<Box> boxes)
    {
        Box? nbBox = FindNeighbor(move, boxes, out Coord nbCoord);
        if (mapSpan.GetValueAt(nbCoord) is '#')
            return;

        if (nbBox is not null)
        {
            nbBox.Move(move, dir, mapSpan, boxes);
            if (nbBox.Position == nbCoord)
                return;
        }

        Moved = true;
        lastPosition = Position;
        Position = nbCoord;
    }

    private Box? FindNeighbor(Coord move, List<Box> boxes, out Coord nbCoord)
    {
        nbCoord = Position + move;
        var target = nbCoord;
        return boxes.Find(box => box.Occupies(target));
    }

    public bool Occupies(Coord coord) => Position == coord;
    public bool OccupiesAny(IEnumerable<Coord> targetCoords) => targetCoords.Any(Occupies);
    public bool OccupiesAnyColInRowBefore(int row, int targetCol) => Position.Row == row && Position.Col < targetCol;
    public bool OccupiesAnyColInRowAfter(int row, int targetCol) => Position.Row == row && Position.Col > targetCol;
    public bool OccupiesAnyRowInColBefore(int col, int targetRow) => Position.Col == col && Position.Row < targetRow;
    public bool OccupiesAnyRowInColAfter(int col, int targetRow) => Position.Col == col && Position.Row > targetRow;

    public void Reset() => Position = lastPosition;

    public void ResetMovedState() => Moved = false;
}
