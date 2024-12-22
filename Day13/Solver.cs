using System.Text.RegularExpressions;
using SuperLinq;

namespace Day13;

public static class Solver
{
    public static int PartOne(string filePath)
    {
        List<int> parsedNumbers = [];
        List<Machine> machines = [];
        int result = 0;

        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                foreach (Match match in Helpers.Numbers().Matches(line))
                {
                    if (match.Groups["X"].Success)
                    {
                        parsedNumbers.Add(int.Parse(match.Groups["X"].Value));
                        continue;
                    }

                    parsedNumbers.Add(int.Parse(match.Groups["Y"].Value));
                }

                line = sr.ReadLine();
            }

            Console.WriteLine(
                $"""
                Calling instance of ClawCalibrator with
                xMoveA: {parsedNumbers[0]}
                yMoveA: {parsedNumbers[1]}
                xMoveB: {parsedNumbers[2]}
                yMoveB: {parsedNumbers[3]}

                PrizeLocation (x, y): ({parsedNumbers[4]}, {parsedNumbers[5]})
                """);

            ClawCalibrator clawCalibrator = new([.. parsedNumbers]);

            List<(int A, int B)> buttonPresses = clawCalibrator.Cartesian();

            parsedNumbers.Clear();
            if (buttonPresses.Count is 0)
            {
                Console.WriteLine($"No solutions found, next batch\n");
                continue;
            }

            int lowestCost = buttonPresses.Select(tup => tup.A * 3 + tup.B).Min();

            Console.WriteLine($"{lowestCost} found as minimum amount of tokens needed.\n");

            result += lowestCost;

        }

        return result;
    }


}

public partial class Helpers
{

    [GeneratedRegex(@"(?=X[=+](?<X>\d+))|(?=Y[=+](?<Y>\d+))")]
    public static partial Regex Numbers();
}
