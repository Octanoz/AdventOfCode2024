using System.Collections;

namespace Day05;

public record Page(int Id);

public record PageFull(int Id)
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

    public void ProcessRules(int[] rule)
    {
        var (before1, before2, after1, after2) = (Before.IndexOf(rule[0]), Before.IndexOf(rule[1]), After.IndexOf(rule[0]), After.IndexOf(rule[1]));
        switch (before1, before2, after1, after2)
        {
            case (-1, _, -1, -1):
                AddBefore(before1, before2, Branch.Before);
                break;
            case (_, -1, -1, -1):
                AddAfter(before2, before1, Branch.Before);
                break;
            case (-1, -1, _, -1):
                AddBefore(after1, after2, Branch.After);
                break;
            case (_, _, -1, -1):
            case (-1, -1, _, _):
                Console.WriteLine("Both pages already processed");
                break;
            default:
                Console.WriteLine($"No process set to deal with [{String.Join(", ", rule)} ]");
                break;
        }
    }

    public void AddAfter(int insertingPage, int existingPage, Branch branch)
    {
        int index = branch is Branch.Before ? Before.IndexOf(existingPage) : After.IndexOf(existingPage);
        if (branch is Branch.Before)
        {
            if (index == Before.Count - 1)
                Before.Add(insertingPage);

            Before.Insert(index + 1, insertingPage);
        }

        if (index == After.Count - 1)
            After.Add(insertingPage);

        After.Insert(index + 1, insertingPage);
    }

    public void AddBefore(int insertingPage, int existingPage, Branch branch)
    {
        int index = branch is Branch.Before ? Before.IndexOf(existingPage) : After.IndexOf(existingPage);
        if (branch is Branch.Before)
        {
            Before.Insert(index, insertingPage);
        }

        After.Insert(index, insertingPage);
    }

    public int[] GetStoredPages() => stored.Cast<int>().ToArray();
    public int GetTotalStored() => Before.Count + After.Count;
    public bool BothInBefore(int a, int b) => Before.Contains(a) && Before.Contains(b);
    public bool BothInAfter(int a, int b) => After.Contains(a) && After.Contains(b);
    public bool BothInEnd(int a, int b) => After.IndexOf(a) + After.IndexOf(b) > After.Count / 2 && Before.IndexOf(a) + Before.IndexOf(b) > Before.Count / 2;

    public void CorrectSorting(int[] rule)
    {
        if (Before.Exists(x => x == rule[0]) && Before.Exists(x => x == rule[1]))
        {
            if (Before.IndexOf(rule[1]) < Before.IndexOf(rule[0]))
                return;

            Before.Remove(rule[0]);
            int beforeIndex = Before.IndexOf(rule[1]);
            Before.Insert(beforeIndex, rule[0]);
        }

        if (Before.IndexOf(rule[1]) < Before.IndexOf(rule[0]))
            return;

        Before.Remove(rule[0]);
        int index = Before.IndexOf(rule[1]);
        Before.Insert(index, rule[0]);
    }
}

public enum Branch
{
    Before,
    After
}
