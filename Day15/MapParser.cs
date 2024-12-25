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
            }

            string directionsLine = sr.ReadLine()!;
            while (!String.IsNullOrEmpty(directionsLine))
            {
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

                directionsLine = sr.ReadLine()!;
            }
        }

        return GridExtensions.JaggedCharArray(lines.ToArray());
    }
}
