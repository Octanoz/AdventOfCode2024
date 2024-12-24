
using AdventUtilities;
using Day14;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day14/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day14/input.txt")
};

List<Robot> robotList = RobotData.ParseRobots(filePaths["example"]);