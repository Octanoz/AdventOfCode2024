using AdventUtilities;

namespace Day13;

public readonly struct Button(int xMove, int yMove)
{
    private readonly int rowMove = yMove;
    private readonly int colMove = xMove;

    public (int, int) Push(int a, int b) => (a + rowMove, b + colMove);

    public Coord Push(Coord c) => c with { Row = c.Row + rowMove, Col = c.Col + colMove };
    public Coord Undo(Coord c) => c with { Row = c.Row - rowMove, Col = c.Col - colMove };

    public int GetRowMove() => rowMove;
    public int GetColMove() => colMove;
}
