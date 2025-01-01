
using AdventUtilities;

using Day18;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day18/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day18/input.txt")
};

//char[,] map = Plotter.PlotMapExample(filePaths["example1"], 7, 7);

//map.Draw2DGridTightXY();

//? 260 was too low
// int result = Plotter.PartOne(filePaths["example1"], true);
int result = Plotter.PartOne(filePaths["challenge"]);
Console.WriteLine(result);


