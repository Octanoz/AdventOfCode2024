namespace Day05;

public class OrderValidator(string[] input)
{
    private static readonly List<int> validMiddlePages = [];

    private static Page root = new(71);

    public int PartOne()
    {
        int splitIndex = Array.FindIndex(input, s => s == "");
        var rules = input[..splitIndex].Select(s => Array.ConvertAll(s.Split('|'), int.Parse));
        string[] manuals = input[(splitIndex + 1)..];

        SetRoot(rules, manuals);
        SeedRootBranches(rules);
        PopulatePages(rules);
        ValidateManuals(manuals);

        return validMiddlePages.Sum();
    }

    private void SeedRootBranches(IEnumerable<int[]> rules)
    {
        foreach (var rule in rules.Where(arr => arr[0] == root.Id || arr[1] == root.Id))
        {
            root.ProcessSeedRule(rule);
        }
    }

    private static void SetRoot(IEnumerable<int[]> rules, string[] manuals)
    {
        Dictionary<int, int> beforeCounts = rules.SelectMany(x => new[] { (x[0], 1), (x[1], -1) })
                                                 .GroupBy(x => x.Item1)
                                                 .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2));

        if (beforeCounts.Values.Any(s => s > 0))
        {
            root = new(beforeCounts.MaxBy(kvp => kvp.Value).Key);
            return;
        }

        foreach (var num in manuals.SelectMany(s => Array.ConvertAll(s.Split(','), int.Parse)))
        {
            beforeCounts[num]++;
        }

        root = new(beforeCounts.MaxBy(kvp => kvp.Value).Key);

        if (root.Id is not 71)
        {
            Console.WriteLine($"root ID is {root.Id} not the hard-coded 71");
        }
    }

    private static void ValidateManuals(string[] manuals)
    {
        foreach (var manual in manuals)
        {
            int[] manualPages = Array.ConvertAll(manual.Split(','), int.Parse);
            if (!CompliantAfter(manualPages) || !CompliantBefore(manualPages))
                continue;

            int middle = manualPages.Length / 2; //All manuals have an odd amount of pages
            validMiddlePages.Add(manualPages[middle]);
        }
    }

    private static bool CompliantBefore(int[] manualPages)
    {
        for (int i = 1; i < manualPages.Length; i++)
        {
            ReadOnlySpan<int> beforeSpan = manualPages.AsSpan()[..i];
            ReadOnlySpan<int> storedIds = current.After.Select(p => p.Id).ToArray();
            if (beforeSpan.ContainsAny(storedIds))
            {
                return false;
            }
        }

        return true;
    }

    private static bool CompliantAfter(int[] manualPages)
    {
        for (int i = 0; i < manualPages.Length - 1; i++)
        {
            if (!pageMap.TryGetValue(manualPages[i], out Page? current))
            {
                throw new ArgumentException($"Couldn't find a page with id {manualPages[i]}. Check PopulatePages logic.");
            }

            ReadOnlySpan<int> afterSpan = manualPages.AsSpan()[(i + 1)..];
            ReadOnlySpan<int> storedIds = current.Before.Select(p => p.Id).ToArray();
            if (afterSpan.ContainsAny(storedIds))
            {
                return false;
            }
        }

        return true;
    }

    private static void PopulatePages(IEnumerable<int[]> rulesSequence)
    {
        HashSet<int> storedSet = new(root.GetStoredPages());
        foreach (var rule in rulesSequence.Where(r => storedSet.Contains(r[0]) || storedSet.Contains(r[1])))
        {
            root.ProcessRules(rule);
        }

        CorrectOrderStoredPages(storedSet, rulesSequence);
    }

    private static void CorrectOrderStoredPages(HashSet<int> storedSet, IEnumerable<int[]> rules)
    {
        foreach (var rule in rules.Where(r => storedSet.Contains(r[0]) && storedSet.Contains(r[1])))
        {
            root.CorrectSorting(rule);
        }
    }
}
