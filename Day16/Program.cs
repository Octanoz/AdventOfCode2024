#define PART1
#undef PART1
using AdventUtilities;
using Day16;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day16/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day16/example2.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day16/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

#if PART1
int result = MazeRunner.PartOne(input);
#else
int result = MazeRunner.PartTwo(input);
#endif

Console.WriteLine(result);