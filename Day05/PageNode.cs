namespace Day05;

public class PageNode(int id)
{
    public int PageId { get; } = id;
    public PageNode? Before { get; set; }
    public PageNode? After { get; set; }

    // Insert a new node in the correct position based on relationships
    public void Insert(PageNode newNode, bool isBefore)
    {
        if (isBefore)
        {
            newNode.After = this;
            newNode.Before = Before;
            if (Before is PageNode pn)
                pn.After = newNode;
            Before = newNode;
        }
        else
        {
            newNode.Before = this;
            newNode.After = After;
            if (After is PageNode pn)
                pn.Before = newNode;
            After = newNode;
        }
    }
}

public class PageTree
{
    private readonly Dictionary<int, PageNode> nodes = [];

    // Add a new relationship between two pages
    public void AddRelationship(int pageA, int pageB)
    {
        nodes[pageA] = nodes.TryGetValue(pageA, out PageNode? storedNode) ? storedNode : new(pageA);
        nodes[pageB] = nodes.TryGetValue(pageB, out storedNode) ? storedNode : new(pageB);

        PageNode nodeA = nodes[pageA];
        PageNode nodeB = nodes[pageB];

        nodeA.Insert(nodeB, isBefore: false); // Change to true for "before" insertion
    }

    // Build the tree from a set of rules
    public void BuildGraph(IEnumerable<int[]> rules)
    {
        foreach (int[] rule in rules)
        {
            AddRelationship(rule[0], rule[1]);
        }
    }

    public void PrintTree(int startPage)
    {
        if (!nodes.TryGetValue(startPage, out var node))
            return;

        HashSet<int> visited = [];
        while (visited.Add(node.PageId))
        {
            Console.WriteLine(node.PageId);
            if (node.After is PageNode afterNode)
                node = afterNode;
        }
    }
}

public enum Collect
{
    Before,
    After
}
