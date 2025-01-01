namespace AdventUtilities;

public record CoordXY(int X, int Y)
{
    public static CoordXY Zero => new(0, 0);
    public static CoordXY Invalid => new(-10, -10);

    public CoordXY Up => new(X, Y - 1);
    public CoordXY Right => new(X + 1, Y);
    public CoordXY Down => new(X, Y + 1);
    public CoordXY Left => new(X - 1, Y);

    public IEnumerable<CoordXY> Neighbours => [Up, Right, Down, Left];

    public static CoordXY operator +(CoordXY a, CoordXY b) => new(a.X + b.X, a.Y + b.Y);
    public static CoordXY operator -(CoordXY a, CoordXY b) => new(a.X - b.X, a.Y - b.Y);

    public int Manhattan(CoordXY other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

    public override string ToString() => $"[{X}, {Y}]";
}