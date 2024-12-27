using AdventUtilities;

namespace Day15;

public class Box(Coord position)
{
    public Coord Position { get; set; } = position;

    public bool IsBlocked(Coord move, Direction dir, char[,] map, List<Box> boxes)
    {
        Box? nbBox = FindNeighbor(move, boxes, out Coord nbCoord);

        if (nbBox is null)
        {
            if (map[nbCoord.Row, nbCoord.Col] is '#')
            {
                return true;
            }

            return false;
        }

        return nbBox.IsBlocked(move, dir, map, boxes);
    }

    public void Move(Coord move, Direction dir, char[,] map, List<Box> boxes)
    {
        if (!IsBlocked(move, dir, map, boxes))
        {
            Box? nbBox = FindNeighbor(move, boxes, out Coord nbCoord);

            if (nbBox is not null)
            {
                nbBox.Move(move, dir, map, boxes);

                if (nbBox.Position == nbCoord)
                {
                    return;
                }
            }

            Position = nbCoord;
        }
    }

    private Box? FindNeighbor(Coord move, List<Box> boxes, out Coord nbCoord)
    {
        nbCoord = Position + move;
        var target = nbCoord;
        return boxes.Find(box => box.Position == target);
    }
}
