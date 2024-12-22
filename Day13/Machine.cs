
using AdventUtilities;
using SuperLinq;

namespace Day13;

public class Machine(params int[] moves)
{
    private readonly Button buttonA = new(moves[0], moves[1]);
    private readonly Button buttonB = new(moves[2], moves[3]);
    private int totalCost = 0;

    public Coord PrizeLocation { get; } = new(moves[5], moves[4]);
    public Coord ClawLocation { get; private set; } = Coord.Zero;

    public void PressA() => ClawLocation = PushButton('A', ClawLocation);
    public void UndoA() => ClawLocation = UndoButton('A', ClawLocation);
    public void PressB() => ClawLocation = PushButton('B', ClawLocation);
    public void UndoB() => ClawLocation = UndoButton('B', ClawLocation);

    private Coord PushButton(char id, Coord coord)
    {
        Pay(id);

        if (id is 'A')
        {
            return PressButtonA(coord);
        }
        else
        {
            return PressButtonB(coord);
        }
    }

    private Coord UndoButton(char id, Coord coord)
    {
        UndoPay(id);

        if (id is 'A')
        {
            return UndoButtonA(coord);
        }
        else
        {
            return UndoButtonB(coord);
        }
    }

    public List<(int, int)> FindCombos()
    {
        int colMoveA = buttonA.GetColMove();
        int rowMoveA = buttonA.GetRowMove();

        int colMoveB = buttonB.GetColMove();
        int rowMoveB = buttonB.GetRowMove();

        var seqA = Enumerable.Range(0, 101).Select(n => (n * rowMoveA, n * colMoveA));
        var seqB = Enumerable.Range(0, 101).Select(n => (n * rowMoveB, n * colMoveB));

        var validCombos = seqA.Cartesian(seqB)
                              .Where(pair => pair.Item1.Item1 + pair.Item2.Item1 == PrizeLocation.Row && pair.Item1.Item2 + pair.Item2.Item2 == PrizeLocation.Col);

        var resultA = validCombos.Select(pair => pair.Item1).Select(tup => (tup.Item1 / rowMoveA + tup.Item2 / colMoveA) / 2);
        var resultB = validCombos.Select(pair => pair.Item2).Select(tup => (tup.Item1 / rowMoveB + tup.Item2 / colMoveB) / 2);

        return resultA.Cartesian(resultB).ToList();
    }

    private void Pay(char id) => totalCost += id is 'A' ? 3 : 1;
    private void UndoPay(char id) => totalCost -= id is 'A' ? 3 : 1;

    private (int, int) PressButtonA(int x, int y) => buttonA.Push(x, y);
    private Coord PressButtonA(Coord c) => buttonA.Push(c);
    private Coord UndoButtonA(Coord coord) => buttonA.Undo(coord);

    private (int, int) PressButtonB(int x, int y) => buttonB.Push(x, y);
    private Coord PressButtonB(Coord c) => buttonB.Push(c);
    private Coord UndoButtonB(Coord coord) => buttonB.Undo(coord);

    public int GetTotalCost() => totalCost;
}
