namespace Day05;

public class Page(int id)
{
    private readonly Dictionary<int, int> stored = [];

    public int Id { get; } = id;
    public List<Page> Before { get; private set; } = [];
    public List<Page> After { get; private set; } = [];

    public void AddAfter(Page afterPage, HashSet<int> visited)
    {
        if (!stored.TryGetValue(afterPage.Id, out var cached))
        {
            After.Add(afterPage);
            cached = 0;
        }

        if (visited.Add(afterPage.Id) && cached < afterPage.GetTotalStored())
        {
            for (int i = 0; i < afterPage.After.Count; i++)
            {
                AddAfter(afterPage.After[i], visited);
            }

            stored[afterPage.Id] = afterPage.GetTotalStored();
        }
    }

    public void AddBefore(Page beforePage, HashSet<int> visited)
    {
        if (!stored.TryGetValue(beforePage.Id, out var cached))
        {
            Before.Add(beforePage);
            cached = 0;
        }

        if (visited.Add(beforePage.Id) && cached < beforePage.GetTotalStored())
        {
            foreach (var pageBefore in beforePage.Before)
            {
                AddBefore(pageBefore, visited);
            }

            stored[beforePage.Id] = beforePage.GetTotalStored();
        }
    }

    public int[] GetStoredPages() => stored.Keys.ToArray();

    public int GetTotalStored() => Before.Count + After.Count;
}
