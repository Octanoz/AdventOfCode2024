namespace Day21;

public static class KeypadManager
{
    public static int PartOne(string[] input)
    {
        SequenceBuilder seqBuilder = new();
        seqBuilder.CacheAllShortestSequences();

        int result = 0;
        foreach (var code in input)
        {
            int numericPart = int.Parse(new string(code.Where(Char.IsDigit).ToArray()));
            string shortestSequence = seqBuilder.GetShortestSequence(code, 0);

            result += shortestSequence.Length * numericPart;
        }

        return result;
    }

    public static ulong PartTwo(string[] input)
    {
        SequenceBuilder seqBuilder = new();
        seqBuilder.CacheAllShortestSequences();

        ulong result = 0;
        foreach (var code in input)
        {
            ulong numericPart = (ulong)int.Parse(new string(code.Where(Char.IsDigit).ToArray()));
            ulong shortestLength = seqBuilder.GetShortestSequenceLength(code, 0);

            result += shortestLength * numericPart;
        }

        return result;
    }
}
