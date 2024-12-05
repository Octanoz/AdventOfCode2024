namespace AdventUtilities.DOF8;

public record Coord(int Row, int Col)
{
    public static Coord Zero => new(0, 0);

    public Coord UpLeft => new(Row - 1, Col - 1);
    public Coord Up => new(Row - 1, Col);
    public Coord UpRight => new(Row - 1, Col + 1);
    public Coord Left => new(Row, Col - 1);
    public Coord Right => new(Row, Col + 1);
    public Coord DownLeft => new(Row + 1, Col - 1);
    public Coord Down => new(Row + 1, Col);
    public Coord DownRight => new(Row + 1, Col + 1);

    public IEnumerable<Coord> Neighbours => [UpLeft, Up, UpRight, Left, Right, DownLeft, Down, DownRight];

    public static Coord operator +(Coord a, Coord b) => new(a.Row + b.Row, a.Col + b.Col);
    public static Coord operator -(Coord a, Coord b) => new(a.Row - b.Row, a.Col - b.Col);

    public int Manhattan(Coord other) => Math.Abs(Row - other.Row) + Math.Abs(Col - other.Col);

    public override string ToString() => $"[{Row}, {Col}]";
}
