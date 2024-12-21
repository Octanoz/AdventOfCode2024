#define PART1
#undef PART1
using AdventUtilities;
using Day12;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example2.txt"),
    ["example3"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example3.txt"),
    ["example4"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example4.txt"),
    ["example5"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example5.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

#if PART1
int resultOne = FencePricing.PartOne(input);

Console.WriteLine(resultOne);
#else
int resultTwo = FencePricing.PartTwo(input);

Console.WriteLine(resultTwo);
#endif