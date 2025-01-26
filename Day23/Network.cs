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

    public void PrintNodes()
    {
        foreach (var node in nodes.Values.OrderBy(n => n.Name))
        {
            Console.WriteLine($"{node.Name.ToUpper()}: [{String.Join(", ", node.Connections.Select(n => n.Name))}]");
        }
    }

    public string FindLargestCluster()
    {
        HashSet<Node> maxCluster = [];
        BronKerboschRecursive([], [.. nodes.Values], [], ref maxCluster);

        var sortedNames = maxCluster.Select(n => n.Name).Order();
        return String.Join(",", sortedNames);
    }

    private void BronKerboschRecursive(HashSet<Node> result, List<Node> potential, HashSet<Node> rejected, ref HashSet<Node> maxCluster)
    {
        if (potential.Count is 0 && rejected.Count is 0)
        {
            if (result.Count > maxCluster.Count)
            {
                maxCluster = new(result);
            }

            return;
        }

        Node? pivot = potential.Concat(rejected).OrderByDescending(n => n.Connections.Count).FirstOrDefault();
        HashSet<Node> pivotNeighbours = pivot is null ? [] : [.. pivot.Connections];

        foreach (var node in new HashSet<Node>(potential.Except(pivotNeighbours)))
        {
            HashSet<Node> newResult = [.. result, node];
            List<Node> newPotential = [.. potential.Intersect(node.Connections)];
            HashSet<Node> newRejected = [.. rejected.Intersect(node.Connections)];

            BronKerboschRecursive(newResult, newPotential, newRejected, ref maxCluster);

            potential.Remove(node);
            rejected.Add(node);
        }
    }
}
