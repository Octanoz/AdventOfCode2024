namespace Day07;

public class OperatorStack
{
    private static readonly List<long> validResults = [];
    public long PartOne(string[] input)
    {

        foreach (var sequence in input)
        {
            int colonIndex = sequence.IndexOf(':');
            long target = long.Parse(sequence[..colonIndex]);

            long[] numSequence = Array.ConvertAll(sequence[(colonIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse).ToArray();
            long startValue = numSequence[0];

            if (CycleOperators(startValue, numSequence.AsSpan()[1..], target))
            {
                validResults.Add(target);
            }
        }

        return validResults.Sum();
    }

    private bool CycleOperators(long currentValue, Span<long> numbers, long target)
    {
        if (numbers.Length == 0)
            return currentValue == target;

        if (currentValue > target)
            return false;

        long nextValue = numbers[0];
        Span<long> remainingNumbers = numbers[1..];

        // Check addition
        if (currentValue <= long.MaxValue - nextValue && CycleOperators(currentValue + nextValue, remainingNumbers, target))
        {
            return true;
        }

        // Check multiplication
        if (currentValue <= long.MaxValue / nextValue && CycleOperators(currentValue * nextValue, remainingNumbers, target))
        {
            return true;
        }

        return false;
    }
}
