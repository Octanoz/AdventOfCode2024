namespace Day21;

using System.Text;

using AdventUtilities;

public class SequenceBuilder
{
    private static char[,] numpad =
    {
        {'7', '8', '9'},
        {'4', '5', '6'},
        {'1', '2', '3'},
        {'X', '0', 'A'}
    };

    private static char[,] arrowpad =
    {
        {'X', '^', 'A'},
        {'<', 'v', '>'}
    };

    private readonly Dictionary<(char origin, char goal), List<string>> shortestSequences = [];

    private readonly Dictionary<(string code, int layer), ulong> shortestPaths = [];

    public ulong GetShortestSequenceLength(string code, int layer)
    {
        if (shortestPaths.TryGetValue((code, layer), out var cached))
        {
            return cached;
        }

        if (layer == 26)
        {
            shortestPaths[(code, layer)] = (ulong)code.Length;
            return (ulong)code.Length;
        }

        ulong bestLength = 0;
        char previous = 'A';
        for (int codeIndex = 0; codeIndex < code.Length; codeIndex++)
        {
            char currentKey = code[codeIndex];
            var paths = shortestSequences;
            var shortestPathsFound = paths[(previous, currentKey)];

            ulong currentBest = ulong.MaxValue;
            foreach (var path in shortestPathsFound)
            {
                var entry = GetShortestSequenceLength(path, layer + 1);
                if (currentBest > entry)
                {
                    currentBest = entry;
                }
            }

            bestLength += currentBest;
            previous = currentKey;
        }

        shortestPaths[(code, layer)] = bestLength;
        return bestLength;
    }



    public string GetShortestSequence(string code, int layer)
    {
        if (layer == 3)
        {
            return code;
        }

        StringBuilder sequence = new();
        char previousPosition = 'A';

        foreach (var letter in code)
        {
            var shortestPaths = shortestSequences[(previousPosition, letter)];

            string bestSolution = null;
            foreach (var path in shortestPaths)
            {
                string currentSolution = GetShortestSequence(path, layer + 1);
                if (bestSolution is null || currentSolution.Length < bestSolution.Length)
                {
                    bestSolution = currentSolution;
                }
            }

            sequence.Append(bestSolution);

            previousPosition = letter;
        }

        return sequence.ToString();
    }

    public void CacheAllShortestSequences()
    {
        CacheAllShortestSequences(numpad);
        CacheAllShortestSequences(arrowpad);
    }

    private void CacheAllShortestSequences(char[,] keypad)
    {
        int maxRow = keypad.GetLength(0);
        int maxCol = keypad.GetLength(1);

        foreach (var letter in keypad)
        {
            foreach (var otherLetter in keypad)
            {
                shortestSequences[(letter, otherLetter)] = [];
                if (letter == otherLetter)
                {
                    shortestSequences[(letter, otherLetter)].Add("A");
                }

                CacheShortestSequence(letter, otherLetter, keypad, maxRow, maxCol);
            }
        }
    }

    private void CacheShortestSequence(char origin, char goal, char[,] keypad, int maxRow, int maxCol)
    {
        Coord start = FindCharacter(origin, keypad);
        Coord end = FindCharacter(goal, keypad);

        int shortestPathLength = start.Manhattan(end);

        Queue<(Coord coord, string seq)> queue = [];
        queue.Enqueue((start, ""));

        while (queue.Count is not 0)
        {
            var (current, sequence) = queue.Dequeue();

            if (current == end)
            {
                shortestSequences[(origin, goal)].Add(sequence + "A");
                continue;
            }

            if (sequence.Length >= shortestPathLength)
            {
                continue;
            }

            foreach (var neighbour in current.GetValidNeighbours(maxRow, maxCol).Where(nb => keypad[nb.Row, nb.Col] != 'X'))
            {
                char arrow = (neighbour - current) switch
                {
                    (-1, 0) => '^',
                    (0, 1) => '>',
                    (1, 0) => 'v',
                    (0, -1) => '<',
                    _ => throw new InvalidOperationException($"Invalid calculation, the difference should be 1 at most.")
                };

                queue.Enqueue((neighbour, sequence + arrow));
            }
        }
    }

    private static Coord FindCharacter(char letter, char[,] keypad)
    {
        for (int row = 0; row < keypad.GetLength(0); row++)
        {
            for (int col = 0; col < keypad.GetLength(1); col++)
            {
                if (keypad[row, col] == letter)
                {
                    return new Coord(row, col);
                }
            }
        }

        throw new InvalidOperationException($"Could not find character [{letter}] on this keypad");
    }
}
