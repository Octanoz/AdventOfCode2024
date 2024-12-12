using System.Text.RegularExpressions;

namespace Day1;

public static class Solver
{
    public static int PartOne(ReadOnlySpan<string> input)
    {
        Span<int> list1 = stackalloc int[input.Length];
        Span<int> list2 = stackalloc int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            (list1[i], list2[i]) = Array.ConvertAll(input[i].Split("   "), int.Parse) switch
            {
                var x => (x[0], x[1])
            };
        }

        list1.Sort();
        list2.Sort();

        int total = 0;
        for (int i = 0; i < list1.Length; i++)
        {
            (int left, int right) = (list1[i], list2[i]);
            total += Math.Abs(left - right);
        }

        return total;
    }

    public static int OneAlt(string[] input)
    {
        var (left, right) = input.Select(s => Array.ConvertAll(s.Split("   "), int.Parse))
        .Aggregate((l: new List<int>(), r: new List<int>()), (acc, x) =>
        {
            acc.l.Add(x[0]);
            acc.r.Add(x[1]);
            return acc;
        });

        return left.Order().Zip(right.Order(), (l, r) => Math.Abs(l - r)).Sum();
    }

    public static int PartTwo(ReadOnlySpan<string> input)
    {
        Span<int> list1 = stackalloc int[input.Length];
        Span<int> list2 = stackalloc int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            (list1[i], list2[i]) = Array.ConvertAll(input[i].Split("   "), int.Parse) switch
            {
                var x => (x[0], x[1])
            };
        }

        list1.Sort();
        list2.Sort();

        int total = 0;
        foreach (var number in list1)
        {
            total += number * list2.Count(number);
        }

        return total;

    }

    public static int Alternative(string filePath)
    {
        int[] input = Array.ConvertAll(Regex.Split(File.ReadAllText(filePath), @"(\s+|\r\n)")
                                            .Where(s => !String.IsNullOrWhiteSpace(s))
                                            .ToArray(), int.Parse);

        var list1 = input.Where((s, i) => i % 2 == 0);
        var list2 = input.Where((s, i) => i % 2 != 0);

        return list2.CountBy(x => x)
                    .Where(g => list1.Contains(g.Key))
                    .Sum(g => list1.Count(n => n == g.Key) * g.Key * g.Value);
    }
}
