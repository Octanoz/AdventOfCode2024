namespace Day21;

using AdventUtilities;

using SuperLinq;

using static SuperLinq.SuperEnumerable;

public class NumpadOperator(char[] input)
{
    private readonly char[] input = input;
    private int index = 0;
    private static Dictionary<char, Coord> numpad = new()
    {
        ['7'] = Coord.Zero,
        ['8'] = new(0, 1),
        ['9'] = new(0, 2),
        ['4'] = new(1, 0),
        ['5'] = new(1, 1),
        ['6'] = new(1, 2),
        ['1'] = new(2, 0),
        ['2'] = new(2, 1),
        ['3'] = new(2, 2),
        ['X'] = new(3, 0),
        ['0'] = new(3, 1),
        ['A'] = new(3, 2)
    };

    private static int maxRow = 4;
    private static int maxCol = 3;

    public string EnterCode()
    {
        string result = "";
        Coord currentKey = new(3, 2);
        CoordComparer comparer = new();
        int stepsTaken = 0;
        while (index < input.Length)
        {
            Coord targetKey = numpad[input[index++]];
            if (targetKey == currentKey)
            {
                stepsTaken = 0;
            }

            var path = GetShortestPath<Coord, int>(
                currentKey,
                (state, cost) => state.GetValidNeighbours(maxRow, maxCol).Select(coord => (coord, GetCost(currentKey, coord, stepsTaken++), 1)),
                targetKey,
                comparer,
                null);

            result += String.Join(" | ", path.Select(x => $"{x.nextState}"));
            currentKey = targetKey;
        }

        return ConvertToDirections(result);
    }

    private int GetCost(Coord origin, Coord coord, int stepsTaken)
    {
        switch ((origin, coord))
        {
            case (_, (3, 0)):
                return 1000;

            case var (hc, lc) when hc.Col > lc.Col:
                if (stepsTaken < 2)
                {
                    return 1;
                }
                else
                    return 20;

            case var (lr, hr) when lr.Row < hr.Row:
                if (stepsTaken < 2)
                {
                    return 1;
                }
                else
                    return 10;

            default:
                return 3;
        }
    }

    private static string ConvertToDirections(string coordsString)
    {
        string result = "";
        bool isAction = false;
        string[] coords = coordsString.Split(" | ");
        // Console.WriteLine("\nNumpad instructions:");
        // Array.ForEach(coords, Console.WriteLine);
        Coord previousCoord = ParseCoord(coords[0]);
        foreach (var state in coords[1..])
        {
            if (state.Length > 6)
                isAction = true;


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
                result += 'A';
                isAction = false;
            }

            previousCoord = current;
        }

        result += 'A';

        return result;
    }

    private static Coord ParseCoord(string coordString) => new((int)(coordString[1] - '0'), (int)(coordString[4] - '0'));

    /* private static int GetCost(Coord current, Coord target, Coord nextMove)
    {
        // Calculate Manhattan distance
        int distanceCost = Math.Abs(current.Row - target.Row) + Math.Abs(current.Col - target.Col);

        // Penalize turns: Moving in the same column is cheaper
        int turnPenalty = current.Col != target.Col ? 2 : 0;

        // Additional penalty for zig-zagging across columns
        int columnPenalty = current.Col == 1 || target.Col == 1 ? 1 : 0;

        // Encourage straight-line moves in the middle column
        if (current.Col == 1 && target.Col == 1)
            columnPenalty = -1;

        return distanceCost + turnPenalty + columnPenalty;
    } */

    private static bool WithinBounds(Coord c) => c.Row >= 0 && c.Row < maxRow
                                              && c.Col >= 0 && c.Col < maxCol
                                              && c != new Coord(3, 0);


}
