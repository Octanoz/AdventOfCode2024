
using AdventUtilities;
using Day12;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example2.txt"),
    ["example3"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example3.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day12/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

int resultOne = FencePricing.PartOne(input);

Console.WriteLine(resultOne);