﻿using AdventUtilities;
using Day11;

Dictionary<string, string> filePaths = new()
{
    ["example1"] = Path.Combine(InputData.GetSolutionDirectory(), "Day11/example1.txt"),
    ["challenge"] = Path.Combine(InputData.GetSolutionDirectory(), "Day11/input.txt")
};

string input = File.ReadAllText(filePaths["challenge"]);

Blink blink = new(input);

// int resultOne = blink.PartOne(25);
long result = blink.PartTwo(75);
Console.WriteLine(result);
