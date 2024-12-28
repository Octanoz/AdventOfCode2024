
using AdventUtilities;
using Day15;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day15/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day15/example2.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day15/input.txt")
};

int result = Solver.PartOne(filePaths["example1"]);
Console.WriteLine(result);

