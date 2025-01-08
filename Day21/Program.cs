
using AdventUtilities;

using Day21;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day21/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day21/input.txt")
};

string testInput = "029A";
/* NumpadOperator numOp = new(testInput.ToCharArray());
string numpadInput = numOp.EnterCode();
Console.WriteLine(numpadInput);

KeypadOperator keyOp = new(numpadInput);
string keypadInput = keyOp.EnterCode(); */

string[] input = File.ReadAllLines(filePaths["example1"]);

int result = OperatorManager.PartOne(input);
Console.WriteLine(result);

