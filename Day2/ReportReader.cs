namespace Day2;

public static class ReportReader
{
    public static int PartOne(string[] input) => input.Select(s => Array.ConvertAll(s.Split(), int.Parse))
                                                      .Count(arr => AllSameDirection(arr));

    public static int PartTwo(string[] input) => input.Select(s => Array.ConvertAll(s.Split(), int.Parse))
                                                      .Count(arr => PassWithProblemDampener(arr));

    private static bool PassWithProblemDampener(int[] report)
    {
        if (report.GroupBy(num => num).Max(group => group.Count()) > 2)
            return false;

        if (AllSameDirection(report))
            return true;

        int length = report.Length;
        for (int i = 0; i < length; i++)
        {
            Span<int> reportSpan = i switch
            {
                0 => report[1..],
                var index when index == length - 1 => report[..^1],
                _ => [.. report[..i], .. report[(i + 1)..]]
            };

            if (AllSameDirection(reportSpan))
            {
                return true;
            }
        }

        return false;
    }

    private static bool AllSameDirection(ReadOnlySpan<int> report)
    {
        Difference diff = (report[1] - report[0]) switch
        {
            0 => Difference.None,
            > 0 => Difference.Increasing,
            _ => Difference.Decreasing
        };

        if (diff is Difference.None)
            return false;

        for (int i = 1; i < report.Length; i++)
        {
            if (diff is Difference.Increasing && report[i] <= report[i - 1]
            || (diff is Difference.Decreasing && report[i] >= report[i - 1])
            || !WithinLimits(report[i - 1], report[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool WithinLimits(int a, int b) => Math.Abs(a - b) is > 0 and <= 3;
}

public enum Difference
{
    None,
    Increasing,
    Decreasing
}
