using System.Collections;

public record Page(int Id)
{
    private readonly BitArray stored = new(100);
    public List<int> Before { get; private set; } = [];
    public List<int> After { get; private set; } = [];

    public void ProcessSeedRule(Span<int> rule)
    {
        switch (rule.IndexOf(Id))
        {
            case 0:
                After.Add(rule[1]);
                stored[rule[1]] = true;
                break;
            case 1:
                Before.Add(rule[0]);
                stored[rule[0]] = true;
                break;
            default:
                throw new ArgumentException($"Cannot use rule [{rule.ToString()}] for seeding as it doesn't contain the root ID: {Id}");
        }
    }

    public void AddAfter(int insertingPage, int existingPage)
    {
        int afterIndex = After.IndexOf(existingPage);
        if (afterIndex is not -1)
        {
            if (existingPage == After[^1])
            {
                After.Add(insertingPage);
            }
            else After.Insert(afterIndex + 1, insertingPage);
        }
        else if (existingPage == Before[^1])
        {
            Before.Add(insertingPage);
        }
        else
        {
            int index = Before.IndexOf(existingPage);
            Before.Insert(index, insertingPage);
        }

        stored[insertingPage] = true;
    }

    public void AddBefore(int insertingPage, int existingPage)
    {
        int index = After.IndexOf(existingPage);
        if (index is not -1)
        {
            After.Insert(index, insertingPage);
        }
        else
        {
            index = Before.IndexOf(existingPage);
            Before.Insert(index, insertingPage);
        }

        stored[insertingPage] = true;
    }

    public Span<int> GetStoredPages() => stored.Cast<int>().ToArray();

    public int GetTotalStored() => Before.Count + After.Count;
}

public enum Branch
{
    Before,
    After
}
