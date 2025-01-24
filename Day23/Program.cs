using AdventUtilities;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day23/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day23/input.txt")
};

string[] input = File.ReadAllLines(filePaths["challenge"]);

Dictionary<string, int> frequencies = input.Select(elem => elem.Split('-'))
                                           .SelectMany(s => s)
                                           .GroupBy(s => s)
                                           .ToDictionary(g => g.Key, g => g.Count());

using StreamWriter sw = new("Notes/frequencies.txt", true);
foreach (var kvp in frequencies)
{
    sw.WriteLine(kvp);
}