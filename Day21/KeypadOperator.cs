namespace Day21;

using AdventUtilities;

using static SuperLinq.SuperEnumerable;
public class KeypadOperator(string input)
{
    private readonly string input = input;
    private int index = 0;

    Dictionary<char, Coord> keypad = new()
    {
        ['X'] = Coord.Zero,
        ['^'] = new(0, 1),
        ['A'] = new(0, 2),
        ['<'] = new(1, 0),
        ['v'] = new(1, 1),
        ['>'] = new(1, 2)
    };

    private static int maxRow = 2;
    private static int maxCol = 3;

    public string EnterCode()
    {
        string result = "";
        Coord currentKey = new(0, 2);
        CoordComparer comparer = new();
        while (index < input.Length)
        {
            Coord targetKey = keypad[input[index++]];
            var path = GetShortestPath<Coord, int>(
                currentKey,
                (state, cost) => state.GetValidNeighbours(maxRow, maxCol).Select(coord => (coord, GetCost(coord), 1)),
                targetKey,
                comparer,
                null);

            result += String.Join(" | ", path.Select(x => $"{x.nextState}"));
            currentKey = targetKey;
        }

        return ConvertToDirections(result);
    }

    private static string ConvertToDirections(string coordsString)
    {
        string result = "";
        bool isAction = false;
        int repeat = 0;
        string[] coords = coordsString.Split(" | ");
        Console.WriteLine("\nKeypad moves:");
        Array.ForEach(coords, Console.WriteLine);
        Coord previousCoord = ParseCoord(coords[0]);
        foreach (var state in coords[1..])
        {
            if (state.Length > 6)
            {
                isAction = true;
                repeat = state.Length / 6;

            }

            Coord current = ParseCoord(state);
            result += (current - previousCoord) switch
            {
                (-1, 0) => '^',
                (0, 1) => '>',
                (1, 0) => 'v',
                (0, -1) => '<',
                _ => throw new ArgumentException($"Either previousCoord {previousCoord} or the next state {current}is invalid, they should be immediate neighbours")
            };

            if (isAction)
            {
                while (--repeat > 0)
                {
                    result += 'A';
                }

                isAction = false;
            }

            previousCoord = current;
        }

        result += 'A';

        return result;
    }

    private static Coord ParseCoord(string coordString) => new((int)(coordString[1] - '0'), (int)(coordString[4] - '0'));

    private static int GetCost(Coord coord) => coord switch
    {
        (0, 0) => 1000,
        (1, 0) => 5,
        (1, 1) => 5,
        (0, _) => 2,
        (1, 2) => 1,
        _ => throw new ArgumentException($"Qu'est ce que c'est le plus de plus de merde c'est quoi? {coord}")
    };
}
