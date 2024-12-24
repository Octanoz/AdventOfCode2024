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
                    int col = int.Parse(groups["position"].Captures[0].Value);
                    int row = int.Parse(groups["position"].Captures[1].Value);

                    position = new(row, col);
                }

                int speedCol = int.Parse(groups["speed"].Captures[0].Value);
                int speedRow = int.Parse(groups["speed"].Captures[1].Value);

                robots.Add(new(position, speedCol, speedRow));
            }
        }

        return robots;
    }
}

public partial class Helpers
{
    [GeneratedRegex(@"(?<=p=(?<position>(?<col>\d+),(?<row>\d+)))|(?<=v=(?<speed>(?<col>-?\d+),(?<row>-?\d+)))")]
    public static partial Regex Robots();
}
