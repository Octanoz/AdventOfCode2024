namespace Day05;

public class OrderValidator(string[] input)
{
    private static readonly List<int> validMiddlePages = [];

    private static PageFull root = new(71);

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

    private static void SeedRootBranches(IEnumerable<int[]> rules)
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
        throw new NotImplementedException();
    }

    private static bool CompliantAfter(int[] manualPages)
    {
        throw new NotImplementedException();
    }

    private static void PopulatePages(IEnumerable<int[]> rulesSequence)
    {
        HashSet<int> storedSet = [.. root.GetStoredPages()];
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
