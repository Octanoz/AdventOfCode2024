namespace Day18;

using System.Net.NetworkInformation;

using AdventUtilities;

using CommunityToolkit.HighPerformance;

using MapContext = (char[,] Map, int MaxX, int MaxY);

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

    public static (int, int) PartTwo(string filePath, bool isTest = false)
    {
        //Copy from the method use in Part One. Create the map and parse the input.
        var map = PlotMap(filePath, 71, 71, out List<CoordXY> holes);
        var mapSpan = map.AsSpan2D(); //using Span2D from the community toolkit
        int maxX = mapSpan.Width, maxY = mapSpan.Height;


        /* Store the map and it's dimensions in a Context for the many, many, many times
           they are going to be used in the call to a method.*/
        MapContext mc = new(map, maxX, maxY);

        //Continue from the known valid sate at the end of part 1 plot the holes up to then on the current map
        List<CoordXY> initialHoles = holes[..1025];
        FirstBlocked(mc, initialHoles);
        PlugHoles(mc, out int initialDots); //Close any open spaces only diagonally

        var (x, y) = ProcessBlockUpdates(mc, holes, initialDots);

        return (x, y);
    }

    //For use with the example input only
    public static (int, int) PartTwoTest(string filePath, bool isTest = false)
    {
        var map = PlotMap(filePath, 7, 7, out List<CoordXY> holes);
        var mapSpan = map.AsSpan2D();
        int maxX = mapSpan.Width, maxY = mapSpan.Height;

        /*Store the map and its dimensions in a Context to make it easier to use them in 
          every method call in this class. Clinging to a static class is so smart. */
        MapContext mc = new(map, maxX, maxY);

        /*Slice of the holes list that are known not to block the exit since they are 
          used in part 1 as well. Mark holes on the new map and start processing the rest.*/
        List<CoordXY> initialHoles = holes[..12];
        FirstBlocked(mc, initialHoles);
        PlugHoles(mc, out int initialDots);

        var (x, y) = ProcessBlockUpdates(mc, holes, initialDots);

        return (x, y);
    }

    private static CoordXY ProcessBlockUpdates(MapContext mc, List<CoordXY> holes, int initialDots)
    {
        var map = mc.Map.AsSpan2D();
        holes = UpdateHoles(map, holes); //Double-check if any of the first holes were already marked.

        /*If halfway point on Y was reached before storing a magic number of paths check only the paths and 
          the BFS route. Before that, set a threshold of open spaces converted to holes to trigger refresh.*/
        Dictionary<CoordXY, (CoordXY LastTile, int TilesAmount)> criticalPaths = FindCriticalPaths(mc, out bool countPaths);
        List<CoordXY> lastPath = [];

        CoordXY start = CoordXY.Zero;
        CoordXY exit = new(map.Width - 1, map.Height - 1);

        int currentDots = initialDots;
        int index = 0;
        int dotsBlocked = 0;

        int refreshThreshold = 0;
        if (!countPaths)
            refreshThreshold = SetLimit(initialDots);
        else if (!CanFinish(mc, lastPath, out lastPath))
        {
            return holes[0];
        }

        CoordXY currentHole = CoordXY.Invalid;
        while (index < holes.Count)
        {
            currentHole = holes[index++];
            List<CoordXY> closedList = CheckNewBlock(mc, currentHole, out int tilesBlocked);
            dotsBlocked += tilesBlocked;
            if (countPaths && lastPath.Exists(c => c == currentHole))
            {
                if (!CanFinish(mc, lastPath, out lastPath))
                {
                    return currentHole;
                }
            }

            if (closedList.Count is 0)
            {
                continue;
            }

            if (countPaths)
            {
                int currentPaths = criticalPaths.Count;
                foreach (var newBlock in closedList)
                {
                    CoordXY currentPath = criticalPaths.FirstOrDefault(path => path.Value.Item1 == newBlock).Key;
                    if (currentPath is not null)
                    {
                        dotsBlocked += criticalPaths[currentPath].TilesAmount;
                        criticalPaths.Remove(currentPath);
                    }
                }

                int pathsUpdated = currentPaths - criticalPaths.Count;
                if (pathsUpdated < closedList.Count)
                {
                    criticalPaths = UpdatePaths(mc, criticalPaths);
                }

                continue;
            }

            dotsBlocked += tilesBlocked;
            if (dotsBlocked > refreshThreshold)
            {
                if (!CanFinish(mc, lastPath, out lastPath))
                {
                    break;
                }
                currentDots -= dotsBlocked;
                criticalPaths = FindCriticalPaths(mc, out bool updatedCountPaths);
                if (criticalPaths.Count is 0)

                    if (!updatedCountPaths)
                        refreshThreshold = SetLimit(currentDots);
                    else
                        countPaths = updatedCountPaths;
            }

        }

        return currentHole;
    }

    private static bool CanFinish(MapContext mc, List<CoordXY> lastPath, out List<CoordXY> previousPath)
    {
        if (lastPath.Count > 0 && !lastPath.Exists(coord => mc.Map[coord.Y, coord.X] is '#'))
        {
            previousPath = lastPath;
            return true;
        }

        var map = mc.Map.AsSpan2D();
        CoordXY target = new(mc.MaxX - 1, mc.MaxY - 1);
        HashSet<CoordXY> visited = [CoordXY.Zero];
        Queue<(CoordXY, List<CoordXY>)> paths = [];

        paths.Enqueue((CoordXY.Zero, []));
        while (paths.Count is not 0)
        {
            var (current, currentList) = paths.Dequeue();
            if (current == target)
            {
                previousPath = currentList;
                return true;
            }

            foreach (var neighbour in current.GetDotNeighbours(mc))
            {
                if (visited.Add(neighbour))
                {
                    paths.Enqueue((neighbour, new(currentList) { neighbour }));
                }
            }
        }

        previousPath = [];

        return false;
    }

    private static int SetLimit(int initialDots) => (int)(initialDots * 0.3);

    private static List<CoordXY> UpdateHoles(Span2D<char> map, List<CoordXY> holes)
    {
        CoordXY currentHole = holes[0];
        while (map[currentHole.Y, currentHole.X] is '#')
        {
            holes.RemoveAt(0);
            currentHole = holes[0];
        }

        return holes;
    }

    public static void FirstBlocked(MapContext mc, List<CoordXY> holes)
    {
        var mapSpan = mc.Map.AsSpan2D();
        foreach (var hole in holes)
        {
            mapSpan.SetCharAt('#', hole);
        }
    }

    private static Dictionary<CoordXY, (CoordXY, int)> FindCriticalPaths(MapContext mc, out bool countPaths)
    {
        var map = mc.Map.AsSpan2D();
        countPaths = false;
        CoordXY initial = CoordXY.Zero;
        CoordXY exit = new(map.Width - 1, map.Height - 1);
        Dictionary<CoordXY, (CoordXY, int)> criticalPathsFromStart = [];
        Dictionary<CoordXY, (CoordXY, int)> criticalPathsFromEnd = [];

        Queue<(CoordXY, bool)> paths = [];
        paths.Enqueue((initial, true));
        paths.Enqueue((exit, false));
        HashSet<CoordXY> visited = [initial, exit];

        int tilesCounter = 0;
        CoordXY next = CoordXY.Zero;
        while (paths.Count is not 0 && criticalPathsFromStart.Count <= 15 && criticalPathsFromEnd.Count <= 15)
        {
            var (current, fromStart) = paths.Dequeue();
            if (fromStart && current.Y > mc.MaxY / 2 || !fromStart && current.Y < mc.MaxY / 2)
            {
                countPaths = true;
                continue;
            }

            if (fromStart && criticalPathsFromStart.Count is 15 || !fromStart && criticalPathsFromEnd.Count is 15)
            {
                countPaths = true;
                continue;
            }

            if (IsChokePoint(mc, current, visited))
            {
                CoordXY startCrit = current;
                if (fromStart)
                {
                    if (!criticalPathsFromStart.TryAdd(startCrit, (current, 1)))
                    {
                        continue;
                    }
                }
                else
                    if (!criticalPathsFromEnd.TryAdd(startCrit, (current, 1)))
                {
                    continue;
                }

                tilesCounter = 1;
                while (next != CoordXY.Invalid)
                {
                    next = GetSinglePath(mc, current, visited);
                    if (next != CoordXY.Invalid && visited.Add(next))
                    {
                        tilesCounter++;
                        current = next;
                    }
                }
                next = CoordXY.Zero;

                if (fromStart)
                {
                    criticalPathsFromStart[startCrit] = (current, tilesCounter);
                }
                else
                    criticalPathsFromEnd[startCrit] = (current, tilesCounter);

                if (current != startCrit)
                {
                    paths.Enqueue((current, fromStart));
                    continue;
                }
            }

            foreach (var neighbour in current.GetDotNeighbours(mc))
            {
                if (visited.Add(neighbour))
                {
                    paths.Enqueue((neighbour, fromStart));
                }
            }
        }

        return criticalPathsFromStart.Concat(criticalPathsFromEnd).ToDictionary(x => x.Key, x => x.Value);
    }

    private static Dictionary<CoordXY, (CoordXY, int)> UpdatePaths(MapContext mc, Dictionary<CoordXY, (CoordXY LastTile, int Tiles)> paths)
    {
        foreach (var kvp in paths)
        {
            //Prevent creating new Dictionaries if this could be an off by 1 error in the counting
            int currentTiles = paths[kvp.Key].Tiles + 1;
            var (newLastTile, tilesCount) = NavigatePath(mc, kvp.Key);
            switch (tilesCount)
            {
                case 0: // no valid path
                    paths.Remove(kvp.Key);
                    break;
                case -1: // dead end
                    SealPath(mc, newLastTile);
                    paths.Remove(kvp.Key);
                    break;
                default:
                    if (tilesCount > currentTiles)
                        return FindCriticalPaths(mc, out _);

                    paths[kvp.Key] = (newLastTile, tilesCount);
                    break;
            }
        }

        return paths;
    }

    private static (CoordXY, int) NavigatePath(MapContext mc, CoordXY start)
    {
        var map = mc.Map.AsSpan2D();
        HashSet<CoordXY> visited = [start];
        Queue<CoordXY> paths = [];
        foreach (var neighbour in start.GetDotNeighbours(mc))
        {
            if (visited.Add(neighbour))
                paths.Enqueue(neighbour);
        }

        int tilesCount = 0;
        CoordXY current = CoordXY.Zero;
        while (paths.Count is not 0)
        {
            current = paths.Dequeue();
            CoordXY next = GetSinglePath(mc, current, visited);
            if (next == CoordXY.Invalid)
                continue;
            else
            {
                tilesCount++;
                visited.Add(next);
                paths.Enqueue(next);
            }
        }

        if (!current.Neighbours.Any(nb => WithinBounds(nb, mc)
                                            && (!visited.Contains(nb)
                                            || mc.Map[nb.Y, nb.X] is not '#')))
        {
            tilesCount = -1;
        }

        return tilesCount is 0 ? (start, 0) : (current, tilesCount);
    }

    private static List<CoordXY> CheckNewBlock(MapContext mc, CoordXY block, out int totalBlocked)
    {
        var map = mc.Map.AsSpan2D();
        map.SetCharAt('#', block); //Mark the hole
        totalBlocked = 1;

        CoordXY start = CoordXY.Zero;
        CoordXY exit = new(map.Height - 1, map.Width - 1);
        List<CoordXY> closedPathTiles = [];

        //Check for neighbouring coordinates that have only one open space neighbouring cell, i.e. a dead end.
        foreach (var neighbour in block.GetDotNeighbours(mc)
                                       .Where(nb => IsChokePoint(mc, nb) && nb != exit && nb != start))
        {
            var (lastBlocked, amountBlocked) = SealPath(mc, neighbour);
            closedPathTiles.Add(lastBlocked);
            totalBlocked += amountBlocked;
        }

        return closedPathTiles;
    }

    //Dead end
    private static bool IsChokePoint(MapContext mc, CoordXY coord) => coord.GetDotNeighbours(mc).Count() is 1;

    //Dead end when using a visited Hashset during plotting.
    private static bool IsChokePoint(MapContext mc, CoordXY coord, HashSet<CoordXY> visited)
        => (coord.GetDotNeighbours(mc, visited).Count() is 1);

    //Keep closing a path while there is only one valid way to go.
    private static (CoordXY, int) SealPath(MapContext mc, CoordXY coord)
    {
        var map = mc.Map.AsSpan2D();
        map.SetCharAt('#', coord);
        int steps = 1;
        HashSet<CoordXY> visited = [coord];
        Queue<CoordXY> paths = [];
        foreach (var neighbour in coord.GetDotNeighbours(mc))
        {
            if (visited.Add(neighbour))
                paths.Enqueue(neighbour);
        }

        while (paths.Count is not 0)
        {
            coord = paths.Dequeue();
            CoordXY next = GetSinglePath(mc, coord, visited);
            if (next == CoordXY.Invalid)
            {
                continue;
            }

            steps++;
            map.SetCharAt('#', coord);
            paths.Enqueue(next);
        }

        return (coord, steps);
    }

    //Only one way to go? Output the coordinate else an invalid one.
    private static CoordXY GetSinglePath(MapContext mc, CoordXY current, HashSet<CoordXY> visited)
    {
        CoordXY[] validNeighbours = current.GetDotNeighbours(mc, visited).ToArray();

        return validNeighbours.Length is 1 ? validNeighbours[0] : CoordXY.Invalid;
    }

    //Setup map and parse input data.
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

    //Mark any open spaces only reachable diagonally as holes as well.
    private static void PlugHoles(MapContext mc, out int totalDots)
    {
        var map = mc.Map.AsSpan2D();
        CoordXY current = CoordXY.Zero;
        totalDots = 0;
        for (int y = 0; y < map.Height; y++)
        {
            bool isFirstHolePlugged = false;
            int dotCounter = 0;
            if (map[y, 0] is '.')
                dotCounter++;

            for (int x = 1; x < map.Width; x++)
            {
                if (x == map.Width - 1
                && map[y, x] is '.'
                && dotCounter is 0
                && AreAllNeighboursHoles(mc, current))
                {
                    map.SetCharAt('#', current);
                    continue;
                }

                if (map[y, x] == '.')
                {
                    dotCounter++;
                }
                else
                {
                    if (dotCounter is 0 or > 1)
                    {
                        totalDots += dotCounter;
                        dotCounter = 0;
                        continue;
                    }

                    current = new(x - 1, y);
                    if (AreAllNeighboursHoles(mc, current))
                    {
                        if (current.X is 0)
                            isFirstHolePlugged = true;

                        map.SetCharAt('#', current);
                        totalDots++;
                    }

                    dotCounter = 0;
                }
            }

            //Slice the map if there is a column that's only holes to increase performance.
            if (isFirstHolePlugged)
            {
                map = SliceMap(map, y);
            }
        }
    }

    private static Span2D<char> SliceMap(Span2D<char> map, int currentRow)
    {
        int columns = 0;
        for (int i = 0; i < map.Width; i++)
        {
            if (i > columns)
                break;

            int index = 0;
            foreach (var cell in map.GetColumn(i))
            {
                if (index <= currentRow)
                {
                    index++;
                    continue;
                }

                if (cell is '.')
                    break;

                index++;
            }

            if (index < map.Height)
            {
                break;
            }

            columns++;
        }

        if (columns is 0)
        {
            return map;
        }

        return map.Slice(0, columns, map.Height, map.Width - columns);
    }

    private static bool AreAllNeighboursHoles(MapContext mc, CoordXY coord)
    {
        var map = mc.Map.AsSpan2D();

        foreach (var neighbour in coord.Neighbours.Where(nb => WithinBounds(nb, mc)))
        {
            if (map.GetValueAtXY(neighbour.X, neighbour.Y) == '.')
            {
                return false;
            }
        }

        return true;
    }



    private static bool WithinBounds(CoordXY c, int maxX, int maxY) => c.X >= 0 && c.X < maxX
                                                                    && c.Y >= 0 && c.Y < maxY;

    private static bool WithinBounds(CoordXY c, MapContext mc) => c.X >= 0 && c.X < mc.MaxX
                                                               && c.Y >= 0 && c.Y < mc.MaxY;
}
