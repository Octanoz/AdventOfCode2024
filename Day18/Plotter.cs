namespace Day18;

using AdventUtilities;

using CommunityToolkit.HighPerformance;

public static class Plotter
{
    public static int PartOne(string filePath, bool isTest = false)
    {
        if (isTest)
        {
            return ShortestRoutePartial(filePath, 7, 7, 12);
        }

        return ShortestRoutePartial(filePath, 71, 71, 1024);
    }

    public static int PartTwo(string filePath, bool isTest = false)
    {
        var map = PlotMap(filePath, 71, 71, out List<CoordXY> holes);
        var mapSpan = map.AsSpan2D();
        int maxX = mapSpan.Width, maxY = mapSpan.Height;

        List<CoordXY> initialHoles = holes[..1025];
        FirstBlocked(mapSpan, initialHoles, maxX, maxY);

        return ShortestRoutePartial(filePath, 71, 71, 1024);
    }

    public static void FirstBlocked(Span2D<char> mapSpan, List<CoordXY> holes, int maxX, int maxY)
    {
        foreach (var hole in holes)
        {
            mapSpan.SetCharAt('#', hole);
        }
    }

    private static int FindBlockingByte(Span2D<char> mapSpan, List<CoordXY> holes, int maxX, int maxY)
    {
        Dictionary<CoordXY, CoordXY> criticalsFromExit = FindCriticalPaths(mapSpan, maxX, maxY);
        var criticalPaths = criticalsFromExit.Concat(FindCriticalPaths(mapSpan, maxX, maxY, false))
                                             .ToDictionary(x => x.Key, x => x.Value);

        for (int i = 0; i < holes.Count; i++)
        {
            CoordXY current = holes[i];
            mapSpan.SetCharAt('#', current);
        }
    }

    private static Dictionary<CoordXY, CoordXY> FindCriticalPaths(Span2D<char> mapSpan, int maxX, int maxY, bool fromExit = true)
    {
        CoordXY initial = CoordXY.Zero;
        CoordXY exit = new(mapSpan.Width - 1, mapSpan.Height - 1);
        Dictionary<CoordXY, CoordXY> criticalPaths = [];
        HashSet<CoordXY> visited = [];

        //Check from exit first
        if (fromExit)
        {
            initial = exit;
        }

        Queue<CoordXY> paths = [];
        CoordXY next = CoordXY.Zero;
        paths.Enqueue((initial));
        visited.Add(initial);

        while (paths.Count is not 0 && criticalPaths.Count < 15)
        {
            var current = paths.Dequeue();
            if (current == CoordXY.Zero && fromExit || current == exit && !fromExit)
            {
                continue;
            }

            int dotCounter = 0;
            foreach (var neighbour in current.Neighbours.Where(nb => WithinBounds(nb, maxX, maxY) && !visited.Contains(nb)))
            {
                if (mapSpan.GetValueAtXY(neighbour.X, neighbour.Y) == '.' && visited.Add(neighbour))
                {
                    dotCounter++;
                    if (next == CoordXY.Zero || next == current)
                    {
                        next = neighbour;
                    }

                    paths.Enqueue(current);
                }
            }

            if (dotCounter is 1)
            {
                CoordXY startCrit = current;
                criticalPaths[startCrit] = current;

                while (next != CoordXY.Invalid)
                {
                    next = GetSinglePath(mapSpan, current, visited, maxX, maxY);
                    if (visited.Add(next))
                    {
                        current = next;
                    }
                }

                criticalPaths[startCrit] = current;
                paths.Enqueue(current);
            }
        }

        return criticalPaths;
    }

    private static void CheckNewBlock(Span2D<char> mapSpan, CoordXY block, int maxX, int maxY)
    {
        int[] deltaY = [-1, -1, -1, 0, 0, 1, 1, 1];
        int[] deltaX = [1, 0, -1, -1, 1, -1, 0, 1];

        for (int i = 0; i < deltaY.Length; i++)
        {
            CoordXY current = new(block.X + deltaX[i], block.Y + deltaY[i]);
            char target = mapSpan.GetValueAtXY(current.X, current.Y);
            if (target is '.' && IsChokePoint(mapSpan, current))
            {
                SealPath(mapSpan, current);
            }
        }
    }

