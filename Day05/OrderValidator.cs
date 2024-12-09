namespace Day05;

public class OrderValidator(string[] input)
{
    private static OrderedDictionary<int, Page> pageMap = [];
    private static readonly List<int> validMiddlePages = [];
    private static readonly List<int[]> invalidManuals = [];

    public int Process(bool isPartTwo = false)
    {
        int splitIndex = Array.FindIndex(input, s => s == "");
        var rules = input[..splitIndex].Select(s => Array.ConvertAll(s.Split('|'), int.Parse));
        var manuals = input[(splitIndex + 1)..].Select(s => Array.ConvertAll(s.Split(','), int.Parse));

        PopulateBranches(rules);
        ValidateManuals(manuals, isPartTwo);

        if (isPartTwo)
        {
            FixOrder(invalidManuals, rules);
        }

        return validMiddlePages.Sum();
    }

    private static void PopulateBranches(IEnumerable<int[]> rules)
    {
        foreach (var rule in rules)
        {
            if (!pageMap.TryGetValue(rule[0], out var firstPage))
            {
                firstPage = new(rule[0]);
                pageMap[rule[0]] = firstPage;
            }

            if (!pageMap.TryGetValue(rule[1], out var secondPage))
            {
                secondPage = new(rule[1]);
                pageMap[rule[1]] = secondPage;
            }

            Update(firstPage, secondPage);
        }
    }

    static void Update(Page a, Page b)
    {
        a.AddAfter(b.Id);
        b.AddBefore(a.Id);
    }

    private static void ValidateManuals(IEnumerable<int[]> manuals, bool isPartTwo = false)
    {
        foreach (var manual in manuals)
        {
            bool isValid = true;
            int index = 0;
            while (isValid && index < manual.Length)
            {
                Page current = pageMap[manual[index]];

                Span<int> beforeSpan = manual.AsSpan()[..index];
                Span<int> afterSpan = manual.AsSpan()[(index + 1)..];

                if (beforeSpan.ContainsAny(current.After.ToArray()))
                {
                    isValid = false;
                }

                if (afterSpan.ContainsAny(current.Before.ToArray()))
                {
                    isValid = false;
                }

                index++;
            }

            if (!isPartTwo && isValid)
            {
                int middle = manual.Length / 2;
                validMiddlePages.Add(manual[middle]);
            }
            else if (isPartTwo && !isValid)
            {
                invalidManuals.Add(manual);
            }
        }
    }

    private static void FixOrder(List<int[]> invalidManuals, IEnumerable<int[]> rules)
    {
        foreach (var currentList in invalidManuals.Select(arr => arr.ToList()))
        {
            bool changed;

            do
            {
                changed = false;

                foreach (var rule in rules.Where(r => currentList.Contains(r[0]) && currentList.Contains(r[1])))
                {
                    int beforeIndex = currentList.IndexOf(rule[0]);
                    int afterIndex = currentList.IndexOf(rule[1]);

                    if (beforeIndex > afterIndex)
                    {
                        currentList.RemoveAt(afterIndex);

                        if (beforeIndex == currentList.Count)
                        {
                            currentList.Add(rule[1]);
                        }
                        else currentList.Insert(beforeIndex, rule[1]);

                        changed = true;
                    }
                }
            } while (changed);

            int middle = currentList.Count / 2;
            validMiddlePages.Add(currentList[middle]);
        }
    }
}
