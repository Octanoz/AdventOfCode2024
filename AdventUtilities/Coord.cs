namespace AdventUtilities;

public record Coord(int Row, int Col)
{
    public static Coord Zero => new(0, 0);
    public static Coord Invalid => new(-1, -1);

    public Coord Up => new(Row - 1, Col);
    public Coord Right => new(Row, Col + 1);
    public Coord Down => new(Row + 1, Col);
    public Coord Left => new(Row, Col - 1);

    public IEnumerable<Coord> Neighbours => [Up, Right, Down, Left];

    public static Coord operator +(Coord a, Coord b) => new(a.Row + b.Row, a.Col + b.Col);
    public static Coord operator -(Coord a, Coord b) => new(a.Row - b.Row, a.Col - b.Col);

    public int Manhattan(Coord other) => Math.Abs(Row - other.Row) + Math.Abs(Col - other.Col);

    public override string ToString() => $"[{Row}, {Col}]";
}
