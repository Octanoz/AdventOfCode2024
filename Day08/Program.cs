#define PART1
#undef PART1
using AdventUtilities;
using Day08;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), @"Day08\example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), @"Day08\input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

PropagationMarker pm = new(input);
#if PART1
var antennaeResult = pm.FindAntennae();

foreach (var kvp in antennaeResult)
{
    Console.WriteLine($"{kvp.Key}: [ {String.Join(", ", kvp.Value)} ]");
}

Console.WriteLine(pm.PartOne());
#else

Console.WriteLine(pm.PartTwo());
#endif