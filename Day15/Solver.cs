using AdventUtilities;

namespace Day15;

public static class Solver
{
    public static int PartOne(string filePath)
    {
        var (map, moves) = ProcessMap(filePath);
        List<IMovable> boxes = map.Index()
                                  .SelectMany(row => row.Item.Index()
                                                             .Select(col => col)
                                                             .Where(col => col.Item == 'O')
                                                             .Select(col => new Box(new Coord(row.Index, col.Index))))
                                  .Cast<IMovable>()
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

        Robot robot = new(robotCoord, map.ConvertJaggedTo2D(), moves, walls, boxes);

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

    public static (char[][], List<Direction>) ProcessMap(string filePath)
    {
        char[][] map = MapParser.ParseMap(filePath, out List<Direction> moves);

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
