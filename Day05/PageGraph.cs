namespace Day05;

public class PageGraph
{
    private readonly Dictionary<ushort, HashSet<ushort>> relationships = [];

    public void AddRelationship(ushort pageA, ushort pageB)
    {
        relationships[pageA] = relationships.TryGetValue(pageA, out var storedSet) ? [.. storedSet, pageB] : [pageB];
        relationships[pageB] = relationships.TryGetValue(pageB, out storedSet) ? [.. storedSet, pageA] : [pageA];
    }

    public bool ValidateManual(ReadOnlySpan<ushort> manual)
    {
        for (int i = 0; i < manual.Length; i++)
        {
            if (!relationships.TryGetValue(manual[i], out var connections))
            {
                continue;
            }

            ReadOnlySpan<ushort> before = manual[..i];
            if (before.Overlaps([.. connections]))
            {
                return false;
            }

            ReadOnlySpan<ushort> after = manual[(i + 1)..];
            if (after.Overlaps([.. connections]))
            {
                return false;
            }
        }

        return true;
    }

    public void BuildGraph(IEnumerable<string> rules)
    {
        foreach (var rule in rules)
        {
            var (before, after) = Array.ConvertAll(rule.Split('|'), ushort.Parse) switch
            {
                var x => (x[0], x[1])
            };

            AddRelationship(before, after);
        }
    }
}
