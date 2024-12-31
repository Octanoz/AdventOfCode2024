#define PART1
#undef PART1

using AdventUtilities;

using Day17;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day17/example1.txt"),
    ["example2"] = Path.Combine(InputData.GetSolutionDirectory(), "Day17/example2.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day17/input.txt")
};

#if PART1
string result = Operator.PartOne(filePaths["challenge"]);
Console.WriteLine(result);
#else

List<int> output = [2, 4, 1, 5, 7, 5, 1, 6, 0, 3, 4, 0, 5, 5, 3, 0];

ReverseEngineer revEngineer = new(output);
string initialA = revEngineer.Setup();
Console.WriteLine($"Initial value of register A: {initialA}");
Console.WriteLine($"[ {String.Join(", ", output)}]");

#endif


