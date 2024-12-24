#define PART1
#undef PART1
using AdventUtilities;
using Day13;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day13/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day13/input.txt")
};

#if PART1
int result = Solver.PartOne(filePaths["challenge"]);
#else
long result = Solver.PartTwo(filePaths["challenge"]);
#endif

Console.WriteLine(result);
