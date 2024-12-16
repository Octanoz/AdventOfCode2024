namespace Day09;

using System.Text;
using System.Text.RegularExpressions;

public class SpanDefragmentation(string input)
{
    public long PartTwo()
    {
        int[] fileBlocks = ParseCompression();
        int[] compactFile = CompactBlocks(fileBlocks);

        return CalculateChecksum(compactFile);
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

    private int[] CompactBlocks(int[] buffer)
    {
        Span<int> spanBuffer = buffer.AsSpan();
        OrderedDictionary<int, Range> fileRanges = [];
        List<Range> freeSpaces = [];

        foreach (var range in Helpers.Blocks().EnumerateSplits(new string(buffer.Select(n => n >= 0 ? '1' : '.').ToArray())))
        {
            Span<int> block = spanBuffer[range];
            if (block[0] >= 0)
            {
                fileRanges.Add(block[0], range);
            }
            else freeSpaces.Add(range);
        }

        foreach (var (currentId, currentRange) in fileRanges.Reverse())
        {
            Span<int> fileBlock = spanBuffer[currentRange];
            int blockLength = fileBlock.Length;

            int freeIndex = freeSpaces.FindIndex(space => space.End.Value - space.Start.Value >= blockLength);
            if (freeIndex is -1)
                continue;

            Range freeRange = freeSpaces[freeIndex];
            Span<int> targetSpan = spanBuffer[freeRange.Start..(freeRange.Start.Value + blockLength)];

            fileBlock.CopyTo(targetSpan);

            spanBuffer[currentRange].Fill(-1);

            if (freeRange.End.Value - freeRange.Start.Value == blockLength)
            {
                freeSpaces.RemoveAt(freeIndex);
            }
            else freeSpaces[freeIndex] = new(freeRange.Start.Value + blockLength, freeRange.End);
        }

        return buffer;
    }





    private string DictToString(OrderedDictionary<int, Range> fileLengths, List<Range> freeSpace, string fileBuffer)
    {
        HashSet<int> movedBlocks = [];
        int fileId = int.MaxValue;

        while (fileId > 0 && movedBlocks.Count < fileLengths.Count)
        {
            int currentIndex = fileLengths.Count - 1;
            var (currentBlock, currentRange) = fileLengths.GetAt(currentIndex);
            while (!movedBlocks.Add(currentBlock))
            {
                (currentBlock, currentRange) = fileLengths.GetAt(--currentIndex);
            }

            int currentLength = currentRange.End.Value - currentRange.Start.Value;

            int availableIndex = freeSpace.FindIndex(r => r.End.Value - r.Start.Value >= currentLength);
            Range newRange = freeSpace[availableIndex];
            int target = newRange.End.Value + 1;
            int originalStart = newRange.Start.Value;

            for (int i = 0; i < fileLengths.Count; i++)
            {
                var (_, storedRange) = fileLengths.GetAt(i);

                if (storedRange.Start.Value == target)
                {
                    fileLengths.Insert(i, currentBlock, currentRange);
                }
            }

            Range replaceRange = new(originalStart + currentLength, newRange.End);

            freeSpace[availableIndex] = replaceRange;
        }

        StringBuilder sb = new();

    }

    private static int FindLeftMostSpan(ReadOnlySpan<char> buffer, Range range)
    {
        int emptyLength = 0;
        for (int i = 0; i < range.Start.Value; i++)
        {
            if (buffer[i] is '.')
            {
                emptyLength++;

                if (emptyLength == range.End.Value)
                {
                    return i - range.End.Value + 1;
                }
                else emptyLength = 0;
            }
        }

        return -1;
    }


    private long CalculateChecksum(int[] defragmented) => defragmented.Index()
                                                                      .Where(elem => elem.Item is not -1)
                                                                      .Aggregate(0L, (acc, elem) => acc + elem.Item * elem.Index);

    private long CalculateChecksumBlocks(List<int[]> fileBlocks)
    {
        long total = 0;
        foreach (var (index, elem) in fileBlocks.SelectMany(n => n).Index())
        {
            if (elem > 0)
            {
                checked
                {
                    total += elem * index;
                }
            }
        }

        return total;
    }

    private static void CleanEmptyBlocks(List<int[]> blocks)
    {
        for (int i = 1; i < blocks.Count; i++)
        {
            if (IsEmpty(blocks[i - 1]) && IsEmpty(blocks[i]))
            {
                int[] move = blocks[i - 1];
                blocks.RemoveAt(i - 1);

                blocks[i - 1] = [.. blocks[i - 1], .. move];
            }
        }
    }

    private bool IsEmpty(Span<int> numSpan) => !numSpan.ContainsAnyExcept(-1);
    private bool IsEmpty(int[] blocks) => Array.TrueForAll(blocks, n => n == -1);
    private bool IsEmpty(char[] blocks) => Array.TrueForAll(blocks, c => c is '/' or '.');
    private bool IsEmpty(Span<char> blockSpan) => !blockSpan.ContainsAnyExcept('.');

    private string ParseCompressionOriginal()
    {
        int fileId = 0;
        StringBuilder sb = new();
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 is 0)
            {
                sb.Append((char)(fileId + '0'), input[i] - '0');
                fileId++;
            }
            else sb.Append('.', input[i] - '0');
        }

        return sb.ToString();
    }

    private string CompactBlocksOriginal(string fileBuffer)
    {
        char[] buffer = fileBuffer.ToCharArray();
        Span<char> fileSpan = buffer.AsSpan();
        OrderedDictionary<int, Range> fileLengths = [];
        List<Range> freeSpace = [];

        int fileId = 0;
        foreach (var range in Helpers.Blocks().EnumerateSplits(fileBuffer))
        {
            var block = fileSpan[range];
            if (Char.IsDigit(block[0]))
            {
                fileLengths.Add(fileId++, range);
            }
            else freeSpace.Add(range);
        }

        foreach (var (blockId, blockRange) in fileLengths.Reverse())
        {
            var block = fileSpan[blockRange];
            int blockLength = block.Length;

            int freeIndex = freeSpace.FindIndex(f => f.End.Value - f.Start.Value >= blockLength);
            if (freeIndex is -1)
                break;

            Range targetRange = freeSpace[freeIndex];
            Span<char> targetSpan = fileSpan[targetRange.Start..(targetRange.Start.Value + blockLength)];

            block.CopyTo(targetSpan);

            freeSpace[freeIndex] = (targetRange.End.Value - targetRange.Start.Value) switch
            {
                var len when len == blockLength => new(0, 0),
                var len when len < blockLength => new(targetRange.Start.Value + blockLength, targetRange.End),
                _ => throw new InvalidOperationException(nameof(blockLength))
            };

            fileSpan[blockRange].Fill('.');
        }

        return new(buffer);
    }

    private string CompactBlocksOld(string fileBuffer)
    {
        OrderedDictionary<int, Range> fileLengths = [];
        List<Range> freeSpace = [];

        int fileId = 0;
        foreach (var range in Helpers.Blocks().EnumerateSplits(fileBuffer))
        {
            var block = fileBuffer.AsSpan(range);
            if (Char.IsDigit(block[0]))
            {
                fileLengths.Add(fileId++, range);
            }
            else freeSpace.Add(range);
        }

        return DictToString(fileLengths, freeSpace, fileBuffer);
    }

    private long CalculateChecksumOriginal(Span<int> diskSpan)
    {
        long checksum = 0;
        for (int i = 0; i < diskSpan.Length; i++)
        {
            if (diskSpan[i] != -1)
            {
                checksum += (long)i * diskSpan[i];
            }
        }
        return checksum;
    }


}

