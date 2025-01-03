namespace AdventUtilities;

using TableContext = (char[,] Table, int MaxRow, int MaxCol);
public static class CoordExtensions
{
    public static IEnumerable<Coord> GetAllNeighboursNoLimits(this Coord coord)
    {
        var deltas = ((int, int)[])
        [
            (-1,-1), (-1, 0), (-1, 1),
            ( 0,-1),          ( 0, 1),
            ( 1,-1), ( 1, 0), ( 1, 1)
        ];

        foreach (var (dRow, dCol) in deltas)
        {
            yield return new(coord.Row + dRow, coord.Col + dCol);
        }
    }

    public static IEnumerable<Coord> GetAllNeighbours(this Coord coord, int maxRow, int maxCol)
    {
        var deltas = ((int, int)[])
        [
            (-1,-1), (-1, 0), (-1, 1),
            ( 0,-1),          ( 0, 1),
            ( 1,-1), ( 1, 0), ( 1, 1)
        ];

        foreach (var (dRow, dCol) in deltas)
        {
            Coord neighbour = new(coord.Row + dRow, coord.Col + dCol);
            if (neighbour.Row >= 0 && neighbour.Row < maxRow && neighbour.Col >= 0 && neighbour.Col < maxCol)
            {
                yield return neighbour;
            }
        }
    }

    public static IEnumerable<Coord> GetAllNeighbours(this Coord coord, TableContext tc) => coord.GetAllNeighbours(tc.MaxRow, tc.MaxCol);

    /* public static Coord MoveTo(this Coord coord, Direction dir) => dir switch
    {
        Direction.Up => coord.Up,
        Direction.Right => coord.Right,
        Direction.Down => coord.Down,
        Direction.Left => coord.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    }; */

}
