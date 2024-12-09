#define PART1
#define PART2
// #undef PART1
#undef PART2

using Day1;
using AdventUtilities;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day01/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day01/input.txt")
};

ReadOnlySpan<string> input = File.ReadAllLines(filePaths["challenge"]);

#if PART1
Console.WriteLine(Solver.PartOne(input));
#elif PART2
Console.WriteLine(Solver.PartTwo(input));
#else
Console.WriteLine(Solver.Alternative(filePaths["challenge"]));
#endif


