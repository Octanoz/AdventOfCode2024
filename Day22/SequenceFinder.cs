namespace Day22;

using CommunityToolkit.HighPerformance;

public static class SequenceFinder
{
    public static void GetSequences(long secretNumber, Dictionary<(int, int, int, int), int> sequences)
    {
        HashSet<(int, int, int, int)> visited = [];
        List<int> differences = new(4);
        long current = secretNumber;
        int currentSingle;
        int previousSingle = (int)(current % 10);

        for (int i = 0; i < 2000; i++)
        {
            current = NumberCooking.ProcessSecretNumber(current);
            currentSingle = (int)(current % 10);

            int diff = currentSingle - previousSingle;
            differences.Add(diff);
            if (differences.Count is 4)
            {
                var currentSequence = (differences[0], differences[1], differences[2], differences[3]);
                if (visited.Add(currentSequence))
                {
                    sequences[currentSequence] = sequences.TryGetValue(currentSequence, out int storedSum)
                                                ? storedSum + currentSingle
                                                : currentSingle;
                }

                differences.RemoveAt(0);
            }

            previousSingle = currentSingle;
        }
    }
}
