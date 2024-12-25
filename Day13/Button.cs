using AdventUtilities.LongCoordinates;

namespace Day13;

public readonly struct Button(CoordL Move)
{
    public CoordL Push(CoordL c) => c + Move;
    public CoordL Undo(CoordL c) => c + Move;

    public long GetRowMove() => Move.Row;
    public long GetColMove() => Move.Col;
}
