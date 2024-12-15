namespace Day09;

using System.Text;
using System.Text.RegularExpressions;

public class SpanDefragmentation(string input)
{
    public long PartTwo()
    {
        string fileBuffer = ParseCompression();
        string compactFile = CompactBlocks(fileBuffer);

        return CalculateChecksum(compactFile);
    }

    private string ParseCompression()
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

        return sb.ToString().Replace(".", "/");
    }

    private string CompactBlocks(string fileBuffer)
    {
        OrderedDictionary<int, Range> fileLengths = [];
        List<Range> freeSpace = [];

        foreach (var range in Helpers.Blocks().EnumerateSplits(fileBuffer))
        {
            var block = fileBuffer.AsSpan(range);
            if (Char.IsDigit(block[0]))
            {
                int commaIndex = block.IndexOf(',');
                int fileId = int.Parse(block[..commaIndex]);
                fileLengths.Add(fileId, range);
            }
            else freeSpace.Add(range);
        }

        return DictToString(fileLengths, freeSpace);
    }

    private string CompactBlocksAlt(string fileBuffer)
    {
        SortedDictionary<int, Range> fileLengths = new();
        List<Range> freeSpace = [];

        // Parse the file buffer to extract block and free space ranges
        foreach (var range in Helpers.Blocks().EnumerateSplits(fileBuffer))
        {
            var block = fileBuffer.AsSpan(range);
            if (char.IsDigit(block[0]))
            {
                int commaIndex = block.IndexOf(',');
                int fileId = int.Parse(block[..commaIndex]);
                fileLengths[fileId] = range;
            }
            else
            {
                freeSpace.Add(range);
            }
        }

        // Process blocks from highest to lowest fileId
        foreach (var fileId in fileLengths.Keys.OrderByDescending(k => k).ToList())
        {
            Range currentRange = fileLengths[fileId];
            int blockLength = currentRange.End.Value - currentRange.Start.Value;

            int freeIndex = freeSpace.FindIndex(r => r.End.Value - r.Start.Value >= blockLength);
            if (freeIndex == -1) continue; // No free space large enough

            // Move the block to the free space
            Range targetRange = freeSpace[freeIndex];
            int newStart = targetRange.Start.Value;
            int newEnd = newStart + blockLength;

            fileLengths[fileId] = new Range(newStart, newEnd);

            // Update the free space list
            freeSpace.RemoveAt(freeIndex);
            if (newEnd < targetRange.End.Value)
            {
                freeSpace.Add(new Range(newEnd, targetRange.End.Value));
            }
        }

        // Generate the compacted buffer
        char[] compactedBuffer = fileBuffer.ToCharArray();
        foreach (var (fileId, range) in fileLengths)
        {
            for (int i = 0; i < range.End.Value - range.Start.Value; i++)
            {
                compactedBuffer[range.Start.Value + i] = (char)('0' + fileId);
            }
        }

        return new string(compactedBuffer);
    }

    private string DictToString(OrderedDictionary<int, Range> fileLengths, List<Range> freeSpace)
    {
        HashSet<int> movedBlocks = [];
        int fileId = int.MaxValue;

        while (fileId > 0 && movedBlocks.Count > fileLengths.Count)
        {
            int blocks = fileLengths.Count;
            var (currentBlock, currentRange) = fileLengths.GetAt(blocks - 1);

            if (!movedBlocks.Add(currentBlock))
            {
                continue;
            }

            int currentLength = currentRange.End.Value - currentRange.Start.Value;

            int availableIndex = freeSpace.FindIndex(r => r.End.Value - r.Start.Value >= currentLength);
            Range newRange = freeSpace[availableIndex];
            int target = newRange.End.Value + 1;

            for (int i = 0; i < fileLengths.Count; i++)
            {
                var (_, storedRange) = fileLengths.GetAt(i);

                if (storedRange.Start.Value == target)
                {
                    fileLengths.Insert(i, currentBlock, currentRange);
                }
            }

        }
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
    private long CalculateChecksum(Span<int> diskSpan)
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

    private bool IsEmpty(int[] blocks) => Array.TrueForAll(blocks, n => n == -1);
    private bool IsEmpty(char[] blocks) => Array.TrueForAll(blocks, c => c is '/' or '.');
    private bool IsEmpty(Span<char> blockSpan) => !blockSpan.ContainsAnyExcept('.');
}

