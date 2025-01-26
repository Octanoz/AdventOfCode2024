namespace Day23;

public class Node(string name)
{
    public string Name { get; } = name;
    public HashSet<Node> Connections { get; } = [];

    public void Connect(Node other)
    {
        Connections.Add(other);
        other.Connections.Add(this);
    }
}

public static class NodeExtensions
{
    public static bool ConnectsToTarget(this Node node, Node target)
        => node.Connections.Any(n => n.Connections.Contains(target));

    public static IEnumerable<(Node, Node, Node)> GetThreeClusters(this Node origin, Node checkedNode)
    {
        foreach (var thirdNode in checkedNode.Connections.Where(n => n.Connections.Contains(origin)))
        {
            yield return (origin, checkedNode, thirdNode);
        }
    }
}
