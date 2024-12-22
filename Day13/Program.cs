
using AdventUtilities;
using Day13;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day13/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day13/input.txt")
};

int result = Solver.PartOne(filePaths["example1"]);

Console.WriteLine(result);