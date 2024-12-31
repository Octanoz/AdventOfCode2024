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

        int[] registers = [0, 0, 0];

        (int, int)[] operations = goal.Split(',')
                                      .Select(int.Parse)
                                      .Batch(2)
                                      .Select(arr => (arr[0], arr[1]))
                                      .ToArray();

        goal = goal.Replace(",", "");

        filePath = Path.Combine(InputData.GetSolutionDirectory(), "Day17/Notes/output.txt");

        string output = "";
        int length = goal.Length;
        int regA = int.MaxValue / 2;
        // for (int iteration = 0; iteration < 256; iteration++)
        while (output != goal)
        {
            // regA = output.Length > length ? (long)(regA * 1.1) : (long)(regA * 0.9);
            // registers[0] = regA;

            ThreeBit computer = new(registers, operations);

            output = computer.Run(true);
            // if (output.Length != length)
            // {
            //     continue;
            // }

            File.AppendAllText(filePath, $"G:{goal}\tO:{output}\tR:{regA}\nbinaryO: {ToBinary(output)}\nbinaryR: {ToBinary(regA)}\n");
        }

        return regA;
    }

    private static int DigitCount(long num) => (int)Math.Floor(Math.Log10(num) + 1);

    private static string ToBinary(string numString)
    {
        long num = long.Parse(numString);
        return Convert.ToString(num, 2);
    }

    private static string ToBinary(long num) => Convert.ToString(num, 2);

}
