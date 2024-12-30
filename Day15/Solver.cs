using AdventUtilities;

namespace Day15;

public static class Solver
{
    public static int PartOne(string filePath)
    {
        var (map, moves) = ProcessMap(filePath);
        List<Box> boxes = map.Index()
                                  .SelectMany(row => row.Item.Index()
                                                             .Select(col => col)
                                                             .Where(col => col.Item == 'O')
                                                             .Select(col => new Box(new Coord(row.Index, col.Index))))
                                  .ToList();

        List<Coord> walls = map.Index()
                               .SelectMany(row => row.Item.Index()
                                                          .Select(col => col)
                                                          .Where(col => col.Item == '#')
                                                          .Select(col => new Coord(row.Index, col.Index)))
                               .ToList();

        Coord robotCoord = map.Index()
                              .SelectMany(row => row.Item.Index()
                                                         .Select(col => col)
                                                         .Where(col => col.Item == '@')
                                                         .Select(col => new Coord(row.Index, col.Index)))
                              .First();

        Robot robot = new(robotCoord, map.ConvertJaggedTo2D(), moves, boxes);

        (boxes, map) = robot.ProcessMoves();

        for (int i = 0; i < map.Length; i++)
        {
            int index = Array.IndexOf(map[i], 'O');
            while (index is not -1)
            {
                map[i][index] = '.';
                index = Array.IndexOf(map[i], 'O');
            }

            foreach (Box box in boxes.Where(box => box.OccupiesAnyColInRowAfter(i, 0)))
            {
                map[i][box.Position.Col] = 'O';
            }
        }

        map[robot.Position.Row][robot.Position.Col] = '@';

        map.DrawJaggedGridTight();

        return boxes.OfType<Box>().Sum(box => box.Position.Row * 100 + box.Position.Col);

    }

    public static int PartTwo(string filePath)
    {
        var (map, moves) = ProcessMapWide(filePath);
        List<WideBox> boxes = map.Index()
                                  .SelectMany(row => row.Item.Index()
                                                             .Select(col => col)
                                                             .Where(col => col.Item == '[')
                                                             .Select(col => new WideBox(new Coord(row.Index, col.Index))))
                                  .ToList();

        List<Coord> walls = map.Index()
                               .SelectMany(row => row.Item.Index()
                                                          .Select(col => col)
                                                          .Where(col => col.Item == '#')
                                                          .Select(col => new Coord(row.Index, col.Index)))
                               .ToList();

        Coord robotCoord = map.Index()
                              .SelectMany(row => row.Item.Index()
                                                         .Select(col => col)
                                                         .Where(col => col.Item == '@')
                                                         .Select(col => new Coord(row.Index, col.Index)))
                              .First();

        RoboBro robot = new(robotCoord, map.ConvertJaggedTo2D(), moves, boxes);

        (boxes, map) = robot.ProcessMoves();

        map.DrawJaggedGridTight();

        return boxes.OfType<WideBox>().Sum(box => CalculateBoxValue(map, box));
    }

    public static int CalculateBoxValue(char[][] map, WideBox box)
    {
        int currentRow = box.LeftWing.Row;
        int colLeft = box.LeftWing.Col;

        return currentRow * 100 + colLeft;
    }

    public static (char[][], List<Direction>) ProcessMap(string filePath)
    {
        char[][] map = MapParser.ParseMap(filePath, out List<Direction> moves);

        return (map, moves);
    }

    public static (char[][], List<Direction>) ProcessMapWide(string filePath)
    {
        char[][] map = MapParser.ParseMap2(filePath, out List<Direction> moves);

        return (map, moves);
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
