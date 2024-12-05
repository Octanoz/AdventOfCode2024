#define PART1
#undef PART1

using Day3;
using AdventUtilities;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day3/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day3/example2.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day3/input.txt")
};

#if PART1
ReadOnlySpan<string> input = File.ReadAllLines(filePaths["challenge"]);

Console.WriteLine(Scanner.PartOne(input));
#else

string inputString = File.ReadAllText(filePaths["challenge"]);

//* Clean up the string before processing. Awkwardly placed values can be corrupted by \r\n
Console.WriteLine(Scanner.PartTwo(inputString.Replace("\r\n", "")));
#endif


