namespace Day09;

using System.Text;
using System.Text.RegularExpressions;

public class SpanDefragmentation(string input)
{
    private readonly string compressedStructure = input;

    public long PartTwo() => CompactAndChecksum(MoveWholeFiles);
    public long PartOne()
    {
        int[] fileBlocks = ParseCompressionOriginal();
        int[] compactFile = CompacterOriginal(fileBlocks);

        return CalculateChecksum(compactFile);
    }

    private int[] ParseCompressionOriginal()
    {
        int[] intSequence = input.Select(c => c - '0').ToArray();
        int fileId = 0;
        StringBuilder sb = new();
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 is 0)
            {
                sb.Append((char)(fileId + '0'), intSequence[i]);
                fileId++;
                continue;
            }

            sb.Append('.', intSequence[i]);
        }
        return sb.ToString().Replace(".", "/").Select(c => c - '0').ToArray();
    }

    private int[] CompacterOriginal(int[] fileBlocks)
    {
        int left = 0;
        int right = fileBlocks.Length - 1;

        while (left < right)
        {
            if (fileBlocks[left] is not -1)
            {
                left++;
                continue;
            }

            if (fileBlocks[right] is -1)
            {
                right--;
                continue;
            }

            (fileBlocks[left], fileBlocks[right]) = (fileBlocks[right], fileBlocks[left]);
        }

        int negIndex = Array.IndexOf(fileBlocks, -1);
        return fileBlocks[..negIndex];
    }

    private long CompactAndChecksum(Func<Span<int>, int[]> compactingStrategy)
    {
        int[] fileBlocks = ParseCompression();
        int[] compacted = compactingStrategy(fileBlocks.AsSpan());
        return CalculateChecksum(compacted);
    }

    private int[] ParseCompression()
    {
        ReadOnlySpan<char> span = compressedStructure.AsSpan();
        List<int> blocks = [];

        int fileId = 0;
        foreach (var range in Helpers.FileOrFreeSpace().EnumerateSplits(compressedStructure))
        {
            ReadOnlySpan<char> chunk = span[range];
            int length = int.Parse(chunk);

            // Alternate between file and free space
            if (fileId % 2 == 0)
            {
                blocks.AddRange(Enumerable.Repeat(fileId / 2, length)); // File ID
            }
            else
            {
                blocks.AddRange(Enumerable.Repeat(-1, length)); // Free space
            }

            fileId++;
        }

        return blocks.ToArray();
    }

    private int[] MoveWholeFiles(Span<int> diskSpan)
    {
        var files = IdentifyBlocks(diskSpan, isFreeSpace: false);
        var freeSpaces = IdentifyBlocks(diskSpan, isFreeSpace: true);

        files.Sort((a, b) => b.Id.CompareTo(a.Id)); // Process files in descending ID order

        foreach (var file in files)
        {
            for (int i = 0; i < freeSpaces.Count; i++)
            {
                var (Id, Start, Length) = freeSpaces[i];
                if (Length >= file.Length && Start < file.Start)
                {
                    // Move file to the leftmost suitable free space
                    diskSpan.Slice(Start, file.Length).Fill(file.Id);
                    diskSpan.Slice(file.Start, file.Length).Fill(-1);

                    // Update free spaces
                    UpdateFreeSpaces(freeSpaces, Start, file.Length);
                    break;
                }
            }
        }

        return diskSpan.ToArray();
    }

    private List<(int Id, int Start, int Length)> IdentifyBlocks(Span<int> span, bool isFreeSpace)
    {
        List<(int Id, int Start, int Length)> blocks = [];
        int start = 0;

        while (start < span.Length)
        {
            int current = span[start];
            if (current == -1 == isFreeSpace)
            {
                int length = 1;
                while (start + length < span.Length && span[start + length] == current)
                {
                    length++;
                }

                blocks.Add((current, start, length));
                start += length;
            }
            else
            {
                start++;
            }
        }

        return blocks;
    }

    private void UpdateFreeSpaces(List<(int Id, int Start, int Length)> freeSpaces, int start, int length)
    {
        int index = freeSpaces.FindIndex(fs => fs.Start == start);
        if (index >= 0)
        {
            var (freeId, freeStart, freeLength) = freeSpaces[index];
            freeSpaces.RemoveAt(index);

            if (freeLength > length)
            {
                freeSpaces.Insert(index, (freeId, freeStart + length, freeLength - length));
            }
        }
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
}

static partial class Helpers
{
    [GeneratedRegex(@"\d")]
    public static partial Regex FileOrFreeSpace();
}
