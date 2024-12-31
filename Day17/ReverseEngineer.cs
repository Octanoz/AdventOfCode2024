namespace Day17;

using System.Text;

public class ReverseEngineer(List<int> stored)
{
    private readonly List<int> stored = stored;
    private readonly List<int> output = [];
    private ulong lowestMatch = ulong.MaxValue;

    public string Setup()
    {
        Span<char> targetOutput = String.Join("", stored).ToArray();
        Recursed(0UL, stored.Count - 1, targetOutput);

        return lowestMatch.ToString();
    }

    private void Recursed(ulong currentA, int index, Span<char> targetOutput)
    {
        if (index < 0)
        {
            lowestMatch = ulong.Min(lowestMatch, currentA);
            return;
        }

        int next = stored[index];
        for (int remainder = 0; remainder < 8; remainder++)
        {
            ulong nextA = currentA * 8 + (ulong)remainder;
            string result = OtherRun(nextA);
            if (targetOutput.EndsWith(result))
            {
                Recursed(nextA, index - 1, targetOutput);
            }
        }

    }

    public string OtherRun(ulong regA)
    {
        ulong regB = 0, regC = 0;

        do
        {
            regB = regA % 8;
            regB ^= 5;
            regC = regA / (ulong)Math.Pow(2, regB);
            regB ^= 6;
            regB ^= regC;
            output.Add((int)(regB % 8));
            regA /= 8;
        } while (regA != 0);

        string result = String.Join("", output);
        output.Clear();

        return result;
    }
}
