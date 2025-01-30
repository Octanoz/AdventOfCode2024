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
        long multiplication = secretNumber << 6;
        secretNumber = Prune(Mix(multiplication, secretNumber));

        long division = secretNumber >> 5;
        secretNumber = Prune(Mix(division, secretNumber));

        long multiplyTwoKilobytes = secretNumber << 11;
        secretNumber = Prune(Mix(multiplyTwoKilobytes, secretNumber));

        return secretNumber;
    }

    private static long Mix(long newValue, long secretNumber) => newValue ^ secretNumber;

    private static long Prune(long secretNumber) => secretNumber % 16777216;
}
