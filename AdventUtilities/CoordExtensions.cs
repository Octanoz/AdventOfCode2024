namespace AdventUtilities;

using TableContext = (char[,] Table, int MaxRow, int MaxCol);
public static class CoordExtensions
{
    /// <summary>
    /// Returns an Enumerable of all 8 coordinates immediately around the current Coord coordinate. These coordinates can
    /// be negative values and therefore out of bounds of any grid they are used in.
    /// </summary>
    /// <param name="coord">The Coord coordinate</param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns a Enumerable of all of the 8 coordinates immediately around the current Coord coordinate that have
    /// a positive value and the values for Row and Col do not exceed the values specified in the call.
    /// </summary>
    /// <param name="coord">The Coord coordinate (Row and Col).</param>
    /// <param name="maxRow">Equivalent to the width of the table.</param>
    /// <param name="maxCol">Equivalant to the height of the table.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns an Enumerable of all of the 8 coordinates immediately around the current Coord coordinate that have
    /// a positive value and, in this overload, do not exceed the maximum values stored in the TableContext object.
    /// </summary>
    /// <param name="coord">The Coord coordinate (Row and Col).</param>
    /// <param name="tc">The TableContext object</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetAllNeighbours(this Coord coord, TableContext tc) => coord.GetAllNeighbours(tc.MaxRow, tc.MaxCol);

    public static IEnumerable<Coord> GetValidNeighbours(this Coord coord, int maxRow, int maxCol)
        => coord.Neighbours.Where(nb => nb.Row >= 0 && nb.Row < maxRow && nb.Col >= 0 && nb.Col < maxCol);

    /// <summary>
    /// Returns the coordinates stored in the Neighbours Enumerable of the curent Coord coordinate and checks that they are
    /// within the limits of the map stored in the TableContext object
    /// </summary>
    /// <param name="coord">The Coord object</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a maxRow and maxCol integer.</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetValidNeighbours(this Coord coord, TableContext tc)
        => coord.Neighbours.Where(nb => nb.Row >= 0 && nb.Row < tc.MaxRow && nb.Col >= 0 && nb.Col < tc.MaxCol);

    /// <summary>
    /// Returns the coordinates stored in the Neighbours Enumerable of the current Coord coordinate and checks that 
    /// they are within the limits specified in the TableContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The Coord coordinate</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <param name="visited">A HashSet<Coord>.</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetValidNeighbours(this Coord coord, TableContext tc, HashSet<Coord> visited)
        => coord.Neighbours.Where(nb => nb.Row >= 0 && nb.Row < tc.MaxRow
                                     && nb.Col >= 0 && nb.Col < tc.MaxCol
                                     && !visited.Contains(nb));

    /// <summary>
    /// Returns the coordinates stored in the Neighbours Enumerable of the current Coord coordinate and checks that 
    /// they are within the limits specified in the TableContext object and that they are not stored in a 2D bool array.
    /// </summary>
    /// <param name="coord">The Coord coordinate</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <param name="visitedArray">A 2D bool array (bool[,])</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetValidNeighbours(this Coord coord, TableContext tc, bool[,] visitedArray)
        => coord.Neighbours.Where(nb => nb.Row >= 0 && nb.Row < tc.MaxRow
                                     && nb.Col >= 0 && nb.Col < tc.MaxCol
                                     && !visitedArray[nb.Row, nb.Col]);

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current Coord coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the TableContext object
    /// and that they are a '#' hash on the map stored in the TableContext object
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetHashNeighbours(this Coord coord, TableContext tc)
        => coord.GetValidNeighbours(tc).Where(nb => tc.Table[nb.Row, nb.Col] is '#');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current Coord coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the TableContext object, 
    /// that they are a '#' hash on the map stored in the TableContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <param name="visited">The HashSet<Coord>.</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetHashNeighbours(this Coord coord, TableContext tc, HashSet<Coord> visited)
        => coord.GetValidNeighbours(tc, visited).Where(nb => tc.Table[nb.Row, nb.Col] is '#');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current Coord coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the TableContext object, 
    /// that they are a '#' hash on the map stored in the TableContext object and that they are not stored in a 
    /// 2D boolean array.
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <param name="visitedArray">A 2D boolean array (bool[,]).</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetHashNeighbours(this Coord coord, TableContext tc, bool[,] visitedArray)
        => coord.GetValidNeighbours(tc, visitedArray).Where(nb => tc.Table[nb.Row, nb.Col] is '#');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current Coord coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the TableContext object
    /// and that they are a '.' dot on the map stored in the TableContext object
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetDotNeighbours(this Coord coord, TableContext tc)
        => coord.GetValidNeighbours(tc).Where(nb => tc.Table[nb.Row, nb.Col] is '.');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current Coord coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the TableContext object, 
    /// that they are a '.' dot on the map stored in the TableContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <param name="visited">The HashSet<Coord>.</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetDotNeighbours(this Coord coord, TableContext tc, HashSet<Coord> visited)
        => coord.GetValidNeighbours(tc, visited).Where(nb => tc.Table[nb.Row, nb.Col] is '.');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current Coord coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the TableContext object, 
    /// that they are a '#' hash on the map stored in the TableContext object and that they are not stored in a 
    /// 2D boolean array.
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">The TableContext object, typically stores a char[,] Table and a MaxRow and MaxCol integer.</param>
    /// <param name="visitedArray">A 2D boolean array (bool[,]).</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetDotNeighbours(this Coord coord, TableContext tc, bool[,] visitedArray)
        => coord.GetValidNeighbours(tc, visitedArray).Where(nb => tc.Table[nb.Row, nb.Col] is '.');

    /// <summary>
    /// Returns an Enumerable of Coord of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values, that they are lower in value than
    /// the maximum X and maximum Y specified in the TableContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">TableContext object, commonly stores a char[,] Table and a maxRow and maxCol integer.</param>
    /// <param name="visited">The HashSet<Coord>.</param>
    /// <returns></returns>
    public static IEnumerable<Coord> GetAllValidNeighbours(this Coord coord, TableContext tc, HashSet<Coord> visited)
        => coord.GetAllNeighbours(tc).Where(nb => !visited.Contains(nb));

    /// <summary>
    /// Returns an Enumerable of Coord of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values, that they are lower in value than
    /// the maximum X and maximum Y specified in the TableContext object and that they are not stored in a 2D boolean array.
    /// </summary>
    /// <param name="coord">The Coord coordinate.</param>
    /// <param name="tc">TableContext object, commonly stores a char[,] Table and a maxRow and maxCol integer.</param>
    /// <param name="visitedArray">A 2D boolean array (bool[,]).</param>
    /// <returns></returns>
    /* public static Coord MoveTo(this Coord coord, Direction dir) => dir switch
    {
        Direction.Up => coord.Up,
        Direction.Right => coord.Right,
        Direction.Down => coord.Down,
        Direction.Left => coord.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    }; */

    public static (int, int) Tupled(this Coord coord) => (coord.Row, coord.Col);
}
