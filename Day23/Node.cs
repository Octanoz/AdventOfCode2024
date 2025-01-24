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
