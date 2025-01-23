#define PART1
#undef PART1
using AdventUtilities;

using Day22;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day22/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day22/example2.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day22/input.txt")
};

string[] input = await File.ReadAllLinesAsync(filePaths["challenge"]);
#if PART1
long result = Negotiate.PartOne(input);
Console.WriteLine(result);
#else

// Negotiate.AnalyzeSellers(input);
int result = Negotiate.PartTwo(input);

Console.WriteLine(result);
#endif
