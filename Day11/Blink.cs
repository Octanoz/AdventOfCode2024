using System.Collections.Concurrent;

namespace Day11;

public class Blink(string input)
{
    private LinkedList<long> stones = new(Array.ConvertAll(input.Split(), long.Parse));
    private ConcurrentDictionary<int, List<long>> stoneCounts = [];

    private Dictionary<int, List<long>> stoneDigitCounts = [];

    public int PartOne(int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            BlinkTimes();
        }

        checked
        {
            return stones.Count;
        }
    }

    public long PartTwo(int blinkCount)
    {
        PopulateDictionary();

        for (int i = 0; i < blinkCount; i++)
        {
            BlinkIfTheyAreInTheRoomWithYou();
        }

        return stoneDigitCounts.Values.Sum(list => list.Count);
    }

    private void PopulateDictionary()
    {
        foreach (var stone in stones)
        {
            int digits = stone is 0 ? 0 : DigitCount(stone);
            stoneDigitCounts[digits] = stoneDigitCounts.TryGetValue(digits, out var stored) ? [.. stored, stone] : [stone];
        }
    }

    private void BlinkIfTheyAreInTheRoomWithYou()
    {
        Dictionary<int, List<long>> blinkedCounts = [];

        foreach (var (countKey, countValueList) in stoneDigitCounts)
        {
            switch (countKey)
            {
                case 0:
                    var onesies = countValueList.Select(x => x = 1).ToList();
                    blinkedCounts[1] = blinkedCounts.TryGetValue(1, out var storedList) ? [.. storedList, .. onesies] : onesies;
                    break;

                case 1:
                    var currentYears = countValueList.Where(x => x is 1L).Select(x => x = 2024L).ToList();
                    blinkedCounts[4] = blinkedCounts.TryGetValue(4, out storedList) ? [.. storedList, .. currentYears] : currentYears;

                    foreach (var stone in countValueList.Where(x => x > 1L))
                    {
                        long newStone = stone * 2024;
                        int digits = DigitCount(newStone);
                        blinkedCounts[digits] = blinkedCounts.TryGetValue(digits, out storedList) ? [.. storedList, newStone] : [newStone];
                    }
                    break;

                case var ck when ck % 2 is 0:
                    int half = countKey / 2;
                    var (splits, zeroes) = SplitStones(countValueList);
                    var halves = splits.Where(x => DigitCount(x) == half).ToList();
                    blinkedCounts[half] = blinkedCounts.TryGetValue(half, out storedList) ? [.. storedList, .. halves] : halves;

                    foreach (var stone in splits.Where(x => DigitCount(x) < half))
                    {
                        int digits = DigitCount(stone);
                        blinkedCounts[digits] = blinkedCounts.TryGetValue(digits, out storedList) ? [.. storedList, stone] : [stone];
                    }

                    if (zeroes is not 0)
                    {
                        List<long> newZeroesList = Enumerable.Repeat(0L, (int)zeroes).ToList();
                        blinkedCounts[0] = blinkedCounts.TryGetValue(0, out var zeroList) ? [.. zeroList, .. newZeroesList] : newZeroesList;
                    }
                    break;

                default:
                    foreach (var stone in countValueList)
                    {
                        long newStone = stone * 2024;
                        int digits = DigitCount(newStone);
                        blinkedCounts[digits] = blinkedCounts.TryGetValue(digits, out storedList) ? [.. storedList, newStone] : [newStone];
                    }
                    break;
            }
        }

        stoneDigitCounts = new(blinkedCounts);
    }

    public long PartTwoParallel(int blinkCount)
    {
        PopulateDictionaryParallel();

        for (int i = 0; i < blinkCount; i++)
        {
            ParallelBlink();
        }

        return stoneCounts.Values.Sum(list => list.Count);
    }

    private void PopulateDictionaryParallel()
    {
        Parallel.ForEach(stones, stone =>
        {
            int digits = stone is 0 ? 0 : DigitCount(stone);
            stoneCounts.AddOrUpdate(digits, [stone], (stone, existingList) =>
            {
                lock (existingList)
                {
                    existingList.Add(stone);
                }

                return existingList;
            });
        });
    }

    private void ParallelBlink()
    {
        ConcurrentDictionary<int, List<long>> local = [];

        Parallel.ForEach(stoneCounts, kvp =>
        {
            int countKey = kvp.Key;
            List<long> countValueList = kvp.Value;

            Dictionary<int, List<long>> blinkedCounts = [];

            List<long> storedList;
            switch (countKey)
            {
                case 0:
                    var onesies = Enumerable.Repeat(1L, countValueList.Count).ToList();
                    if (!blinkedCounts.TryGetValue(1, out storedList))
                    {
                        blinkedCounts[1] = onesies;
                    }
                    else storedList.AddRange(onesies);
                    break;
                case 1:
                    var currentYears = countValueList.Where(n => n is 1).Select(n => 2024L).ToList();
                    if (!blinkedCounts.TryGetValue(4, out storedList))
                    {
                        blinkedCounts[4] = currentYears;
                    }
                    else storedList.AddRange(currentYears);

                    foreach (var stone in countValueList.Where(n => n > 1))
                    {
                        long newStone = stone * 2024L;
                        int digits = DigitCount(newStone);

                        if (!blinkedCounts.TryGetValue(digits, out storedList))
                        {
                            blinkedCounts[digits] = [newStone];
                        }
                        else storedList.Add(newStone);
                    }
                    break;
                case var ck when ck % 2 is 0:
                    var (splits, zeroes) = SplitStones(countValueList);
                    foreach (var splitStone in splits)
                    {
                        int digits = DigitCount(splitStone);
                        if (!blinkedCounts.TryGetValue(digits, out storedList))
                        {
                            blinkedCounts[digits] = [splitStone];
                        }
                        else storedList.Add(splitStone);
                    }

                    if (zeroes is not 0)
                    {
                        if (!blinkedCounts.TryGetValue(0, out storedList))
                        {
                            blinkedCounts[0] = Enumerable.Repeat(0L, (int)zeroes).ToList();
                        }
                        else storedList.AddRange(Enumerable.Repeat(0L, (int)zeroes).ToList());
                    }
                    break;
                default:
                    foreach (var stone in countValueList)
                    {
                        long newStone = stone * 2024L;
                        int digits = DigitCount(newStone);

                        if (!blinkedCounts.TryGetValue(digits, out storedList))
                        {
                            blinkedCounts[digits] = [newStone];
                        }
                        else storedList.Add(newStone);
                    }
                    break;
            }

            /* foreach (var (key, longList) in blinkedCounts)
            {
                local.AddOrUpdate(key, longList, (longList, existingList) =>
                {
                    lock (existingList)
                    {
                        existingList.AddRange(longList);
                    }

                    return existingList;
                });
            } */

            lock (local)
            {
                foreach (var (key, val) in blinkedCounts)
                {
                    if (!local.TryGetValue(key, out var existingList))
                    {
                        local[key] = val;
                    }
                    else existingList.AddRange(val);
                }
            }
        });

        stoneCounts = new(local);
    }

    private void BlinkTimes()
    {
        var currentNode = stones.First;
        while (currentNode is not null)
        {
            var nextNode = currentNode.Next;

            switch (currentNode.Value)
            {
                case 0:
                    currentNode.Value = 1;
                    break;
                case var n when HasEvenDigitCount(n):
                    var (left, right) = SplitStone(n);

                    stones.AddBefore(currentNode, left);
                    stones.AddAfter(currentNode, right);
                    stones.Remove(currentNode);
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

    private static (long, long) SplitStone(long num)
    {
        int halfLength = DigitCount(num) / 2;
        long divisor = Divisor(halfLength);

        return (num / divisor, num % divisor);
    }

    private static (List<long>, long) SplitStones(List<long> stones)
    {
        List<long> halves = [];
        long zeroCount = 0;

        foreach (var stone in stones)
        {
            var (left, right) = SplitStone(stone);

            if (right is 0)
            {
                halves.Add(left);
                zeroCount++;
            }
            else halves.AddRange([left, right]);
        }

        return (halves, zeroCount);
    }
}
