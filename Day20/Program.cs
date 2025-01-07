#define PART1
#undef PART1
using AdventUtilities;

using Day20;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day20/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day20/input.txt")
};


#if PART1
input = File.ReadAllLines(filePaths["challenge"]);
Dictionary<int, int> cheatFrequencies = Mapper.PartOne(input);

foreach (var kvp in cheatFrequencies)
{
    Console.WriteLine(kvp);
}

Console.WriteLine(cheatFrequencies.Values.Sum());
#else

string[] input = File.ReadAllLines(filePaths["challenge"]);
Dictionary<int, int> testFrequencies = Mapper.PartTwo(input);

foreach (var kvp in testFrequencies)
{
    Console.WriteLine(kvp);
}

Console.WriteLine(testFrequencies.Values.Sum());

#endif