    private static bool IsChokePoint(Span2D<char> mapSpan, CoordXY coord)
    {
        int dotCounter = 0;
        foreach (var neighbour in coord.Neighbours)
        {
            if (mapSpan.GetValueAtXY(neighbour.X, neighbour.Y) is '.')
            {
                dotCounter++;
            }
        }

        if (dotCounter is 1)
            return true;

        return false;
    }

    private static void SealPath(Span2D<char> mapSpan, CoordXY coord)
    {
        int maxX = mapSpan.Width;
        int maxY = mapSpan.Height;
        CoordXY next = CoordXY.Zero;
        while (next != CoordXY.Invalid)
        {
            next = GetSinglePath(mapSpan, coord, [], maxX, maxY);
            if (next != CoordXY.Invalid)
            {
                mapSpan.SetCharAt('#', coord);
                coord = next;
            }
        }
    }

    private static CoordXY GetSinglePath(Span2D<char> mapSpan, CoordXY current, HashSet<CoordXY> visited, int maxX, int maxY, bool fromExit = true)
    {
        int dotCounter = 0;
        CoordXY next = CoordXY.Zero;

        foreach (var neighbour in current.Neighbours.Where(nb => WithinBounds(nb, maxX, maxY) && !visited.Contains(nb)))
        {
            if (mapSpan.GetValueAtXY(neighbour.X, neighbour.Y) is '.')
            {
                next = neighbour;
                dotCounter++;
            }
        }

        return dotCounter is 1 ? next : CoordXY.Invalid;
    }

    private static char[,] PlotMap(string filePath, int maxX, int maxY, out List<CoordXY> holes)
    {
        holes = [];
        char[,] map = GridExtensions.New2DGridBlank(maxY, maxX);
        var mapSpan = map.AsSpan2D();
        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine() ?? "0,0";
            holes.Add(Array.ConvertAll(line.Split(','), int.Parse) switch
            {
                var arr => new CoordXY(arr[0], arr[1])
            });
        }

        return map;
    }

    private static char[,] PlotMapPartial(string filePath, int maxX, int maxY, int bytes, out List<CoordXY> holes)
    {
        holes = [];
        char[,] map = GridExtensions.New2DGridBlank(maxY, maxX);
        var mapSpan = map.AsSpan2D();
        using StreamReader sr = new(filePath);
        for (int i = 0; i < bytes; i++)
        {
            string? line = sr.ReadLine() ?? "0,0";
            holes.Add(Array.ConvertAll(line.Split(','), int.Parse) switch
            {
                var arr => new CoordXY(arr[0], arr[1])
            });
        }

        foreach (var hole in holes)
        {
            mapSpan.SetCharAt('#', new CoordXY(hole.X, hole.Y));
        }

        return map;
    }

    public static int ShortestRoutePartial(string filePath, int maxX, int maxY, int bytes)
    {
        var map = PlotMapPartial(filePath, maxX, maxY, bytes, out List<CoordXY> holes);
        var mapSpan = map.AsSpan2D();
        // map.Draw2DGridTightXY();

        HashSet<CoordXY> visited = [CoordXY.Zero];
        visited.UnionWith(holes);

        Queue<(CoordXY, List<CoordXY>)> paths = [];
        paths.Enqueue((CoordXY.Zero, []));
        CoordXY target = new(maxX - 1, maxY - 1);
        while (paths.Count is not 0)
        {
            var (current, coordList) = paths.Dequeue();

            if (current == target)
            {
                foreach (var coord in coordList)
                {
                    map[coord.Y, coord.X] = 'O';
                }

                map.Draw2DGridTightXY();
                return coordList.Count;
            }

            mapSpan.SetCharAt('X', current);
            // mapSpan.Draw2DGridTightXY();
            Console.WriteLine();

            foreach (var neighbour in current.Neighbours.Where(nb => WithinBounds(nb, maxX, maxY) && map[nb.Y, nb.X] is not '#'))
            {
                if (visited.Add(neighbour))
                {
                    List<CoordXY> newList = new(coordList) { neighbour };
                    paths.Enqueue((neighbour, newList));
                }
            }

            mapSpan.SetCharAt('.', current);
        }

        return 0;
    }

    private static bool WithinBounds(CoordXY c, int maxX, int maxY) => c.X >= 0 && c.X < maxX
                                                                    && c.Y >= 0 && c.Y < maxY;
}
