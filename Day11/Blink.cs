using System.Collections.Concurrent;
using AdventUtilities;

namespace Day11;

public class Blink(string input)
{
    private readonly LinkedList<long> linkedStones = new(Array.ConvertAll(input.Split(), long.Parse));

    private static readonly Dictionary<long, List<long>> cachedSplits = new()
    {
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
        Dictionary<long, long> currentState = linkedStones.GroupBy(n => n)
                                                          .ToDictionary(g => g.Key, g => (long)g.Count());

        Dictionary<long, long> newState = [];
        for (int i = 0; i < blinkCount; i++)
        {
            foreach (var (num, count) in currentState)
            {
                BlinkIfTheyAreInTheRoomWithYou(num, count, newState);
            }
            currentState = new(newState);
            newState.Clear();
        }

        return currentState.Values.Sum();
    }

    private static void BlinkIfTheyAreInTheRoomWithYou(long stone, long freq, Dictionary<long, long> next)
    {
        switch (stone)
        {
            case 0:
                next[1] = next.TryGetValue(1, out var cached) ? cached + freq : freq;
                break;

            case var n when HasEvenDigitCount(n):
                var splits = SplitStone(n);

                next[splits[0]] = next.TryGetValue(splits[0], out cached) ? cached + freq : freq;
                next[splits[1]] = next.TryGetValue(splits[1], out cached) ? cached + freq : freq;
                break;

            default:
                long newStone = stone * 2024L;
                next[newStone] = next.TryGetValue(newStone, out cached) ? cached + freq : freq;
                break;
        }
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
        if (cachedSplits.TryGetValue(num, out var cachedResult))
        {
            return cachedResult;
        }

        int halfLength = DigitCount(num) / 2;
        long divisor = Divisor(halfLength);

        List<long> result = cachedSplits[num] = [num / divisor, num % divisor];

        return result;
    }
}