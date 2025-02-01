
using AdventUtilities;

using Day25;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day25/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day25/input.txt")
};

int result = Locksmith.PartOne(filePaths["challenge"]);
Console.WriteLine(result);
