using AdventUtilities;

namespace Day15;

public static class MapParser
{
    public static char[][] ParseMap(string filePath, out List<Direction> moves)
    {
        List<string> lines = [];
        moves = [];
        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
            while (!String.IsNullOrEmpty(line))
            {
                lines.Add(line);
                line = sr.ReadLine()!;
            }

            ParseDirections(sr, moves);
        }

        return lines.ToArray().JaggedCharArray();
    }

    private static void ParseDirections(StreamReader sr, List<Direction> moves)
    {
        while (!sr.EndOfStream)
        {
            string directionsLine = sr.ReadLine()!;

            foreach (var letter in directionsLine)
            {
                moves.Add(letter switch
                {
                    '^' => Direction.Up,
                    '>' => Direction.Right,
                    'v' => Direction.Down,
                    '<' => Direction.Left,
                    _ => throw new InvalidDataException($"Found unexpected character: {letter} when parsing directions")
                });
            }
        }
    }

    public static char[][] ParseMap2(string filePath, out List<Direction> moves)
    {
        Dictionary<char, string> replacements = new()
        {
            ['#'] = "##",
            ['O'] = "[]",
            ['.'] = "..",
            ['@'] = "@."
        };

        List<char[]> lines = [];
        moves = [];
        using StreamReader sr = new(filePath);

        string line = sr.ReadLine()!;
        while (!String.IsNullOrEmpty(line))
        {
            char[] letterArray = new char[line.Length * 2];
            Queue<char> queue = [];
            foreach (var letter in line)
            {
                queue.Enqueue(letter);
            }

            for (int i = 1; i < letterArray.Length; i += 2)
            {
                char currentLetter = queue.Dequeue();
                if (replacements.TryGetValue(currentLetter, out string? storedString))
                {
                    letterArray[i - 1] = storedString[0];
                    letterArray[i] = storedString[1];
                }
                else
                    break;
            }

            lines.Add(letterArray);

            line = sr.ReadLine()!;
        }

        ParseDirections(sr, moves);

        return [.. lines];
    }
}
