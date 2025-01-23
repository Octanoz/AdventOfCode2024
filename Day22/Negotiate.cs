namespace Day22;

using AdventUtilities;

using CommunityToolkit.HighPerformance;

public static class Negotiate
{
    public static long PartOne(string[] input)
    {
        int[] secretNumbers = Array.ConvertAll(input, int.Parse);
        long result = 0;
        foreach (var secret in secretNumbers)
        {
            result += NumberCooking.ProcessSecretTimes(secret, 2000);
        }

        return result;
    }

    public static int PartTwo(string[] input)
    {
        Dictionary<(int, int, int, int), int> sequences = [];
        int[] secretNumbers = Array.ConvertAll(input, int.Parse);

        foreach (var secret in secretNumbers)
        {
            SequenceFinder.GetSequences(secret, sequences);
        }

        return sequences.Values.Max();
    }
}
