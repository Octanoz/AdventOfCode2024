#define PART1
#undef PART1
using AdventUtilities;
using Day11;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day11/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day11/input.txt")
};


string input = File.ReadAllText(filePaths["challenge"]);

Blink blink = new(input);
#if PART1
int resultOne = blink.PartOne(25);
#else
long result = blink.PartTwoParallel(75);
Console.WriteLine(result);
#endif
