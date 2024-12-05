namespace AdventUtilities;

public static class InputData
{
    public static string GetSolutionDirectory()
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        while (directory is not null && !directory.EnumerateFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }

        return directory?.FullName
        ?? throw new InvalidOperationException("Solution root not found");
    }
}
