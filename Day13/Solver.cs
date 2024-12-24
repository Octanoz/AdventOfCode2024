using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using AdventUtilities;
using AdventUtilities.LongCoordinates;
using SuperLinq;

namespace Day13;

public static class Solver
{
    public static int PartOne(string filePath)
    {
        List<int> parsedNumbers = [];
        int result = 0;

        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
            while (!String.IsNullOrEmpty(line))
            {
                foreach (var groupCollection in Helpers.Numbers().Matches(line).Select(match => match.Groups))
                {
                    if (groupCollection["X"].Success)
                    {
                        parsedNumbers.Add(int.Parse(groupCollection["X"].Value));
                        continue;
                    }

                    parsedNumbers.Add(int.Parse(groupCollection["Y"].Value));
                }

                line = sr.ReadLine()!;
            }

            ClawCalibrator clawCalibrator = new([.. parsedNumbers]);

            List<(int A, int B)> buttonPresses = clawCalibrator.Cartesian();

            parsedNumbers.Clear();

            if (buttonPresses.Count is 0)
                continue;

            int lowestCost = buttonPresses.Select(tup => tup.A * 3 + tup.B).Min();
            result += lowestCost;
        }

        return result;
    }

    public static long PartTwo(string filePath)
    {
        List<long> parsedNumbers = [];
        List<Machine> machines = [];

        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
            while (!String.IsNullOrEmpty(line))
            {
                foreach (var groups in Helpers.Numbers().Matches(line).Select(match => match.Groups))
                {
                    if (groups["X"].Success)
                    {
                        parsedNumbers.Add(long.Parse(groups["X"].Value));
                        continue;
                    }

                    parsedNumbers.Add(long.Parse(groups["Y"].Value));
                }

                line = sr.ReadLine()!;
            }

            //Positional class uses row, column which is y, x. Values are parsed in the order x, y
            Button buttonA = new(new(parsedNumbers[1], parsedNumbers[0]));
            Button buttonB = new(new(parsedNumbers[3], parsedNumbers[2]));
            CoordL prizeLocation = new(parsedNumbers[5], parsedNumbers[4]);

            machines.Add(new(buttonA, buttonB, prizeLocation));
            parsedNumbers.Clear();
        }

        return ProcessMachines(machines);
    }

    public static long ProcessMachines(IEnumerable<Machine> machines)
    {
        long totalTokens = 0L;
        foreach (var machine in machines)
        {
            if (CanWin(machine, out var tokensUsed))
            {
                totalTokens += tokensUsed;
            }
        }

        return totalTokens;
    }

    private static bool CanWin(Machine machine, out long tokens)
    {
        long shift = 10_000_000_000_000L;
        CoordL newPrizeLocation = machine.Prize + new CoordL(shift, shift);

        var (prizeY, prizeX) = newPrizeLocation;

        double buttonAX = machine.A.GetColMove();
        double buttonAY = machine.A.GetRowMove();
        double buttonBX = machine.B.GetColMove();
        double buttonBY = machine.B.GetRowMove();

        long b = (long)Math.Round((prizeY - (prizeX / buttonAX) * buttonAY) / (buttonBY - (buttonBX / buttonAX) * buttonAY));
        long a = (long)Math.Round((prizeX - b * buttonBX) / buttonAX);

        tokens = 0;
        CoordL actualLocation = new((long)(a * buttonAY + b * buttonBY), (long)(a * buttonAX + b * buttonBX));
        if (actualLocation == newPrizeLocation)
        {
            tokens = a * 3 + b;
            return true;
        }

        return false;
    }
}

public partial class Helpers
{

    [GeneratedRegex(@"(?=X[=+](?<X>\d+))|(?=Y[=+](?<Y>\d+))")]
    public static partial Regex Numbers();
}
