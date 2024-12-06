namespace Day05;

public class PageGraph
{
    private readonly Dictionary<ushort, HashSet<ushort>> relationships = [];

    public void AddRelationship(ushort pageA, ushort pageB)
    {
        if (!relationships.TryGetValue(pageA, out var storedSet))
        {
            storedSet = [];
            relationships[pageA] = storedSet;
        }
        storedSet.Add(pageB);

        if (!relationships.TryGetValue(pageB, out storedSet))
        {
            storedSet = [];
            relationships[pageB] = storedSet;
        }
        storedSet.Add(pageA);
    }

    public bool ValidateManual(ReadOnlySpan<ushort> manual)
    {
        Console.WriteLine(manual.ToString());

        for (int i = 0; i < manual.Length; i++)
        {
            if (!relationships.TryGetValue(manual[i], out var connections))
            {
                Console.WriteLine($"No connections, skip {manual[i]}");

                continue;
            }

            ReadOnlySpan<ushort> before = manual[..i];
            if (before.Overlaps([.. connections]))
            {
                Console.WriteLine($"Conflict found overlap before {manual[i]}");

                return false;
            }

            ReadOnlySpan<ushort> after = manual[(i + 1)..];
            if (after.Overlaps([.. connections]))
            {
                Console.WriteLine($"Conflict found, overlap after {manual[i]}");

                return false;
            }
        }

        return true;
    }

    public void BuildGraph(IEnumerable<ushort[]> rules)
    {
        foreach (var rule in rules)
        {

            AddRelationship(rule[0], rule[1]);
        }
    }
}
