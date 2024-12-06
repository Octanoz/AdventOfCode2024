namespace Day05;

public class PageGraph
{
    private readonly Dictionary<int, HashSet<int>> relationships = [];

    public void AddRelationship(int pageA, int pageB)
    {
        relationships[pageA] = relationships.TryGetValue(pageA, out var storedSet) ? [.. storedSet, pageB] : [pageB];
        relationships[pageB] = relationships.TryGetValue(pageB, out storedSet) ? [.. storedSet, pageA] : [pageA];
    }

    public bool ValidateManual(ReadOnlySpan<int> manual)
    {
        Console.WriteLine(manual.ToString());

        for (int i = 0; i < manual.Length; i++)
        {
            if (!relationships.TryGetValue(manual[i], out var connections))
            {
                Console.WriteLine($"No connections, skip {manual[i]}");

                continue;
            }

            ReadOnlySpan<int> before = manual[..i];
            if (before.Overlaps([.. connections]))
            {
                Console.WriteLine($"Conflict found overlap before {manual[i]}");

                return false;
            }

            ReadOnlySpan<int> after = manual[(i + 1)..];
            if (after.Overlaps([.. connections]))
            {
                Console.WriteLine($"Conflict found, overlap after {manual[i]}");

                return false;
            }
        }

        return true;
    }

    public void BuildGraph(IEnumerable<int[]> rules)
    {
        foreach (var rule in rules)
        {
            AddRelationship(rule[0], rule[1]);
        }
    }

    public PageTree BuildTree(int rootPage)
    {
        HashSet<int> visited = [rootPage];
        Dictionary<int, List<int>> tree = [];
        Queue<int> pageQueue = [];

        pageQueue.Enqueue(rootPage);
        while (pageQueue.Count is not 0)
        {
            var current = pageQueue.Dequeue();
            tree[current] = tree.TryGetValue(current, out var cached) ? cached : [];

            foreach (var neighbour in relationships[current])
            {
                if (visited.Add(neighbour))
                {
                    tree[current].Add(neighbour);
                    pageQueue.Enqueue(neighbour);
                }
            }
        }

        return tree;
    }
}
