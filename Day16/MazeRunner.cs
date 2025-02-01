using AdventUtilities;

using CommunityToolkit.HighPerformance;

namespace Day16;

public static class MazeRunner
{
    public static int PartOne(string[] input)
    {
        char[,] mainMap = input.New2DGrid<char>();
        Span2D<char> map = mainMap;
        Coord start = new(map.Height - 2, 1);

        List<Reindeer> finishedDeer = [];
        Dictionary<Coord, int> visitedScores = [];
        Queue<Reindeer> queue = [];
        Reindeer rudolph = new(start, Direction.Right, []);
        queue.Enqueue(rudolph);

        while (queue.Count is not 0)
        {
            Reindeer current = queue.Dequeue();

            if (map.GetValueAt(current.Pos) is 'E')
            {
                finishedDeer.Add(current);
                continue;
            }

            if (current.Points > 5000)
            {
                if (ScoreTooHigh(current.Pos, current.Points, visitedScores))
                    continue;
            }

            for (int i = 0; i < 4; i++)
            {
                Coord neighbour = NeighbourByIndex(current.Pos, i);

                if (map.GetValueAt(neighbour) is '.' or 'E' && !current.Visited.Contains(neighbour))
                {
                    HashSet<Coord> newSet = new(current.Visited) { neighbour };
                    Direction targetDirection = (Direction)i;
                    int points = PointsForTurning(current.Dir, targetDirection) + 1;
                    queue.Enqueue(new(new(neighbour.Row, neighbour.Col), targetDirection, newSet, current.Points + points));
                }
            }
        }

        Reindeer? winner = finishedDeer.MinBy(deer => deer.Points);

        return winner is not null ? winner.Points : -1;
    }

    public static int PartTwo(string[] input)
    {
        char[,] mainMap = input.New2DGrid<char>();
        Span2D<char> map = mainMap;
        Coord start = new(1, map.Width - 2); //Start from E

        List<Reindeer> finishedDeer = [];
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
                finishedDeer.Add(current);
                visited.UnionWith(current.Visited);
                continue;
            }

            if (current.Points > 133584) //winning score for part 1
                continue;

            if (current.Points > 5000)
            {
                if (ScoreTooHigh(current.Pos, current.Points, visitedScores))
                    continue;
            }

            for (int i = 0; i < 4; i++)
            {
                Coord neighbour = NeighbourByIndex(current.Pos, i);

                if (map.GetValueAt(neighbour) is '.' or 'S' && !current.Visited.Contains(neighbour))
                {
                    HashSet<Coord> newSet = new(current.Visited) { neighbour };
                    Direction targetDirection = (Direction)i;
                    int points = PointsForTurning(current.Dir, targetDirection) + 1;
                    queue.Enqueue(new(new(neighbour.Row, neighbour.Col), targetDirection, newSet, current.Points + points));
                }
            }
        }

        foreach (var point in visited)
        {
            map.SetCharAt('O', point);
        }

        map.Draw2DGridTight();

        return visited.Count;
    }

    private static Coord NeighbourByIndex(Coord coord, int index) => index switch
    {
        0 => coord.Up,
        1 => coord.Right,
        2 => coord.Down,
        _ => coord.Left
    };

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
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}