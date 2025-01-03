namespace AdventUtilities;
using MapContext = (char[,] Map, int MaxX, int MaxY);

public static class CoordXYExtensions
{
    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates. The coordinates are not checked against any grid limits
    /// or whether they are positive or not.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllNeighboursNoLimits(this CoordXY coord)
    {
        var deltas = ((int, int)[])
        [
            (-1,-1), (-1, 0), (-1, 1),
            ( 0,-1),          ( 0, 1),
            ( 1,-1), ( 1, 0), ( 1, 1)
        ];

        foreach (var (dX, dY) in deltas)
        {
            yield return new(coord.X + dX, coord.Y + dY);
        }
    }

    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates.
    /// They have been checked to have positive values and that they are lower than the specified maximum X and maximum Y coordinates.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="maxX">The maximum width.</param>
    /// <param name="maxY">The maximum height.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllNeighbours(this CoordXY coord, int maxX, int maxY)
    {
        var deltas = ((int, int)[])
        [
            (-1,-1), (-1, 0), (-1, 1),
            ( 0,-1),          ( 0, 1),
            ( 1,-1), ( 1, 0), ( 1, 1)
        ];

        foreach (var (dX, dY) in deltas)
        {
            CoordXY neighbour = new(coord.X + dX, coord.Y + dY);
            if (neighbour.X >= 0 && neighbour.X < maxX && neighbour.Y >= 0 && neighbour.Y < maxY)
            {
                yield return neighbour;
            }
        }
    }

    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values and that they are lower in value than
    /// the maximum X and maximum Y specified in the MapContext object.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">MapContext object, commonly stores a char[,] map and a maxX and maxY integer.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllNeighbours(this CoordXY coord, MapContext mc)
        => coord.GetAllNeighbours(mc.MaxX, mc.MaxY);

    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values, that they are lower in value than
    /// the maximum X and maximum Y specified in the MapContext object and that they are stored as a '#' hash
    /// on the map.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">MapContext object, commonly stores a char[,] map and a maxX and maxY integer.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllHashNeighbours(this CoordXY coord, MapContext mc)
        => coord.GetAllNeighbours(mc).Where(nb => mc.Map[nb.Y, nb.X] is '#');

    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values, that they are lower in value than
    /// the maximum X and maximum Y specified in the MapContext object and that they are stored as a '.' dot
    /// on the map.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">MapContext object, commonly stores a char[,] map and a maxX and maxY integer.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllDotNeighbours(this CoordXY coord, MapContext mc)
        => coord.GetAllNeighbours(mc).Where(nb => mc.Map[nb.Y, nb.X] is '.');

    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values, that they are lower in value than
    /// the maximum X and maximum Y specified in the MapContext object and that they are stored as the
    /// specified character on the map.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">MapContext object, commonly stores a char[,] map and a maxX and maxY integer.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllNeighboursStoredAs(this CoordXY coord, MapContext mc, char c)
        => coord.GetAllNeighbours(mc).Where(nb => mc.Map[nb.Y, nb.X] == c);



    /// <summary>
    /// Returns the coordinates stored in the Neighbours Enumerable of the curent CoordXY coordinate and checks that they are
    /// within the limits of the map stored in the MapContext object
    /// </summary>
    /// <param name="coord">The CoordXY object</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] map and a maxX and maxY integer.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetValidNeighbours(this CoordXY coord, MapContext mc)
        => coord.Neighbours.Where(nb => nb.X >= 0 && nb.X < mc.MaxX && nb.Y >= 0 && nb.Y < mc.MaxY);

    /// <summary>
    /// Returns the coordinates stored in the Neighbours Enumerable of the current CoordXY coordinate and checks that 
    /// they are within the limits specified in the MapContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <param name="visited">A HashSet<CoordXY>.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetValidNeighbours(this CoordXY coord, MapContext mc, HashSet<CoordXY> visited)
        => coord.Neighbours.Where(nb => nb.X >= 0 && nb.X < mc.MaxX
                                     && nb.Y >= 0 && nb.Y < mc.MaxY
                                     && !visited.Contains(nb));

