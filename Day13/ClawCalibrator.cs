

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
        var seqA = Enumerable.Range(0, 100).Select(n => (n * xMoveA, n * yMoveA));
        var seqB = Enumerable.Range(0, 100).Select(n => (n * xMoveB, n * yMoveB));

        var validCombos = seqA.Cartesian(seqB)
                              .Where(pair => pair.Item1.Item1 + pair.Item2.Item1 == prizeX
                                          && pair.Item1.Item2 + pair.Item2.Item2 == prizeY);

        var resultA = validCombos.Select(pair => (pair.Item1.Item1 / xMoveA, pair.Item1.Item2 / yMoveA))
                                 .Where(tup => tup.Item1 == tup.Item2)
                                 .Select(tup => tup.Item1);

        var resultB = validCombos.Select(pair => (pair.Item2.Item1 / xMoveB, pair.Item2.Item2 / yMoveB))
                                 .Where(tup => tup.Item1 == tup.Item2)
                                 .Select(tup => tup.Item1);

        return resultA.Zip(resultB, (first, second) => (first, second)).ToList();
    }
}

