
using AdventUtilities;
using Day09;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), @"Day09\example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), @"Day09\input.txt")
};

string input = File.ReadAllText(filePaths["challenge"]);
Defragmenter df = new(input);

long result = df.PartOne();

//? was too low
Console.WriteLine(result);
