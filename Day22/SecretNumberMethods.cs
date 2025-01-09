namespace Day22;

public static class NumberCooking
{
    public static long Mix(long newValue, long secretNumber) => newValue ^ secretNumber;

    public static long Prune(long secretNumber) => secretNumber % 16777216;
}
