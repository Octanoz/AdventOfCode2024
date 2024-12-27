using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class MapUpdater(List<Box> boxes)
{
    private readonly List<Box> boxes = boxes;

    public void UpdateMapLeft(Span<char> rowSpan, Coord robotPosition)
    {
        HashSet<Coord> processedPositions = [];

        foreach (var box in boxes.Where(box => box.Position.Row == robotPosition.Row && box.Position.Col < robotPosition.Col))
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
    }

}
