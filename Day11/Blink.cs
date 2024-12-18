using System.Collections.Concurrent;
using AdventUtilities;

namespace Day11;

public class Blink(string input)
{
    private LinkedList<long> linkedStones = new(Array.ConvertAll(input.Split(), long.Parse));

    private long totalStones = 0;
    private static List<long> stones = [];

    private static readonly Dictionary<long, bool> problematicZeroCache = [];

    private static readonly Dictionary<long, List<long>> cachedResults = new()
    {
        [0L] = [1L],
        [1L] = [2024L],
        [2L] = [4048L],
        [4L] = [8096L],
        [8L] = [16192L],
        [20L] = [2L, 0L],
        [24L] = [2L, 4L],
        [40L] = [4L, 0L],
        [48L] = [4L, 8L],
        [80L] = [8L, 0L],
        [96L] = [9L, 6L],
        [2024L] = [20L, 24L],
        [4048L] = [40L, 48L],
        [8096L] = [80L, 96L]
    };

    private Dictionary<Number, List<long>> stoneNumbers = new()
    {
        [Number.Contains0] = [],
        [Number.Other] = []
    };


    public int PartOne(int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            BlinkTimes();
        }

        return linkedStones.Count;
    }

    public long PartTwo(int blinkCount)
    {
        Setup();

        for (int i = 0; i < blinkCount; i++)
        {
            BlinkIfTheyAreInTheRoomWithYou(blinkCount - i);
        }

        totalStones += stones.Count;

        return totalStones;
    }

    private void Setup() => stones = new(linkedStones);

    private void BlinkIfTheyAreInTheRoomWithYou(int stepsLeft)
    {
        List<long> blinkedStones = [];

        foreach (var stone in stones)
        {
            if (cachedResults.TryGetValue(stone, out var storedResult))
            {
                blinkedStones.AddRange(storedResult);
            }

            if (HasEvenDigitCount(stone))
            {
                blinkedStones.AddRange(SplitStone(stone));
            }
            else
            {
                cachedResults[stone] = [stone * 2024L];
                blinkedStones.AddRange(cachedResults[stone]);
            }
        }

        if (stepsLeft >= 18)
        {
            totalStones += blinkedStones.Count(s => s is 0L && s is 1L) * 54;
        }

        stones = blinkedStones.Where(stone => stone > 1).ToList();
    }

    private void BlinkTimes()
    {
        var currentNode = linkedStones.First;
        while (currentNode is not null)
        {
            var nextNode = currentNode.Next;

            switch (currentNode.Value)
            {
                case 0:
                    currentNode.Value = 1;
                    break;
                case var n when HasEvenDigitCount(n):
                    List<long> splits = SplitStone(n);

                    linkedStones.AddBefore(currentNode, splits[0]);
                    linkedStones.AddAfter(currentNode, splits[1]);
                    linkedStones.Remove(currentNode);
                    break;
                default:
                    currentNode.Value *= 2024;
                    break;
            }

            currentNode = nextNode;
        }
    }

    private static long Divisor(long num) => (long)Math.Pow(10, num);

    private static bool HasEvenDigitCount(long num) => DigitCount(num) % 2 == 0;

    private static int DigitCount(long num) => (int)Math.Floor(Math.Log10(num) + 1);

    private static List<long> SplitStone(long num)
    {
        if (cachedResults.TryGetValue(num, out var cachedResult))
        {
            return cachedResult;
        }

        int halfLength = DigitCount(num) / 2;
        long divisor = Divisor(halfLength);

        List<long> result = cachedResults[num] = [num / divisor, num % divisor];

        return result;
    }

    private static bool ProblematicZero(long num)
    {
        int mid = DigitCount(num) / 2;
        string numString = num.ToString();

        if (numString.AsSpan()[mid..].Contains('0'))
        {
            return true;
        }

        return false;
    }
}

public enum Number
{
    Contains0,
    Other
}
