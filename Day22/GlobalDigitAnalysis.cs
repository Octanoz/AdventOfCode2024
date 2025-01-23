namespace Day22;

using AdventUtilities;

public class GlobalDigitAnalysis
{
    private readonly Dictionary<int, int> digitFrequencies = [];

    public void AnalyzeSeller(long initialSecret, int iterations = 2000)
    {
        long secret = initialSecret;
        for (int i = 0; i < iterations; i++)
        {
            secret = NumberCooking.ProcessSecretNumber(secret);
            int firstDigit = Math.Abs((int)(secret % 10)); // Extract first digit
            digitFrequencies[firstDigit] = digitFrequencies.TryGetValue(firstDigit, out int freq) ? ++freq : 1;
        }
    }

    public void PrintResults()
    {
        using StreamWriter sw = new(Path.Combine(InputData.GetSolutionDirectory(), "Day22/Notes/Frequencies.txt"), true);
        foreach (var kvp in digitFrequencies)
        {
            sw.WriteLine($"Digit {kvp.Key}: {kvp.Value} occurrences");
            Console.WriteLine($"Digit {kvp.Key}: {kvp.Value} occurrences");
        }
    }
}
