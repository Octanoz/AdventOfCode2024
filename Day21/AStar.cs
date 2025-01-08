namespace Day21;

using AdventUtilities;

public static class AStar
{
    public static List<Coord> FindPath(Coord start, Coord goal, int maxRow, int maxCol)
    {
        AStarNodeComparer nodeComparer = new();
        SortedSet<AStarNode> openList = new(nodeComparer);
        HashSet<Coord> closedList = [];
        Dictionary<Coord, int> gCost = [];
        Dictionary<Coord, Coord> parentMap = [];

        openList.Add(new(start, 0, start.Manhattan(goal)));

        gCost[start] = 0;
        parentMap[start] = start;

        while (openList.Count is not 0)
        {
            AStarNode currentNode = openList.Min()!;
            openList.Remove(currentNode);

            if (currentNode.Coord.Equals(goal))
            {
                return ReconstructPath(parentMap, start, goal);
            }

            closedList.Add(currentNode.Coord);

            foreach (var neighbour in currentNode.Coord.GetValidNeighbours(maxRow, maxCol).Where(nb => !closedList.Contains(nb)))
            {
                int tentativeGCost = gCost[currentNode.Coord] + GetMoveCost(currentNode.Coord, neighbour);

                if (!gCost.TryGetValue(neighbour, out var storedCost) || tentativeGCost < storedCost)
                {
                    gCost[neighbour] = tentativeGCost;
                    int fCost = tentativeGCost + neighbour.Manhattan(goal);
                    openList.Add(new(neighbour, tentativeGCost, fCost));
                    parentMap[neighbour] = currentNode.Coord;
                }
            }
        }

        return [];
    }

    private static List<Coord> ReconstructPath(Dictionary<Coord, Coord> parentMap, Coord start, Coord goal)
    {
        List<Coord> path = [];
        Coord current = goal;

        while (!current.Equals(start))
        {
            path.Add(current);
            current = parentMap[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }

    private static int GetMoveCost(Coord from, Coord to)
    {
        switch ((from, to))
        {
            case (_, (3, 0)):
                return 1000;

            case (_, (3, 2)):
                return 1;

            case var (hc, lc) when hc.Col > lc.Col:
                return 5;

            case var (lr, hr) when lr.Row < hr.Row:
                return 3;

            default:
                return 1;
        }
    }
}

public class AStarNode(Coord coord, int gCost, int fCost)
{
    public Coord Coord { get; } = coord;
    public int GCost { get; } = gCost;
    public int FCost { get; } = fCost;
}

public class AStarNodeComparer : IComparer<AStarNode>
{
    public int Compare(AStarNode? x, AStarNode? y) => x!.FCost.CompareTo(y!.FCost);
}
