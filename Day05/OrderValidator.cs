using System.Data;

namespace Day05;

public class OrderValidator
{
    private static Dictionary<int, Page> pageMap = [];
    private static List<Page> validMiddlePages = [];

    public int PartOne(string[] input)
    {
        int splitIndex = Array.FindIndex(input, s => s == "");
        string[] rules = input[..splitIndex];
        string[] manuals = input[(splitIndex + 1)..];

        PopulatePages(rules);

        ValidateManuals(manuals);

        return validMiddlePages.Sum(p => p.Id);
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

    private static void PopulatePages(string[] rulesStrings)
    {
        int[][] rules = rulesStrings.Select(s => Array.ConvertAll(s.Split('|'), int.Parse)).ToArray();

        Console.WriteLine($"{rules.Length} rules for {pageMap.Count} stored pages");


        Dictionary<int, int> ruleCount = rules.SelectMany(arr => arr)
                                              .GroupBy(x => x)
                                              .ToDictionary(g => g.Key, g => g.Count());

        Queue<Page> pageQueue = CycleRules(rules, ruleCount);
        Console.WriteLine($"{pageMap.Count} pages currently stored");
        int queueCounter = 1;

        while (pageQueue.Count is not 0)
        {
            var current = pageQueue.Dequeue();
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

    private static Queue<Page> CycleRules(int[][] rules, Dictionary<int, int> ruleCount)
    {
        Queue<Page> queue = [];
        foreach (var rule in rules)
        {
            if (!pageMap.TryGetValue(rule[0], out var first))
            {
                pageMap[rule[0]] = new(rule[0]);
                first = pageMap[rule[0]];
            }

            if (!pageMap.TryGetValue(rule[1], out var second))
            {
                pageMap[rule[1]] = new(rule[1]);
                second = pageMap[rule[1]];
            }

            Connect(first, second);

            if (--ruleCount[first.Id] is 0)
            {
                queue.Enqueue(first);
            }

            if (--ruleCount[second.Id] is 0)
            {
                queue.Enqueue(second);
            }
        }

        return queue;
    }

    private static void Connect(Page a, Page b)
    {
        a.AddAfter(b, []);
        b.AddBefore(a, []);
    }
}
