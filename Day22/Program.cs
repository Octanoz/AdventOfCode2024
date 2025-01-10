
using AdventUtilities;

using Day22;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day22/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day22/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);
long result = Negotiate.PartOne(input);

Console.WriteLine(result);
