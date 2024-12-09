namespace Day05;

public static class Validator
{
    private static OrderedDictionary<int, Page> pageMap = new(49);
    private static List<Page> validMiddlePages = [];
    private static int root = 71;
    public static int PartOne(string[] input)
    {
        int splitIndex = Array.FindIndex(input, s => s == "");
        var rules = input[..splitIndex].Select(s => Array.ConvertAll(s.Split('|'), int.Parse)); ;
        string[] manuals = input[(splitIndex + 1)..];

        SetRoot(rules, manuals);
        SeedDictionary(rules);
        PopulatePages(rules);
        ValidateManuals(manuals);
        return checked(validMiddlePages.Sum(p => p.Id));
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
        pageMap.Add(root, new(root));
        foreach (var rule in rules.Where(arr => arr[0] == root || arr[1] == root))
        {
            switch (Array.IndexOf(rule, root))
            {
                case 0:
                    pageMap.Add(rule[1], new(rule[1]));
                    break;
                case 1:
                    pageMap.Insert(0, rule[0], new(rule[0]));
                    break;
            }
        }

        CorrectSorting(rules);
    }

    private static void PopulatePages(IEnumerable<int[]> rules)
    {
        int pageCount = CountPages(rules);
        while (pageMap.Count < pageCount)
        {
            int[][] filteredRules = FilterRules(rules);
            foreach (var rule in filteredRules)
            {
                ApplyRule(rule);
            }

            CorrectSorting(rules);
        }
    }

    private static int CountPages(IEnumerable<int[]> rules) => rules.SelectMany(arr => arr)
                                                                    .GroupBy(x => x)
                                                                    .ToLookup(g => g.Key, g => g.Count()).Count;

    private static void ApplyRule(int[] rule)
    {
        switch (pageMap.IndexOf(rule[0]), pageMap.IndexOf(rule[1]))
        {
            case (-1, var num):
                pageMap.Insert(num is 0 ? num : num - 1, rule[0], new(rule[0]));
                break;
            case (var num, -1):
                if (num == pageMap.Count - 1)
                {
                    pageMap.Add(rule[1], new(rule[1]));
                }
                else pageMap.Insert(num, rule[1], new(rule[1]));
                break;
            default: break;
        }
    }

    private static int[][] FilterRules(IEnumerable<int[]> rules)
    {
        return rules.Where(r => pageMap.ContainsKey(r[0]) && !pageMap.ContainsKey(r[1])
                             || (pageMap.ContainsKey(r[1]) && !pageMap.ContainsKey(r[0])))
                    .ToArray();
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

                if (beforeIndex == pageMap.Count)
                {
                    pageMap.Add(rule[1], new(rule[1]));
                }
                else pageMap.Insert(beforeIndex, rule[1], new(rule[1]));
            }
        }
    }

    private static void ValidateManuals(string[] manuals)
    {
        foreach (var manual in manuals.Select(s => Array.ConvertAll(s.Split(','), int.Parse)))
        {
            bool isValid = true;
            int currentIndex = -1;
            for (int i = 0; i < manual.Length; i++)
            {
                int index = pageMap.IndexOf(manual[i]);
                if (index > currentIndex)
                {
                    currentIndex = index;
                }
                else
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                int middle = manual.Length / 2;
                validMiddlePages.Add(new(manual[middle]));
            }
        }
    }
}

