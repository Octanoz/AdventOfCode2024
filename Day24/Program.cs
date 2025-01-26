using AdventUtilities;

using Day24;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day24/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day24/example2.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day24/input.txt")
};

long result = LogicProcessor.PartOne(filePaths["challenge"]);

Console.WriteLine(result);
