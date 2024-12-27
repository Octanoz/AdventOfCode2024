using AdventUtilities;

namespace Day15;

public class WideBox(Coord leftWing)
{
    private Coord lastPosition = leftWing;
    public Coord LeftWing { get; private set; } = leftWing;
    public Coord RightWing => new(LeftWing.Row, LeftWing.Col + 1);

    public void Move(Coord move, Direction dir, char[,] map, List<WideBox> boxes)
    {
        if (dir is Direction.Up or Direction.Down)
        {
            Coord leftMove = dir is Direction.Up ? LeftWing.Up : LeftWing.Down;
            Coord rightMove = leftMove with { Col = leftMove.Col + 1 };
            char leftDest = map[leftMove.Row, leftMove.Col];
            char rightDest = map[rightMove.Row, rightMove.Col];

            switch ((leftDest, rightDest))
            {
                case ('#', _):
                case (_, '#'):
                    return;

                case ('[', ']'):
                    WideBox singleNeighbour = boxes.Find(box => box.Occupies(leftMove))!;
                    singleNeighbour.Move(move, dir, map, boxes);
                    if (singleNeighbour.Occupies(leftMove))
                        return;
                    break;

                case (']', '['):
                    WideBox firstNeighbour = boxes.Find(box => box.Occupies(leftMove))!;
                    WideBox secondNeighbour = boxes.Find(box => box.Occupies(rightMove))!;

                    firstNeighbour.Move(move, dir, map, boxes);
                    if (firstNeighbour.Occupies(leftMove))
                        return;

                    secondNeighbour.Move(move, dir, map, boxes);
                    if (secondNeighbour.Occupies(rightMove))
                    {
                        firstNeighbour.Reset();
                        return;
                    }
                    break;

                case (']', _):
                    WideBox leftNeighbour = boxes.Find(box => box.Occupies(leftMove))!;
                    leftNeighbour.Move(move, dir, map, boxes);
                    if (leftNeighbour.Occupies(leftMove))
                        return;
                    break;

                case (_, '['):
                    WideBox rightNeighbour = boxes.Find(box => box.Occupies(rightMove))!;
                    rightNeighbour.Move(move, dir, map, boxes);
                    if (rightNeighbour.Occupies(rightMove))
                        return;
                    break;

                default: break;
            }
        }
        else
        {
            WideBox? nbBox = FindRowNeighbour(move, boxes, out Coord nbCoord);
            if (map[nbCoord.Row, nbCoord.Col] is '#')
                return;

            if (nbBox is not null)
            {
                nbBox.Move(move, dir, map, boxes);
                if (nbBox.Occupies(nbCoord))
                    return;
            }
        }

        lastPosition = LeftWing;
        LeftWing += move;
    }

    private WideBox? FindRowNeighbour(Coord move, List<WideBox> boxes, out Coord nbCoord)
    {
        nbCoord = move.Col is 1 ? RightWing + move : LeftWing + move;
        var target = nbCoord;

        return boxes.Find(box => box.Occupies(target));
    }

    public void Reset() => LeftWing = lastPosition;

    public bool Occupies(Coord coord) => coord == LeftWing || coord == RightWing;
}
