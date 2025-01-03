
using AdventUtilities;

using Day19;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day19/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day19/input.txt")
};

//? 20707955558 was too low
long result = Onsen.PartTwo(filePaths["challenge"]);
Console.WriteLine(result);
