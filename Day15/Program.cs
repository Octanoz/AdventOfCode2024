#define PART1
#undef PART1
using AdventUtilities;
using Day15;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day15/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day15/example2.txt"),
    ["example3"] = Path.Combine(InputData.GetSolutionDirectory(), "Day15/example3.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day15/input.txt")
};

#if PART1
int result = Solver.PartOne(filePaths["challenge"]);
#else
int result = Solver.PartTwo(filePaths["challenge"]);
#endif
Console.WriteLine(result);