    /// <summary>
    /// Returns the coordinates stored in the Neighbours Enumerable of the current CoordXY coordinate and checks that 
    /// they are within the limits specified in the MapContext object and that they are not stored in a 2D bool array.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <param name="visitedArray">A 2D bool array (bool[,])</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetValidNeighbours(this CoordXY coord, MapContext mc, bool[,] visitedArray)
        => coord.Neighbours.Where(nb => nb.X >= 0 && nb.X < mc.MaxX
                                     && nb.Y >= 0 && nb.Y < mc.MaxY
                                     && !visitedArray[nb.Y, nb.X]);

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current CoordXY coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the MapContext object
    /// and that they are a '#' hash on the map stored in the MapContext object
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetHashNeighbours(this CoordXY coord, MapContext mc)
        => coord.GetValidNeighbours(mc).Where(nb => mc.Map[nb.Y, nb.X] is '#');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current CoordXY coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the MapContext object, 
    /// that they are a '#' hash on the map stored in the MapContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <param name="visited">The HashSet<CoordXY>.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetHashNeighbours(this CoordXY coord, MapContext mc, HashSet<CoordXY> visited)
        => coord.GetValidNeighbours(mc, visited).Where(nb => mc.Map[nb.Y, nb.X] is '#');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current CoordXY coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the MapContext object, 
    /// that they are a '#' hash on the map stored in the MapContext object and that they are not stored in a 
    /// 2D boolean array.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <param name="visitedArray">A 2D boolean array (bool[,]).</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetHashNeighbours(this CoordXY coord, MapContext mc, bool[,] visitedArray)
        => coord.GetValidNeighbours(mc, visitedArray).Where(nb => mc.Map[nb.Y, nb.X] is '#');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current CoordXY coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the MapContext object
    /// and that they are a '.' dot on the map stored in the MapContext object
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetDotNeighbours(this CoordXY coord, MapContext mc)
        => coord.GetValidNeighbours(mc).Where(nb => mc.Map[nb.Y, nb.X] is '.');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current CoordXY coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the MapContext object, 
    /// that they are a '.' dot on the map stored in the MapContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <param name="visited">The HashSet<CoordXY>.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetDotNeighbours(this CoordXY coord, MapContext mc, HashSet<CoordXY> visited)
        => coord.GetValidNeighbours(mc, visited).Where(nb => mc.Map[nb.Y, nb.X] is '.');

    /// <summary>
    /// Returns an Enumerable of the coordinates stored in the current CoordXY coordinate's Neighbours Enumerable.
    /// These coordinates are checked to be of positive value and within the limits specified in the MapContext object, 
    /// that they are a '#' hash on the map stored in the MapContext object and that they are not stored in a 
    /// 2D boolean array.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">The MapContext object, typically stores a char[,] Map and a MaxX and MaxY integer.</param>
    /// <param name="visitedArray">A 2D boolean array (bool[,]).</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetDotNeighbours(this CoordXY coord, MapContext mc, bool[,] visitedArray)
        => coord.GetValidNeighbours(mc, visitedArray).Where(nb => mc.Map[nb.Y, nb.X] is '.');

    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values, that they are lower in value than
    /// the maximum X and maximum Y specified in the MapContext object and that they are not stored in a visited HashSet.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">MapContext object, commonly stores a char[,] map and a maxX and maxY integer.</param>
    /// <param name="visited">The HashSet<CoordXY>.</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllValidNeighbours(this CoordXY coord, MapContext mc, HashSet<CoordXY> visited)
        => coord.GetAllNeighbours(mc).Where(nb => !visited.Contains(nb));

    /// <summary>
    /// Returns an Enumerable of CoordXY of all 8 neighbouring coordinates.
    /// The coordinates have been checked to have positive values, that they are lower in value than
    /// the maximum X and maximum Y specified in the MapContext object and that they are not stored in a 2D boolean array.
    /// </summary>
    /// <param name="coord">The CoordXY coordinate.</param>
    /// <param name="mc">MapContext object, commonly stores a char[,] map and a maxX and maxY integer.</param>
    /// <param name="visitedArray">A 2D boolean array (bool[,]).</param>
    /// <returns></returns>
    public static IEnumerable<CoordXY> GetAllValidNeighbours(this CoordXY coord, MapContext mc, bool[,] visitedArray)
        => coord.GetAllNeighbours(mc).Where(nb => !visitedArray[nb.Y, nb.X]);

    /* public static CoordXY MoveTo(this CoordXY coord, Direction dir) => dir switch
    {
        Direction.Up => coord.Up,
        Direction.Right => coord.Right,
        Direction.Down => coord.Down,
        Direction.Left => coord.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    }; */

}
