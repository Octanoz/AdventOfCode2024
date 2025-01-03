namespace Day19;

using System;

public class TowelMaster(string[] designs)
{
    public string[] Designs { get; } = designs;

    public int FindValidRequests(List<string> requests)
    {
        int valid = 0;
        foreach (var request in requests)
        {
            if (CanCreate(request))
            {
                valid++;
            }
        }

        return valid;
    }

    internal long FindAllCombinations(List<string> requestsList)
    {
        long possibleCombos = 0;

        Dictionary<string, long> comboCache = [];

        foreach (var request in requestsList)
        {
            possibleCombos += CountCombos(request, [request]);
        }

        return possibleCombos;

        long CountCombos(string request, params Span<string> snippets)
        {
            if (request.Length is 0)
            {
                return 1;
            }

            if (comboCache.TryGetValue(request, out long cachedResult))
            {
                return cachedResult;
            }

            long combinations = 0;
            foreach (var design in Designs.Where(des => request.StartsWith(des)).OrderByDescending(des => des.Length))
            {
                combinations += CountCombos(request[design.Length..]);
            }

            comboCache[request] = combinations;
            return combinations;
        }
    }

    private bool CanCreate(string request)
    {
        if (request.Length is 0)
            return true;

        foreach (var design in Designs.Where(des => request.StartsWith(des)).OrderByDescending(des => des.Length))
        {
            int length = design.Length;
            if (CanCreate(request[length..]))
            {
                return true;
            }
        }

        return false;
    }
}
