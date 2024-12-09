using System.Collections;

namespace Day05;

public record Page(int Id)
{
    public List<int> Before { get; private set; } = [];
    public List<int> After { get; private set; } = [];

    public void AddAfter(int afterPage)
    {
        After.Add(afterPage);
    }

    public void AddBefore(int beforePage)
    {
        Before.Add(beforePage);
    }

    public int[] GetStoredPages() => [.. Before, .. After];
    public int GetTotalStored() => Before.Count + After.Count;
}

public enum Branch
{
    Before,
    After
}
