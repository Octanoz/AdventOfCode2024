#define PART1
#undef PART1

using AdventUtilities;
using Day4;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day4/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day4/input.txt")
};

#if PART1
Span<string> input = File.ReadAllLines(filePaths["challenge"]);
WordFinder wf = new("XMAS");

Console.WriteLine(wf.FindTarget(input));
#else

string[] input = File.ReadAllLines(filePaths["challenge"]);
Console.WriteLine(WordFinder.FindCrosses(input));
#endif