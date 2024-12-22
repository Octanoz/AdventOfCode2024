using System.Text.RegularExpressions;
using SuperLinq;

namespace Day13;

public static class Solver
{
    public static int PartOne(string filePath)
    {
        List<int> numbers = [];
        List<Machine> machines = [];

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
                        numbers.Add(int.Parse(match.Groups["X"].Value));
                        continue;
                    }

                    numbers.Add(int.Parse(match.Groups["Y"].Value));
                }

                line = sr.ReadLine();
            }

            machines.Add(new([.. numbers]));
            numbers.Clear();

        }
        machines.RemoveAt(3);
        machines.RemoveAt(1);

        int minCost = machines.Sum(PlayMachine);

        return minCost;
    }

    private static int PlayMachine(Machine m)
    {
        int[] buttonPresses = [0, 0];
        int lowestCost = int.MaxValue;

        List<(int, int)> combos = m.FindCombos();

        if (combos.Count is 0)
        {
            Console.WriteLine($"No solution found for machine with Prize at {m.PrizeLocation}");
            return 0;
        }

        Play(m, combos);

        if (lowestCost < int.MaxValue)
        {
            Console.WriteLine($"Finished checking machine with prize at {m.PrizeLocation}");
        }

        return lowestCost;

        void Play(Machine machine, List<(int, int)> combos)
        {
            /* if (ClawTooFar(m) || machine.GetTotalCost() > lowestCost || buttonPresses[0] > 100 || buttonPresses[1] > 100)
            {
                return;
            } */

            if (ClawHasPrize(machine))
            {
                if (machine.GetTotalCost() < lowestCost)
                {
                    Console.WriteLine($"Claw reached prize, new lowest cost: {machine.GetTotalCost()}");
                }

                lowestCost = int.Min(lowestCost, machine.GetTotalCost());
                return;
            }

            foreach (var (pressA, pressB) in combos)
            {
                if (buttonPresses[0] < pressA)
                {
                    machine.PressA();
                    buttonPresses[0]++;
                    Play(machine, combos);

                    machine.UndoA();
                    buttonPresses[0]--;
                }

                if (buttonPresses[1] < pressB)
                {
                    machine.PressB();
                    buttonPresses[1]++;
                    Play(machine, combos);

                    machine.UndoB();
                    buttonPresses[1]--;
                }
            }
        }
    }

    private static void MarkDone((int, int)[] possibleSequences, int index, HashSet<(int, int)> visited) => visited.Add(possibleSequences[index]);
    private static bool ClawTooFar(Machine m) => m.ClawLocation.Row > m.PrizeLocation.Row && m.ClawLocation.Col > m.PrizeLocation.Col;
    private static bool ClawHasPrize(Machine m) => m.ClawLocation == m.PrizeLocation;
}

public partial class Helpers
{

    [GeneratedRegex(@"(?=X[=+](?<X>\d+))|(?=Y[=+](?<Y>\d+))")]
    public static partial Regex Numbers();
}

public enum Stage
{
    Setup,
    MinMax
}