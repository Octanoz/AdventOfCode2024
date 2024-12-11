
using AdventUtilities;
using Day07;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day07/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day07/input.txt")
};


string[] input = File.ReadAllLines(filePaths["challenge"]);
long result = OperatorStack.ValidateEquations(input);

Console.WriteLine(result);