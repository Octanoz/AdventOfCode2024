namespace Day20;

using AdventUtilities;

using CommunityToolkit.HighPerformance;

using TableContext = (char[,] Table, int MaxRow, int MaxCol);

public static class Mapper
{
    public static Dictionary<int, int> PartOne(string[] input, bool isTest = false)
    {
        char[,] charGrid = GridExtensions.New2DGridWithDimensions<char>(input, out int maxRow, out int maxCol);
        TableContext tc = new(charGrid, maxRow, maxCol);
        int[,] numGrid = CharToInt2D(tc);

        int regularSteps = RateCells(tc, numGrid, out Coord start, out Coord exit);

        if (isTest)
        {
            return FindCheatsTest(tc, numGrid, regularSteps, start, exit);
        }

        return FindCheats(tc, numGrid, regularSteps, start, exit);
    }

    private static int[,] CharToInt2D(TableContext tc)
    {
        int[,] grid = new int[tc.MaxRow, tc.MaxCol];

        for (int row = 0; row < tc.MaxRow; row++)
        {
            for (int col = 0; col < tc.MaxCol; col++)
            {
                char currentChar = tc.Table[row, col];
                grid[row, col] = currentChar is '#' ? -1 : 0;
            }
        }

        return grid;
    }

    private static Dictionary<int, int> FindCheats(TableContext tc, Span2D<int> numSpan, int regularSteps, Coord start, Coord exit)
    {
        Dictionary<int, int> cheats = [];
        Span2D<char> grid = tc.Table;
        bool[,] visited = tc.Table.Generate2DBool();

        Queue<(Coord, int)> paths = [];
        paths.Enqueue((start, 0));

        while (paths.Count is not 0)
        {
            var (current, steps) = paths.Dequeue();

            int stepsToExit = numSpan.GetValueAt(current);
            foreach (var neighbour in current.GetDotNeighbours(tc, visited))
            {
                MarkVisited(visited, neighbour);
                paths.Enqueue((neighbour, steps + 1));
            }

            int optimalStepsFromCurrent = numSpan.GetValueAt(current);
            foreach (var neighbour in current.GetHashNeighbours(tc, visited))
            {
                MarkVisited(visited, neighbour);
                Coord move = neighbour - current;
                int cheatingSteps = StepsWhenCheating(tc, numSpan, current, move);

                /*1.Negative steps: less is, surprisingly, not good
                  2. The reduced total number of steps is the same or more than the regular optimal path
                  (cheatingSteps includes the 2 steps to get to it from the current location)*/
                if (cheatingSteps is -1 || steps + cheatingSteps >= regularSteps)
                    continue;

                int timeImproved = regularSteps - cheatingSteps - steps;
                if (timeImproved < 100)
                    continue;

                cheats[timeImproved] = cheats.TryGetValue(timeImproved, out int frequency) ? frequency + 1 : 1;
            }
        }

        return cheats;
    }

    private static Dictionary<int, int> FindCheatsTest(TableContext tc, Span2D<int> numSpan, int regularSteps, Coord start, Coord exit)
    {
        Dictionary<int, int> cheats = [];
        Span2D<char> grid = tc.Table;
        bool[,] visited = tc.Table.Generate2DBool();

        Queue<(Coord, int)> paths = [];
        paths.Enqueue((start, 0));

        while (paths.Count is not 0)
        {
            var (current, steps) = paths.Dequeue();

            int stepsToExit = numSpan.GetValueAt(current);
            foreach (var neighbour in current.GetDotNeighbours(tc, visited))
            {
                MarkVisited(visited, neighbour);
                paths.Enqueue((neighbour, steps + 1));
            }

            int optimalStepsFromCurrent = numSpan.GetValueAt(current);
            foreach (var neighbour in current.GetHashNeighbours(tc, visited))
            {
                MarkVisited(visited, neighbour);
                Coord move = neighbour - current;
                int cheatingSteps = StepsWhenCheating(tc, numSpan, current, move);

                /*1.Negative steps less is, surprisingly, not good
                  2. The reduced total number of steps is the same or more than the regular optimal path
                  (cheatingSteps includes the 2 steps to get to it from the current location)*/
                if (cheatingSteps is -1 || steps + cheatingSteps >= regularSteps)
                    continue;

                int timeImproved = regularSteps - cheatingSteps - steps;

                cheats[timeImproved] = cheats.TryGetValue(timeImproved, out int frequency) ? frequency + 1 : 1;
            }
        }

        return cheats;
    }

    private static int StepsWhenCheating(TableContext tc, Span2D<int> numSpan, Coord current, Coord move)
    {
        Coord zero = Coord.Zero;
        Coord cheatCell = move switch
        {
            (-1, 0) => current.Up.Up,
            (0, 1) => current.Right.Right,
            (1, 0) => current.Down.Down,
            (0, -1) => current.Left.Left,
            _ => throw new ArgumentException($"Invalid move Coord provided in FindCheat call: {move}. Should be an immmediate neighbour.")
        };

        if (!WithinBounds(tc, cheatCell))
            return -1;

        int stepsFromCheatCell = numSpan.GetValueAt(cheatCell);
        if (stepsFromCheatCell is -1) //Walls have value of -1
            return stepsFromCheatCell;

        int stepsFromCurrent = numSpan.GetValueAt(current);

        //+2 for the 2 steps from the current position to the cheat cell
        return stepsFromCheatCell + 2 < stepsFromCurrent ? stepsFromCheatCell + 2 : -1;
    }

    private static int RateCells(TableContext tc, Span2D<int> numSpan, out Coord start, out Coord exit)
    {
        Span2D<char> gridSpan = tc.Table;
        (start, exit) = FindMarkers(gridSpan);
        gridSpan.SetCharAt('.', start);
        gridSpan.SetCharAt('.', exit);
        bool[,] visited = tc.Table.Generate2DBool();
        Queue<(Coord, int)> paths = [];

        MarkVisited(visited, exit);
        paths.Enqueue((exit, 0));
        while (paths.Count is not 0)
        {
            var (current, steps) = paths.Dequeue();

            foreach (var neighbour in current.GetDotNeighbours(tc, visited))
            {
                MarkVisited(visited, neighbour);
                numSpan.SetIntAt(steps + 1, neighbour);
                paths.Enqueue((neighbour, steps + 1));
            }
        }

        return numSpan.GetValueAt(start);
    }

    public static (Coord start, Coord exit) FindMarkers(Span2D<char> grid)
    {
        var (start, exit) = (Coord.Invalid, Coord.Invalid);
        Span<char> markers = ['S', 'E'];

        int row = 0;
        int index = 0;
        while (start == Coord.Invalid || exit == Coord.Invalid)
        {
            var rowSpan = grid.GetRowSpan(row);
            if (rowSpan.ContainsAny(markers))
            {
                foreach (var letter in grid.GetRow(row))
                {
                    if (letter is 'S')
                    {
                        start = new(row, index);
                    }

                    if (letter is 'E')
                    {
                        exit = new(row, index);
                    }

                    index++;
                }

                index = 0;
            }

            row++;
        }

        return (start, exit);
    }

    private static void MarkVisited(bool[,] visited, Coord coord) => visited[coord.Row, coord.Col] = true;

    private static bool WithinBounds(TableContext tc, Coord coord) => coord.Row >= 0 && coord.Row < tc.MaxRow
                                                                   && coord.Col >= 0 && coord.Col < tc.MaxCol;
}
