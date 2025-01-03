namespace Day19;

public static class TowelParser
{
    public static (List<string> designs, List<string> requests) ParseTowels(string filePath)
    {
        List<string> designs = [], requests = [];

        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                designs.AddRange(line.Split(", "));
                line = sr.ReadLine();
            }

            line = sr.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                requests.Add(line);
                line = sr.ReadLine();
            }
        }

        return (designs, requests);
    }
}
