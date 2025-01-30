#define PART1
#undef PART1

using AdventUtilities;

using Day24;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day24/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day24/example2.txt"),
    ["example3"] = Path.Combine(InputData.GetSolutionDirectory(), "Day24/example3.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day24/input.txt")
};

#if PART1
long result = LogicProcessor.PartOne(filePaths["challenge"]);

Console.WriteLine(result);
#else

var result = LogicProcessor.PartTwo(filePaths["challenge"]);

Console.WriteLine(result);


#endif

