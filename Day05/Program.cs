
using AdventUtilities;
using Day05;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day05/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day05/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);
OrderValidator ov = new(input);

Console.WriteLine(ov.Process(true));