
using AdventUtilities;
using Day14;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day14/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day14/input.txt")
};

// int result1 = Solver.PartOne(filePaths["challenge"], 103, 101);

// long result1 = Solver.PartOne(filePaths["example1"], 7, 11);

Solver.PartTwo(filePaths["challenge"], 103, 101);

// Console.WriteLine(result1);