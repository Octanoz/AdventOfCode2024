namespace Day07;

public static class OperatorStack
{
    private static readonly Lock lockObject = new();
    public static long ValidateEquations(string[] input, bool partTwo = false)
    {
        long validResults = 0;

        Parallel.ForEach(input, sequence =>
        {
            int colonIndex = sequence.IndexOf(':');
            long target = long.Parse(sequence[..colonIndex]);

            long[] numSequence = Array.ConvertAll(sequence[(colonIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse);
            long startValue = numSequence[0];

            if (CycleOperators(startValue, numSequence.AsSpan()[1..], target, partTwo))
            {
                using (lockObject.EnterScope())
                {
                    validResults += target;
                }
            }
        });

        return validResults;
    }

    private static bool CycleOperators(long currentValue, Span<long> numbers, long target, bool partTwo = false)
    {
        if (numbers.Length == 0)
        {
            return currentValue == target;
        }
        if (currentValue > target)
        {
            return false;
        }

        long nextValue = numbers[0];
        Span<long> remainingNumbers = numbers[1..];

        // Addition
        if (currentValue <= long.MaxValue - nextValue && CycleOperators(currentValue + nextValue, remainingNumbers, target, partTwo))
        {
            return true;
        }

        // Multiplication
        if (currentValue <= long.MaxValue / nextValue && CycleOperators(currentValue * nextValue, remainingNumbers, target, partTwo))
        {
            return true;
        }

        // Concatenation
        if (partTwo)
        {
            long concatenatedValue = Concatenate(currentValue, nextValue);
            if (concatenatedValue > 0 && CycleOperators(concatenatedValue, remainingNumbers, target, partTwo))
            {
                return true;
            }
        }

        return false;
    }

    private static long Concatenate(long left, long right)
    {
        long multiplier = 1;
        while (multiplier <= right)
        {
            multiplier *= 10;
        }

        return checked(left * multiplier + right);
    }
}
