
namespace Day09;

using System.Text;
using System.Text.RegularExpressions;

public class Defragmenter(string input)
{
    public long PartOne()
    {
        int[] fileBlocks = ParseCompression();
        int[] compactFile = Compacter(fileBlocks);

        return CalculateChecksum(compactFile);
    }

    public long PartTwo()
    {
        List<int[]> fileBlocks = ParseBlocks();
        fileBlocks.RemoveAll(arr => arr.Length is 0);

        List<int[]> defragmented = CompactBlocks(fileBlocks);

        return CalculateChecksumBlocks(defragmented);
    }

    private List<int[]> CompactBlocks(List<int[]> fileBlocks)
    {
        int left = 0;
        int right = fileBlocks.Count - 1;
        while (right > 0)
        {
            int emptyLength = -1;
            if (IsEmpty(fileBlocks[right]))
            {
                right--;
                continue;
            }

            int fileLength = fileBlocks[right].Length;
            while (left < right && emptyLength < fileLength)
            {
                if (!IsEmpty(fileBlocks[left]))
                {
                    left++;
                    continue;
                }

                emptyLength = fileBlocks[left].Length;
                if (emptyLength < fileLength)
                {
                    left++;
                }
            }

            if (left >= right)
            {
                right--;
                left = 0;
                continue;
            }

            if (emptyLength > fileLength)
            {
                int[] move = fileBlocks[left][fileLength..];
                fileBlocks[left] = fileBlocks[left][..fileLength];

                (fileBlocks[left], fileBlocks[right]) = (fileBlocks[right], fileBlocks[left]);
                if (IsEmpty(fileBlocks[left + 1]))
                {
                    fileBlocks[left + 1] = [.. fileBlocks[left + 1], .. move];
                }
                else fileBlocks.Insert(left + 1, move);
            }

            if (emptyLength == fileLength)
            {
                (fileBlocks[left], fileBlocks[right]) = (fileBlocks[right], fileBlocks[left]);
            }

            CleanBlocks(fileBlocks);
        }

        return fileBlocks;
    }

    private List<int[]> ParseBlocks()
    {
        int[] numSequence = input.Select(c => c - '0').ToArray();
        int fileId = 0;
        List<int[]> fileBlocks = [];
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 == 0)
            {
                fileBlocks.Add(Enumerable.Repeat(fileId, numSequence[i]).ToArray());
                fileId++;
                continue;
            }

            fileBlocks.Add(Enumerable.Repeat(-1, numSequence[i]).ToArray());
        }

        return fileBlocks;
    }

    private int[] ParseCompression()
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

    private int[] Compacter(int[] fileBlocks)
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

    private long CalculateChecksum(int[] compactFile) => compactFile.Index().Sum(elem => (long)elem.Index * elem.Item);

    private long CalculateChecksum(IEnumerable<int> compact) => compact.Index().Sum(elem => elem.Item <= 0 ? 0 : (long)elem.Index * elem.Item);

    private long CalculateChecksumBlocks(List<int[]> fileBlocks)
    {
        int total = 0;
        foreach (var (index, elem) in fileBlocks.SelectMany(n => n).Index())
        {
            if (elem > 0)
            {
                total += elem * index;
            }
        }

        return total;
    }

    private List<int[]> GetBlocksFileId(List<int[]> blocks, int fileId) => blocks.FindAll(arr => Array.TrueForAll(arr, n => n == fileId));

    private static void CleanBlocks(List<int[]> blocks)
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

    private static bool IsEmpty(int[] arr) => Array.TrueForAll(arr, n => n is -1);
}

static partial class Helpers
{
    [GeneratedRegex(@"(?<file>(\d)\1{0,})|(?<free>(\.+))")]
    public static partial Regex Blocks();
}
