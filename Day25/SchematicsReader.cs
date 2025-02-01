namespace Day25;

using AdventUtilities;

using CommunityToolkit.HighPerformance;

public class SchematicsReader(string filePath)
{
    private static readonly List<Lock> locks = [];
    private static readonly List<Key> keys = [];

    public void ProcessInput()
    {
        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            Parse(sr);
        }
    }

    private static void Parse(StreamReader sr)
    {
        int[] columns = [-1, -1, -1, -1, -1];
        string[] schematic = new string[7];

        string? line = sr.ReadLine() ?? throw new InvalidDataException($"Trying to parse an empty file");
        bool isLock = line[0] is '#';

        int index = 0;
        while (!String.IsNullOrEmpty(line))
        {
            schematic[index++] = line;
            line = sr.ReadLine();
        }

        Span2D<char> schematicSpan = schematic.New2DGrid<char>();
        for (int i = 0; i < columns.Length; i++)
        {
            foreach (var letter in schematicSpan.GetColumn(i))
            {
                if (letter is '#')
                {
                    columns[i]++;
                }
            }
        }

        if (isLock)
        {
            locks.Add(new(columns));
        }
        else
            keys.Add(new(columns));
    }

    public List<Lock> GetLocks() => new(locks);
    public List<Key> GetKeys() => new(keys);
}
