#define PART1
#undef PART1

using AdventUtilities;
using Day10;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day10/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day10/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

#if PART1
Console.WriteLine(PeakFinder.PartOne(input));
#else
Console.WriteLine(PeakFinder.PartTwo(input));
#endif