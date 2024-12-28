using AdventUtilities;

namespace Day15;

public interface IMovable
{
    void Move(Coord move, Direction dir, char[,] map, List<IMovable> boxes);
    bool Occupies(Coord coord);
    bool OccupiesAny(IEnumerable<Coord> targetCoords);
    bool OccupiesAnyColInRowBefore(int row, int targetCol);
    bool OccupiesAnyColInRowAfter(int row, int targetCol);
    bool OccupiesAnyRowInColBefore(int col, int targetRow);
    bool OccupiesAnyRowInColAfter(int col, int targetRow);
}
