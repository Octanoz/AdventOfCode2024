namespace Day22;

public static class Negotiate
{
    public static long PartOne(string[] input)
    {
        int[] secretNumbers = Array.ConvertAll(input, int.Parse);
        long result = 0;
        foreach (var secret in secretNumbers)
        {
            result += NumberCooking.ProcessSecretTimes(secret, 2000);
        }

        return result;
    }
}
