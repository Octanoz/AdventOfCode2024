

using SuperLinq;

namespace Day13;

public class ClawCalibrator(params int[] moves)
{
    private readonly int xMoveA = moves[0];
    private readonly int yMoveA = moves[1];
    private readonly int xMoveB = moves[2];
    private readonly int yMoveB = moves[3];
    private readonly int prizeX = moves[4];
    private readonly int prizeY = moves[5];

    public List<(int, int)> Cartesian()
    {
        var seqA = Enumerable.Range(0, 200).Select(n => (n * xMoveA, n * yMoveA));
        var seqB = Enumerable.Range(0, 200).Select(n => (n * xMoveB, n * yMoveB));

        var validCombos = seqA.Cartesian(seqB)
                              .Where(pair => pair.Item1.Item1 + pair.Item2.Item1 == prizeX
                                          && pair.Item1.Item2 + pair.Item2.Item2 == prizeY);

        var resultA = validCombos.Select(pair => pair.Item1)
                                 .Select(tup => (tup.Item1 / xMoveA + tup.Item2 / yMoveA) / 2);
        var resultB = validCombos.Select(pair => pair.Item2)
                                 .Select(tup => (tup.Item1 / xMoveB + tup.Item2 / yMoveB) / 2);

        return resultA.Zip(resultB, (first, second) => (first, second)).ToList();
    }

    public (bool canReach, int minCost, int pressA, int pressB) Solve(int prizeX, int prizeY)
    {
        int gcdX = GCD(xMoveA, xMoveB);
        int gcdY = GCD(yMoveA, yMoveB);

        if (prizeX % gcdX != 0 || prizeY % gcdY != 0)
        {
            return (false, 0, 0, 0);
        }

        int scaleX = prizeX / gcdX;
        int scaleY = prizeY / gcdY;

        var (aX, bX) = ExtendedGCD(xMoveA / gcdX, xMoveB / gcdX);
        var (aY, bY) = ExtendedGCD(yMoveA / gcdY, yMoveB / gcdY);

        aX *= scaleX;
        bX *= scaleX;
        aY *= scaleY;
        bY *= scaleY;

        var resultX = AdustCoefficients(aX, bX, xMoveA / gcdX, xMoveB / gcdX);
        var resultY = AdustCoefficients(aY, bY, yMoveA / gcdY, yMoveB / gcdY);

        if (!resultX.isValid || !resultY.isValid)
            return (false, 0, 0, 0);

        int totalPressA = resultX.pressA + resultY.pressA;
        int totalPressB = resultX.pressB + resultY.pressB;
        int costA = totalPressA * 3;
        int costB = totalPressB;

        return (true, costA + costB, totalPressA, totalPressB);
    }

    private static (bool isValid, int pressA, int pressB) AdustCoefficients(int a, int b, int moveA, int moveB)
    {
        //In case of negatives
        int tMinA = (int)Math.Ceiling(-1.0 * a / moveB);
        int tMinB = (int)Math.Ceiling(-1.0 * b / moveA);
        int tMin = int.Max(tMinA, tMinB);

        int tMaxA = (int)Math.Ceiling(1.0 * b / moveA);
        int tMaxB = (int)Math.Ceiling(1.0 * a / moveB);
        int tMax = int.Min(tMaxA, tMaxB);

        if (tMin > tMax)
        {
            return (false, 0, 0);
        }

        int t = tMin;
        int adjustedA = a + t * moveB;
        int adjustedB = b - t * moveA;

        return (adjustedA >= 0 && adjustedB >= 0, adjustedA, adjustedB);
    }

    private int GCD(int a, int b)
    {
        while (b is not 0)
        {
            (a, b) = (b, a % b);
        }

        return int.Abs(a);
    }
    private static (int coefficientA, int coefficientB) ExtendedGCD(int a, int b)
    {
        int oldR = a, r = b;
        int oldS = 1, s = 0;
        int oldT = 0, t = 1;

        while (r is not 0)
        {
            int quotient = oldR / r;
            (oldR, r) = (r, oldR - quotient * r);
            (oldS, s) = (s, oldS - quotient * s);
            (oldT, t) = (t, oldT - quotient * t);
        }

        return (oldS, oldT);
    }
}

