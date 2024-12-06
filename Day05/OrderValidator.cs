using System.Data;

namespace Day05;

public class OrderValidator
{
    private static readonly List<int> middleIds = [];

    public int PartOne(string[] input)
    {
        int splitIndex = Array.FindIndex(input, s => s == "");
        var rules = input[..splitIndex].Select(s => Array.ConvertAll(s.Split('|'), ushort.Parse));
        ReadOnlySpan<string> manuals = input.AsSpan()[(splitIndex + 1)..];

        PageGraph graph = new();
        graph.BuildGraph(rules);

        foreach (var manual in manuals)
        {
            var manualPages = Array.ConvertAll(manual.Split(','), ushort.Parse).AsSpan();
            if (graph.ValidateManual(manualPages))
            {
                int middleIndex = manualPages.Length / 2;
                middleIds.Add(manualPages[middleIndex]);
            }
        }

        return middleIds.Sum();
    }
}
