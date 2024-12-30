using AdventUtilities;
using SuperLinq;

namespace Day17;

public static class Operator
{
    /* public static string PartOne(string filePath)
    {
        string input = File.ReadAllText(filePath);

        int[] nums = input.Split('\n', ',', ' ')
                          .Where(elem => Char.IsDigit(elem[0]))
                          .Select(int.Parse)
                          .ToArray();

        var registers = nums[..3];

        (int, int)[] operations = nums[3..].Batch(2)
                                           .Select(arr => (arr[0], arr[1]))
                                           .ToArray();

        ThreeBit computer = new(registers, operations);

        return computer.Run();
    } */

    public static long PartTwo(string filePath)
    {
        using StreamReader sr = new(filePath);

        string goal = "";
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (String.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.StartsWith("Program:"))
            {
                goal = line.Split(' ')[1];
            }
        }

        long[] registers = [0, 0, 0];

        (int, int)[] operations = goal.Split(',')
                                      .Select(int.Parse)
                                      .Batch(2)
                                      .Select(arr => (arr[0], arr[1]))
                                      .ToArray();

        goal = goal.Replace(",", "");

        filePath = Path.Combine(InputData.GetSolutionDirectory(), "Day17/Notes/output.txt");

        long result = SearchInput(filePath, goal, registers, operations);

        return result;
    }

    private static long SearchInput(string filePath, string goal, long[] registers, (int, int)[] operations)
    {
        long left = 0, right = long.MaxValue;
        long goalNum = long.Parse(goal);
        int length = goal.Length;

        string output = "";

        while (left <= right)
        {
            long mid = left + (right - left) / 2;
            output = BuildComputerAndRun(mid, registers, operations);

            File.AppendAllText(filePath, $"G:{goal}\tO:{output}\tR:{mid}\n");

            if (output.Length == length)
            {
                long outputNum = long.Parse(output);
                if (outputNum == goalNum)
                {
                    return mid;
                }

                if (outputNum < goalNum)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid;
                }
            }

            if (output.Length < length)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return -1;
    }

    private static string BuildComputerAndRun(long mid, long[] registers, (int, int)[] operations)
    {
        registers[0] = mid;
        ThreeBit computer = new(registers, operations);

        return computer.Run(true);
    }

    private static int DigitCount(long num) => (int)Math.Floor(Math.Log10(num) + 1);

}
