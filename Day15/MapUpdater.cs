using System.Collections.Frozen;
using AdventUtilities;
using CommunityToolkit.HighPerformance;
using System.Linq;

namespace Day15;

public class MapUpdater(List<Box> boxes)
{
    private static HashSet<Coord> wallCache = [];
    public void UpdateBoxesMap(Span2D<char> mapSpan, Coord previous, Coord current, Direction dir, bool boxMoved)
    {
        if (!boxMoved)
        {
            mapSpan[previous.Row, previous.Col] = '.';
            mapSpan[current.Row, current.Col] = '@';

            return;
        }

        switch (dir)
        {
            case Direction.Up:
                if (wallCache.Contains(current.Up))
                    break;

                WipeColBeforeCoordRow(mapSpan, current);
                break;

            case Direction.Right:
                if (wallCache.Contains(current.Right))
                    break;

                WipeRowAfterCoordCol(mapSpan, current);
                break;

            case Direction.Down:
                if (wallCache.Contains(current.Down))
                    break;

                WipeColAfterCoordRow(mapSpan, current);
                break;

            case Direction.Left:
                if (wallCache.Contains(current.Left))
                    break;

                WipeRowBeforeCoordCol(mapSpan, current);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(dir));
        }

        foreach (Box box in boxes.Where(box => box.Moved))
        {
            mapSpan.SetCharAt('O', box.Position);
            box.ResetMovedState();
        }

        mapSpan.SetCharAt('.', previous);
        mapSpan.SetCharAt('@', current);
    }

    private static void WipeColBeforeCoordRow(Span2D<char> mapSpan, Coord coord)
    {
        var wallCoords = mapSpan.StoreWallCoordsInCol(coord.Col);
        if (!wallCache.Contains(wallCoords.Skip(1).First()))
        {
            wallCache.UnionWith(wallCoords);
        }
        var lastWall = wallCoords.Where(wall => wall.Row < coord.Row).DefaultIfEmpty(Coord.Zero with { Col = coord.Col }).MaxBy(wall => wall.Row)!.Row;
        if (lastWall == coord.Up.Row)
            return;

        var mapSlice = mapSpan.Slice(lastWall + 1, coord.Col, coord.Row - (lastWall + 1), 1);
        mapSlice.Fill('.');
    }

    private static void WipeColAfterCoordRow(Span2D<char> mapSpan, Coord coord)
    {
        var wallCoords = mapSpan.StoreWallCoordsInCol(coord.Col);
        if (!wallCache.Contains(wallCoords.Skip(1).First()))
        {
            wallCache.UnionWith(wallCoords);
        }

        var firstWall = wallCoords.Where(wall => wall.Row > coord.Row).DefaultIfEmpty(coord with { Row = mapSpan.Height - 1 }).MinBy(wall => wall.Row)!.Row;
        if (firstWall == coord.Down.Row)
            return;

        var mapSlice = mapSpan.Slice(coord.Row, coord.Col, firstWall - 1 - coord.Row, 1);
        mapSlice.Fill('.');
    }

    private static void WipeRowBeforeCoordCol(Span2D<char> mapSpan, Coord coord)
    {
        var rowSpan = mapSpan.GetRowSpan(coord.Row)[..coord.Col];
        int index = rowSpan.LastIndexOf('#');
        if (index == coord.Left.Col)
        {
            wallCache.Add(coord.Left);
            return;
        }

        rowSpan = rowSpan[(index + 1)..];
        rowSpan.Fill('.');
    }

    private static void WipeRowAfterCoordCol(Span2D<char> mapSpan, Coord coord)
    {
        var rowSpan = mapSpan.GetRowSpan(coord.Row)[(coord.Col + 1)..];
        int index = rowSpan.IndexOf('#');
        if (index == coord.Right.Col)
        {
            wallCache.Add(coord.Right);
            return;
        }

        rowSpan = rowSpan[..index];
        rowSpan.Fill('.');
    }

    public void RestoreMovedBoxes(Span2D<char> mapSpan)
    {
        foreach (Box box in boxes.Where(box => box.Moved))
        {
            mapSpan.SetCharAt('O', box.Position);
            box.ResetMovedState();
        }
    }

    public int GetBoxCount() => boxes.Count;
}
