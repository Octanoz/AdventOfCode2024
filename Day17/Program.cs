#define PART1
#undef PART1

using AdventUtilities;
using Day17;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day17/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day17/example2.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day17/input.txt")
};

#if PART1
string result = Operator.PartOne(filePaths["challenge"]);
Console.WriteLine(result);
#else

long resultNum = Operator.PartTwo(filePaths["challenge"]);
Console.WriteLine(resultNum);
#endif