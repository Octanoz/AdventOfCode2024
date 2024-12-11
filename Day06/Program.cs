#define PART1
#undef PART1

using AdventUtilities;
using Day06;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day06/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day06/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);
Tracker tracker = new(input);

#if PART1
int result = tracker.PartOne();

Console.WriteLine(result);
#else
int result = tracker.PartTwo();

Console.WriteLine(result);

#endif