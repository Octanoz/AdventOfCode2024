namespace Day05;

public struct PageNode(int id)
{
    public int Id { get; private set; } = id;
    public List<PageNode> Before { get; private set; } = [];
    public List<PageNode> After { get; private set; } = [];

    public void AddBefore(PageNode other) => Before.Add(other);
    public void AddAfter(PageNode other) => After.Add(other);

    /* public bool Find(int id, Collect collect)
    {
        if (collect is Collect.Before)
        {
            if (Before.Exists(pn => pn.Id == id))
            {
                return true;
            }

            foreach (var node in Before)
            {
                if (node.Find(id, collect))
                    return true;
            }

            return false;
        }

        return false;
    } */
}

public enum Collect
{
    Before,
    After
}
