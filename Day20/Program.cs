
using AdventUtilities;

using Day20;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day20/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day20/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);
Dictionary<int, int> cheatFrequencies = Mapper.PartOne(input);

foreach (var kvp in cheatFrequencies)
{
    Console.WriteLine(kvp);
}

Console.WriteLine(cheatFrequencies.Values.Sum());

//Travel in opposite direction and mark every open space with the steps from the finish.
//Compare distance to go between current position and position after one wall cell