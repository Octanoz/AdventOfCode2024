namespace Day19;

public static class Onsen
{
    public static int PartOne(string filePath)
    {
        var (designsList, requestsList) = TowelParser.ParseTowels(filePath);

        TowelMaster tm = new([.. designsList]);

        return tm.FindValidRequests(requestsList);
    }

    public static long PartTwo(string filePath)
    {
        var (designsList, requestsList) = TowelParser.ParseTowels(filePath);
        TowelMaster tm = new([.. designsList]);

        return tm.FindAllCombinations(requestsList);
    }

}
