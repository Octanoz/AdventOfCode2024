namespace Day22;

public static class NumberCooking
{
    public static long ProcessSecretTimes(long secretNumber, int times)
    {
        for (int i = 0; i < times; i++)
        {
            secretNumber = ProcessSecretNumber(secretNumber);
        }

        return secretNumber;
    }
    public static long ProcessSecretNumber(long secretNumber)
    {
        long multiplication = secretNumber * 64;
        long mixedMultiplication = Mix(multiplication, secretNumber);
        secretNumber = Prune(mixedMultiplication);

        long division = (long)Math.Floor(secretNumber / 32.0);
        long mixedDivision = Mix(division, secretNumber);
        secretNumber = Prune(mixedDivision);

        long multiplyTwoKilobytes = secretNumber * 2048;
        long mixedTwoKBMultiplication = Mix(multiplyTwoKilobytes, secretNumber);
        secretNumber = Prune(mixedTwoKBMultiplication);

        return secretNumber;
    }

    private static long Mix(long newValue, long secretNumber) => newValue ^ secretNumber;

    private static long Prune(long secretNumber) => secretNumber % 16777216;
}
