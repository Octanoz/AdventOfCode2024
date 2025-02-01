namespace Day25;

using SuperLinq;

public static class Locksmith
{
    public static int PartOne(string filePath)
    {
        SchematicsReader schemaReader = new(filePath);
        schemaReader.ProcessInput();

        var combinations = schemaReader.GetKeys().Cartesian(schemaReader.GetLocks());

        return combinations.Count(keyLock => keyLock.Item1.Fits(keyLock.Item2));
    }
}

public record Lock(int[] Columns);

public record Key(int[] Columns);

public static class KeyExtensions
{
    public static bool Fits(this Key key, Lock @lock)
    {
        var combined = key.Columns.Index().Select(elem => elem.Item + @lock.Columns[elem.Index]);

        return combined.All(n => n <= 5);
    }
}
