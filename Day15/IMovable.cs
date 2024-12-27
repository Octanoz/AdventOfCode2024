using AdventUtilities;

namespace Day15;

public interface IMovable
{
    void Move(Coord move, Direction dir, char[,] map, List<IMovable> boxes);
    bool Occupies(Coord coord);
}
