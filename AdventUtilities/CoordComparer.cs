namespace AdventUtilities;

using System.Diagnostics.CodeAnalysis;

public class CoordComparer : IEqualityComparer<Coord>
{
    public bool Equals(Coord? x, Coord? y) => x?.Row == y?.Row && x?.Col == y?.Col;
    public int GetHashCode([DisallowNull] Coord obj) => HashCode.Combine(obj.Row, obj.Col);
}
