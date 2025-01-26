#define PART1
#undef PART1

using AdventUtilities;

using Day23;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day23/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day23/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

#if PART1
int result = EasterBunnyAdmin.PartOne(input);
Console.WriteLine(result);
#else
string longestCluster = EasterBunnyAdmin.PartTwo(input);
Console.WriteLine(longestCluster);
#endif