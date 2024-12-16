namespace Day09;

public class SpanDefragmentation(string input)
{
    private static readonly Dictionary<int, Range> fileRanges = [];
    private static readonly List<Range> freeSpaces = [];
    public long PartTwo()
    {
        int[] fileBlocks = ParseCompression();
        PopulateCollections(fileBlocks);
        int[] defragmented = CompactBlocks(fileBlocks);

        return CalculateChecksum(defragmented);
    }


    private int[] ParseCompression()
    {
        List<int> parsedBuffer = [];
        int fileId = 0;

        for (int i = 0; i < input.Length; i++)
        {
            int count = input[i] - '0';
            if (i % 2 is 0)
            {
                parsedBuffer.AddRange(Enumerable.Repeat(fileId++, count));
            }
            else parsedBuffer.AddRange(Enumerable.Repeat(-1, count));
        }

        return [.. parsedBuffer];
    }

    private static void PopulateCollections(int[] buffer)
    {
        fileRanges.Clear();
        freeSpaces.Clear();

        Span<int> spanBuffer = buffer.AsSpan();

        int start = 0;
        while (start < spanBuffer.Length)
        {
            int current = spanBuffer[start];
            int end = start;

            while (end < spanBuffer.Length && spanBuffer[end] == current)
            {
                end++;
            }

            Range range = new(start, end);
            if (current is -1)
            {
                freeSpaces.Add(range);
            }
            else fileRanges[current] = range;

            start = end;
        }
    }

    private static int[] CompactBlocks(int[] buffer)
    {
        Span<int> spanBuffer = buffer.AsSpan();

        foreach (var (currentId, currentRange) in fileRanges.Reverse())
        {
            Span<int> fileBlock = spanBuffer[currentRange];
            int blockLength = fileBlock.Length;

            int freeIndex = freeSpaces.FindIndex(space => space.End.Value - space.Start.Value >= blockLength);
            if (freeIndex is -1
            || freeSpaces[freeIndex].Start.Value > currentRange.Start.Value)
            {
                continue;
            }

            Range freeRange = freeSpaces[freeIndex];
            Span<int> targetSpan = spanBuffer[freeRange.Start..(freeRange.Start.Value + blockLength)];

            fileBlock.CopyTo(targetSpan);

            spanBuffer[currentRange].Fill(-1);

            if (freeRange.End.Value - freeRange.Start.Value == blockLength)
            {
                freeSpaces.RemoveAt(freeIndex);
            }
            else freeSpaces[freeIndex] = new(freeRange.Start.Value + blockLength, freeRange.End);

            fileRanges[currentId] = new(freeRange.Start, freeRange.Start.Value + blockLength);
        }

        return buffer;
    }

    private static long CalculateChecksum(int[] defragmented) =>
        defragmented.Index()
                    .Where(elem => elem.Item is not -1)
                    .Aggregate(0L, (acc, elem) => acc + elem.Item * elem.Index);
}

