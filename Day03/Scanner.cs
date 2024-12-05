namespace Day3;

using System.Text.RegularExpressions;

public static class Scanner
{
    public static int PartOne(ReadOnlySpan<string> input)
    {
        int total = 0;
        foreach (var line in input)
        {
            foreach (Match match in Helpers.Multiplications().Matches(line))
            {
                int num1 = int.Parse(match.Groups["left"].Value);
                int num2 = int.Parse(match.Groups["right"].Value);

                total += num1 * num2;
            }
        }

        return total;
    }

    public static int PartTwo(string input)
    {
        int total = 0;
        Processing proc = Processing.Active;
        foreach (Match match in Helpers.Operations().Matches(input))
        {
            if (match.Groups["toggle"].Success)
            {
                proc = match.Groups["toggle"].Value switch
                {
                    "do()" when proc is Processing.Suspended => ++proc,
                    "don't()" when proc is Processing.Active => --proc,
                    _ => proc
                };

                continue;
            }

            if (proc is Processing.Suspended)
                continue;

            int num1 = int.Parse(match.Groups["left"].Value);
            int num2 = int.Parse(match.Groups["right"].Value);

            total += num1 * num2;
        }

        return total;
    }
}

static partial class Helpers
{
    [GeneratedRegex(@"mul\((?<left>\d{1,3}),(?<right>\d{1,3})\)")]
    public static partial Regex Multiplications();

    [GeneratedRegex(@"mul\((?<left>\d{1,3}),(?<right>\d{1,3})\)|(?<toggle>(do(n't)?)\(\))")]
    public static partial Regex Operations();
}

public enum Processing
{
    Suspended,
    Active
}