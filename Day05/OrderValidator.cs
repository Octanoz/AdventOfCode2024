using System.Data;

namespace Day05;

public class OrderValidator
{
    private static readonly List<int> middleIds = [];

    public int PartOne(string[] input)
    {
        int splitIndex = Array.FindIndex(input, s => s == "");
        var rules = input[..splitIndex].Select(s => Array.ConvertAll(s.Split('|'), int.Parse));
        ReadOnlySpan<string> manuals = input.AsSpan()[(splitIndex + 1)..];


        PageGraph graph = new();
        graph.BuildGraph(rules);

        int[] roots = [79, 96];
        Dictionary<int, PageTree> trees = [];

        foreach (var root in roots)
        {
            trees[root] = graph.BuildTree(root);
        }

        foreach (var manual in manuals)
        {
            var manualPages = Array.ConvertAll(manual.Split(','), int.Parse).AsSpan();
            if (ValidateWithTrees(manualPages, trees))
            {
                int middleIndex = manualPages.Length / 2;
                middleIds.Add(manualPages[middleIndex]);
            }
        }

        return middleIds.Sum();
    }

    private static bool ValidateWithTrees(ReadOnlySpan<int> manual, Dictionary<int, Dictionary<int, List<int>>> trees)
    {
        for (int i = 0; i < manual.Length; i++)
        {
            var current = manual[i];
            bool isValid = false;

            foreach (var tree in trees.Values.Where(d => d.ContainsKey(current)))
            {
                var beforePages = manual[..i];
                var afterPages = manual[(i + 1)..];

                if (!beforePages.ContainsAny([.. tree[current]])
                && !afterPages.ContainsAny([.. tree[current]]))
                {
                    isValid = true;
                    break;
                }
            }

            if (!isValid)
            {
                return false;
            }
        }

        return true;
    }
}
