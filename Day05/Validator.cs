namespace Day05;

public class Validator
{
    private static OrderedDictionary<int, Page> pageMap = new(49);
    private static List<Page> validMiddlePages = [];
    private static int root = 71;
    public int PartOne(string[] input)
    {
        int splitIndex = Array.FindIndex(input, s => s == "");
        var rules = input[..splitIndex].Select(s => Array.ConvertAll(s.Split('|'), int.Parse)); ;
        string[] manuals = input[(splitIndex + 1)..];

        SetRoot(rules, manuals);
        SeedDictionary(rules);
        PopulatePages(rules);
        ValidateManuals(manuals);
        return validMiddlePages.Sum(p => p.Id);
    }

    private static void SetRoot(IEnumerable<int[]> rules, string[] manuals)
    {
        Dictionary<int, int> beforeCounts = rules.SelectMany(x => new[] { (x[0], 1), (x[1], -1) })
                                                 .GroupBy(x => x.Item1)
                                                 .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2));

        if (beforeCounts.Values.Any(s => s > 0))
        {
            root = beforeCounts.MaxBy(kvp => kvp.Value).Key;
        }
        else
        {
            foreach (var num in manuals.SelectMany(s => Array.ConvertAll(s.Split(','), int.Parse)))
            {
                beforeCounts[num]++;
            }

            root = beforeCounts.MaxBy(kvp => kvp.Value).Key;
        }

        if (root is not 71)
        {
            Console.WriteLine($"root is {root} not the hard-coded 71");
        }
    }

    private static void SeedDictionary(IEnumerable<int[]> rules)
    {
        pageMap.SetAt(24, root, new(root));

        foreach (var rule in rules.Where(arr => arr[0] == root || arr[1] == root))
        {
            switch (Array.FindIndex(rule, r => r == root))
            {
                case 0:
                    pageMap.SetAt(25, rule[1], new(rule[1]));
                    break;
                case 1:
                    pageMap.SetAt(23, rule[0], new(rule[0]));
                    break;
            }
        }
    }

    private static void PopulatePages(IEnumerable<int[]> rules)
    {
        int rootIndex = pageMap.IndexOf(root);
        CorrectSorting(rules);
        while (pageMap.Count < pageMap.Capacity)
        {
            Span<int[]> filteredRules = FilterRules(rules);
            foreach (var rule in filteredRules)
            {
                ApplyRule(rule);
            }
            int storedBefore = current.GetTotalStored();
            foreach (var rule in rules.Where(r => r.Contains(current.Id)))
            {
                Page other = current.Id == rule[0] ? pageMap[rule[1]] : pageMap[rule[0]];
                if (current.Id == rule[0])
                {
                    current.AddAfter(other, []);
                }
                else current.AddBefore(other, []);
            }
            if (storedBefore < current.GetTotalStored())
            {
                if (++queueCounter % 1000 == 0)
                {
                    Console.WriteLine($"Queue'ed {queueCounter} pages.\nCurrent page had {storedBefore} pages store, this grew to {current.GetTotalStored()}");
                }
                pageQueue.Enqueue(current);
            }
        }
    }

    private static void ApplyRule(int[] rule)
    {
        throw new NotImplementedException();
    }

    private static int[][] FilterRules(IEnumerable<int[]> rules)
    {
        return rules.Where(r => pageMap.ContainsKey(r[0]) || pageMap.ContainsKey(r[1])).ToArray();
    }

    private static void CorrectSorting(IEnumerable<int[]> rules)
    {
        foreach (var rule in rules.Where(r => pageMap.ContainsKey(r[0]) && pageMap.ContainsKey(r[1])))
        {
            int beforeIndex = pageMap.IndexOf(rule[0]);
            int afterIndex = pageMap.IndexOf(rule[1]);

            if (beforeIndex > afterIndex)
            {
                pageMap.RemoveAt(afterIndex);
                pageMap.SetAt(beforeIndex, rule[1], new(rule[1]));
            }
        }
    }

    private static void ValidateManuals(string[] manuals)
    {
        foreach (var manual in manuals)
        {
            int[] manualPages = Array.ConvertAll(manual.Split(','), int.Parse);
            if (!CompliantAfter(manualPages) || !CompliantBefore(manualPages))
                continue;
            int middleIndex = manualPages.Length / 2;
            if (!pageMap.TryGetValue(manualPages[middleIndex], out Page? middle))
            {
                throw new ArgumentException($"Couldn't find a middle page with id {manualPages[middleIndex]}. Check PopulatePages logic.");
            }
            validMiddlePages.Add(middle);
        }
    }
    private static bool CompliantBefore(int[] manualPages)
    {
        for (int i = 1; i < manualPages.Length; i++)
        {
            if (!pageMap.TryGetValue(manualPages[i], out Page? current))
            {
                throw new ArgumentException($"Couldn't find a page with id {manualPages[i]}. Check PopulatePages logic.");
            }
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
}

