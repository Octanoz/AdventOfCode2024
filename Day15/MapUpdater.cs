using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class MapUpdater(List<IMovable> boxes, List<Coord> walls)
{
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
                mapSpan.WipeCharBeforeRow(previous, 1);
                foreach (Box box in boxes.Where(box => box.OccupiesAnyRowInColBefore(current.Col, current.Row)))
                {
                    mapSpan.SetCharAt('O', box.Position);
                }

                RestoreWallsCol(mapSpan, current.Col);
                break;

            case Direction.Right:
                mapSpan.WipeCharAfterCol(previous, 1);
                foreach (Box box in boxes.Where(box => box.OccupiesAnyColInRowAfter(current.Row, current.Col)))
                {
                    mapSpan.SetCharAt('O', box.Position);
                }

                RestoreWallsRow(mapSpan, current.Row);
                break;

            case Direction.Down:
                mapSpan.WipeCharAfterRow(previous, 1);
                foreach (Box box in boxes.Where(box => box.OccupiesAnyRowInColAfter(current.Col, current.Row)))
                {
                    mapSpan.SetCharAt('O', box.Position);
                }

                RestoreWallsCol(mapSpan, current.Col);
                break;

            case Direction.Left:
                mapSpan.WipeCharBeforeCol(previous, 1);
                foreach (Box box in boxes.Where(box => box.OccupiesAnyColInRowBefore(current.Row, current.Col)))
                {
                    mapSpan.SetCharAt('O', box.Position);
                }

                RestoreWallsRow(mapSpan, current.Row);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(dir));
        }

        mapSpan.SetCharAt('.', previous);
        mapSpan.SetCharAt('@', current);
    }

    private void RestoreWallsCol(Span2D<char> mapSpan, int col)
    {
        foreach (var wall in walls.Where(w => w.Col == col))
        {
            mapSpan.SetCharAt('#', wall);
        }
    }

    private void RestoreWallsRow(Span2D<char> mapSpan, int row)
    {
        foreach (var wall in walls.Where(w => w.Row == row))
        {
            mapSpan.SetCharAt('#', wall);
        }
    }

    public int GetBoxCount() => boxes.Count;

    /*public void UpdateMapLeft(Span<char> rowSpan, Coord robotPosition)
    {
        HashSet<Coord> processedPositions = [];

        foreach (var box in boxes.OfType<Box>().Where(box => box.OccupiesAnyColInRowBefore(robotPosition.Row, robotPosition.Col)))
        {
            rowSpan[box.Position.Col] = 'O';
            processedPositions.Add(box.Position);
        }

        for (int i = 0; i < rowSpan.Length; i++)
        {
            Coord current = robotPosition with { Col = i };
            if (rowSpan[current.Col] is 'O' && !processedPositions.Contains(current))
            {
                rowSpan[current.Col] = '.';
            }
        }

        rowSpan[^1] = '.';
        rowSpan[^2] = '@';
    }

    public void UpdateMapRight(Span<char> rowSpan, Coord robotPosition)
    {
        HashSet<Coord> processedPositions = [];
        int offset = robotPosition.Col;

        foreach (var box in boxes.Where(box => box.Position.Row == robotPosition.Row && box.Position.Col > robotPosition.Col))
        {
            rowSpan[box.Position.Col - offset] = 'O';
            processedPositions.Add(box.Position);
        }

        for (int i = 1; i < rowSpan.Length; i++)
        {
            Coord current = robotPosition with { Col = i };
            Coord actual = robotPosition with { Col = i + offset };
            if (rowSpan[current.Col] is 'O' && !processedPositions.Contains(actual))
            {
                rowSpan[current.Col] = '.';
            }

        }

        rowSpan[0] = '.';
        rowSpan[1] = '@';
    }

    public void UpdateMapUp(Span2D<char> mapSpan, Coord robotPosition)
    {
        HashSet<Coord> processedPositions = [];
        Span<char> column = stackalloc char[mapSpan.Height];
        mapSpan.GetColumn(robotPosition.Col).CopyTo(column);

        foreach (var box in boxes.Where(box => box.Position.Col == robotPosition.Col
                                            && box.Position.Row < robotPosition.Row))
        {
            mapSpan[box.Position.Row, box.Position.Col] = 'O';
            processedPositions.Add(box.Position);
        }

        for (int i = 1; i < robotPosition.Row; i++)
        {
            Coord current = robotPosition with { Row = i };
            if (mapSpan[i, robotPosition.Col] == 'O' && !processedPositions.Contains(current))
            {
                mapSpan[i, robotPosition.Col] = '.';
            }
        }

        mapSpan[robotPosition.Row, robotPosition.Col] = '.';
        mapSpan[robotPosition.Row - 1, robotPosition.Col] = '@';
    }

    public void UpdateMapDown(Span2D<char> mapSpan, Coord robotPosition)
    {
        int offset = robotPosition.Row;
        HashSet<Coord> processedPositions = [];
        Span<char> column = stackalloc char[mapSpan.Height];
        mapSpan.GetColumn(robotPosition.Col).CopyTo(column);

        foreach (var box in boxes.Where(box => box.Position.Col == robotPosition.Col
                                            && box.Position.Row > robotPosition.Row))
        {
            mapSpan[box.Position.Row, box.Position.Col] = 'O';
            processedPositions.Add(box.Position);
        }

        for (int i = offset + 1; i < column.Length; i++)
        {
            Coord current = robotPosition with { Row = i };
            if (mapSpan[i, robotPosition.Col] == 'O' && !processedPositions.Contains(current))
            {
                mapSpan[i, robotPosition.Col] = '.';
            }
        }

        mapSpan[robotPosition.Row, robotPosition.Col] = '.';
        mapSpan[robotPosition.Row + 1, robotPosition.Col] = '@';
    }*/

}
