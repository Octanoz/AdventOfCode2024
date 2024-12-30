using AdventUtilities;

namespace Day16;

public record Reindeer(Coord Pos, Direction Dir, HashSet<Coord> Visited, int Points = 0);
