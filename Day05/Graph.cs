namespace Day05;

public class Graph
{
    private Dictionary<int, List<int>> adjList = [];
    public void AddEdge(int u, int v)
    {
        adjList[u] = adjList.TryGetValue(u, out var cachedList) ? [.. cachedList, v] : [v];
    }

    public List<int> GetEdges(int u)
    {
        return adjList.TryGetValue(u, out var storedList) ? storedList : [];
    }
}
