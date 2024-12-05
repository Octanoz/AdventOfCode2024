#define PART1
#undef PART1

using Day2;
using AdventUtilities;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

#if PART1
Console.WriteLine(ReportReader.PartOne(input));
#else
Console.WriteLine(ReportReader.PartTwo(input));
#endif



