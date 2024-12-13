
namespace Day09;

using System.Text;
using System.Text.RegularExpressions;

public class Defragmenter(string input)
{
    private readonly string compressedStructure = input;

    public long PartOne()
    {
        int[] fileBlocks = ParseCompression();
        int[] compactFile = Compacter(fileBlocks);

        return CalculateChecksum(compactFile);
    }

    public long PartTwo()
    {
        int[][] fileBlocks = ParseBlocks();
        int[] compactFile = CompactBlocks(fileBlocks);

        return CalculateChecksum(compactFile);
    }

    private int[] CompactBlocks(int[][] fileBlocks)
    {
        int[] freeBlocks = fileBlocks.Index().SelectMany(row => row.Item.Where(n => n is -1)).ToArray();

        return freeBlocks;
    }

    private int[][] ParseBlocks()
    {
        List<int[]> fileBlocks = [];
        foreach (Match match in Regex.Matches(compressedStructure, @"(?<file>(\d)\1{0,})|(?<free>(\.+))"))
        {
            if (match.Groups["file"].Success)
            {
                fileBlocks.Add(Enumerable.Repeat(match.Value[0] - '0', match.Value.Length).ToArray());
                continue;
            }

            fileBlocks.Add(Enumerable.Repeat(-1, match.Length).ToArray());
        }

        return [.. fileBlocks];
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
}

static partial class Helpers
{
    [GeneratedRegex(@"(?<file>(\d)\1{0,})|(?<free>(\.+))")]
    public static partial Regex Blocks();
}
