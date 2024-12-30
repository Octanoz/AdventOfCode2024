using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day16;

public static class MazeRunner
{
    public static int PartOne(string[] input)
    {
        char[,] mainMap = GridExtensions.New2DGrid<char>(input);
        Span2D<char> map = mainMap;
        Coord start = new(map.Height - 2, 1);

        List<Reindeer> finishedDeers = [];
        Dictionary<Coord, int> visitedScores = [];
        // HashSet<(Coord, int)> visited = [];
        Queue<Reindeer> queue = [];
        Reindeer rudolph = new(start, Direction.Right, []);
        queue.Enqueue(rudolph);

        while (queue.Count is not 0)
        {
            Reindeer current = queue.Dequeue();

            if (map.GetValueAt(current.Pos) is 'E')
            {
                finishedDeers.Add(current);
                continue;
            }

            if (current.Points > 5000)
            {
                if (ScoreTooHigh(current.Pos, current.Points, visitedScores))
                    continue;
            }

            for (int i = 0; i < 4; i++)
            {
                Coord neighbour = current.Pos.Neighbours.ElementAt(i);
                HashSet<Coord> newSet = new(current.Visited);

                if (map.GetValueAt(neighbour) is '.' or 'E' && newSet.Add(neighbour))
                {
                    Direction targetDirection = (Direction)i;
                    int points = PointsForTurning(current.Dir, targetDirection) + 1;
                    queue.Enqueue(new(new(neighbour.Row, neighbour.Col), targetDirection, newSet, current.Points + points));
                }
            }
        }

        Reindeer? winner = finishedDeers.MinBy(deer => deer.Points);
        if (winner is not null)
        {
            Coord lastPosition = start;
            foreach (var point in winner.Visited)
            {
                Coord[] neighbours = lastPosition.Neighbours.ToArray();

                char arrow = Array.IndexOf(neighbours, point) switch
                {
                    0 => '^',
                    1 => '>',
                    2 => 'V',
                    _ => '<'
                };

                map.SetCharAt(arrow, point);
                lastPosition = point;
            }
        }

        map.Draw2DGridTight();

        return finishedDeers.MinBy(deer => deer.Points).Points;
    }

    public static int PartTwo(string[] input)
    {
        char[,] mainMap = GridExtensions.New2DGrid<char>(input);
        Span2D<char> map = mainMap;
        Coord start = new(1, map.Width - 2);

        //Cache dimensions
        int logRow = map.Height / 2;
        int logCol = map.Width / 2;
        Coord logLimit = new(logRow, logCol);

        List<Reindeer> finishedDeers = [];
        Dictionary<Coord, int> visitedScores = [];
        HashSet<Coord> visited = [start];
        Queue<Reindeer> queue = [];
        Reindeer rudolph = new(start, Direction.Right, []);
        queue.Enqueue(rudolph);

        while (queue.Count is not 0)
        {
            Reindeer current = queue.Dequeue();

            if (map.GetValueAt(current.Pos) is 'S')
            {
                finishedDeers.Add(current);
                continue;
            }

            if (current.Points > 133584)
            {
                continue;
            }

            if (current.Points > 5000)
            {
                if (ScoreTooHigh(current.Pos, current.Points, visitedScores))
                    continue;
            }

            for (int i = 0; i < 4; i++)
            {
                Coord neighbour = current.Pos.Neighbours.ElementAt(i);
                HashSet<Coord> newSet = new(current.Visited);

                if (map.GetValueAt(neighbour) is '.' or 'S' && newSet.Add(neighbour))
                {
                    Direction targetDirection = (Direction)i;
                    int points = PointsForTurning(current.Dir, targetDirection) + 1;
                    queue.Enqueue(new(new(neighbour.Row, neighbour.Col), targetDirection, newSet, current.Points + points));
                }
            }
        }

        int winningPoints = finishedDeers.MinBy(deer => deer.Points)!.Points;
        foreach (var winner in finishedDeers.Where(deer => deer.Points == winningPoints))
        {
            visited.UnionWith(winner.Visited);
            Coord lastPosition = start;
            foreach (var point in winner.Visited)
            {
                Coord[] neighbours = lastPosition.Neighbours.ToArray();

                map.SetCharAt('O', point);
                lastPosition = point;
            }
        }

        map.Draw2DGridTight();

        return visited.Count;
    }

    private static bool ScoreTooHigh(Coord pos, int points, Dictionary<Coord, int> visitedScores)
    {
        if (visitedScores.TryGetValue(pos, out int storedScore))
        {
            if (storedScore < points)
                return true;
        }
        visitedScores[pos] = points;

        return false;
    }

    private static int PointsForTurning(Direction current, Direction target)
    {
        int turns = 0;
        var rightCurrent = current;

        while (current != target && rightCurrent != target)
        {
            turns++;
            current = (Direction)(((int)current + 3) & 3);
            rightCurrent = (Direction)(((int)rightCurrent + 1) & 3);
        }

        return turns * 1000;
    }

    private static bool InLogArea(Coord limit, Coord current) => current.Row < limit.Row && current.Col > limit.Col;

}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}