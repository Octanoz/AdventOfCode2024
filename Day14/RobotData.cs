using System.Text.RegularExpressions;
using AdventUtilities;

namespace Day14;

public static class RobotData
{
    public static List<Robot> ParseRobots(string filePath)
    {
        List<Robot> robots = [];

        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;

            Coord position = Coord.Zero;
            foreach (var groups in Helpers.Robots().Matches(line).Select(match => match.Groups))
            {
                if (groups["position"].Success)
                {
                    int row = int.Parse(groups["row"].Value);
                    int col = int.Parse(groups["col"].Value);

                    position = new(row, col);
                    continue;
                }

                int speedRow = int.Parse(groups["row"].Value);
                int speedCol = int.Parse(groups["col"].Value);

                robots.Add(new(position, speedCol, speedRow));
            }
        }

        return robots;
    }
}

public partial class Helpers
{
    [GeneratedRegex(@"(\d+)")]
    public static partial Regex Numbers();

    [GeneratedRegex(@"(?<position>p=(?<col>\d+),(?<row>\d+))|(?<speed>v=(?<col>-?\d+),(?<row>-?\d+))")]
    public static partial Regex Robots();
}
