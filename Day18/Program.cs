#define PART1
#undef PART1
using AdventUtilities;

using Day18;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day18/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day18/input.txt")
};

#if PART1
// int result = Plotter.PartOne(filePaths["example1"], true);
int result = Plotter.PartOne(filePaths["challenge"]);
Console.WriteLine(result);
#else
var (x, y) = Plotter.PartTwo(filePaths["challenge"]);
Console.WriteLine($"Coordinate of last blocking byte: ({x}, {y})");

#endif
