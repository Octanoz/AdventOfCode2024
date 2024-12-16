#define PART1
#undef PART1
using AdventUtilities;
using Day09;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), @"Day09\example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), @"Day09\input.txt")
};

string input = File.ReadAllText(filePaths["challenge"]);
Defragmenter df = new(input);

#if PART1
long result = df.PartOne();
#else
SpanDefragmentation sd = new(input);
long result = sd.PartTwo();
#endif
Console.WriteLine(result);
