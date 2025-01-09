#define PART1
#undef PART1
using AdventUtilities;

using Day21;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day21/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day21/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

#if PART1
int result = KeypadManager.PartOne(input);
#else
ulong result = KeypadManager.PartTwo(input);
#endif

Console.WriteLine(result);

