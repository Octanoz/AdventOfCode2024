using AdventUtilities;

using Day23;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day23/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day23/input.txt")
};

string[] input = await File.ReadAllLinesAsync(filePaths["challenge"]);
int result = EasterBunnyAdmin.PartOne(input);

Console.WriteLine(result);

/*Dictionary<string, int> frequencies = input.SelectMany(elem => elem.Split('-'))
                                           .CountBy(s => s)
                                           .ToDictionary(g => g.Key, g => g.Value);

// using StreamWriter sw = new("Notes/frequencies.txt", true);
foreach (var kvp in frequencies)
{
    Console.WriteLine(kvp);
}*/