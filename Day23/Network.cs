namespace Day23;

public class Network
{
    private readonly Dictionary<string, Node> nodes = [];

    public void AddNodes(string first, string second)
    {
        nodes.TryAdd(first, new(first));
        nodes.TryAdd(second, new(second));
        nodes[first].Connect(nodes[second]);
    }

    public HashSet<string> FindTriangles()
    {
        HashSet<string> triangles = [];

        foreach (var node in nodes.Values.Where(n => n.Name.StartsWith('t')))
        {
            foreach (var connected in node.Connections)
            {
                foreach (var (n1, n2, n3) in node.GetThreeClusters(connected))
                {
                    List<string> nodeNames = [n1.Name, n2.Name, n3.Name];
                    triangles.Add(String.Join("-", nodeNames.Order()));
                }
            }
        }

        return triangles;
    }
}
