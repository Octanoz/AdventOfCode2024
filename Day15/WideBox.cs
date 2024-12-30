using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class WideBox(Coord leftWing, bool IsLocked = false)
{
    private Coord lastPosition = leftWing;
    public Coord LeftWing { get; private set; } = leftWing;
    public Coord RightWing => new(LeftWing.Row, LeftWing.Col + 1);

    public bool Moved { get; set; } = false;
    public bool IsLocked { get; private set; } = IsLocked;

    public void Move(Coord move, Direction dir, Span2D<char> mapSpan, List<WideBox> boxes)
    {
        if (IsLocked)
            return;

        if (dir is Direction.Up or Direction.Down)
        {
            Coord leftMove = dir is Direction.Up ? LeftWing.Up : LeftWing.Down;
            Coord rightMove = leftMove with { Col = leftMove.Col + 1 };
            char leftDest = mapSpan.GetValueAt(leftMove);
            char rightDest = mapSpan.GetValueAt(rightMove);

            switch ((leftDest, rightDest))
            {
                case ('#', _):
                case (_, '#'):
                    CheckLockedPosition(mapSpan, boxes);
                    return;

                case ('[', ']'):
                    WideBox singleNeighbour = boxes.Find(box => box.Occupies(leftMove))
                    ?? throw new NullReferenceException($"Couldn't find the singleNeighbour box at {LeftWing + move}");
                    singleNeighbour.Move(move, dir, mapSpan, boxes);
                    if (singleNeighbour.Occupies(leftMove))
                    {
                        CheckLockedPosition(mapSpan, boxes);
                        return;
                    }
                    break;

                case (']', '['):
                    WideBox? firstNeighbour = FindSharedNeighbour(leftMove, move, boxes, "first");
                    WideBox? secondNeighbour = FindSharedNeighbour(rightMove, move, boxes, "right");

                    firstNeighbour?.Move(move, dir, mapSpan, boxes);
                    if (firstNeighbour?.Occupies(leftMove) ?? false)
                    {
                        CheckLockedPosition(mapSpan, boxes);
                        return;
                    }

                    secondNeighbour?.Move(move, dir, mapSpan, boxes);
                    if (secondNeighbour?.Occupies(rightMove) ?? false)
                    {
                        firstNeighbour.Reset();
                        return;
                    }
                    break;

                case (']', _):
                    WideBox? leftNeighbour = FindSharedNeighbour(leftMove, move, boxes, "left");
                    leftNeighbour?.Move(move, dir, mapSpan, boxes);
                    if (leftNeighbour?.Occupies(leftMove) ?? false)
                        return;
                    break;

                case (_, '['):
                    WideBox? rightNeighbour = FindSharedNeighbour(rightMove, move, boxes, "right");
                    rightNeighbour?.Move(move, dir, mapSpan, boxes);
                    if (rightNeighbour?.Occupies(rightMove) ?? false)
                        return;
                    break;

                default: break;
            }
        }
        else
        {
            WideBox? nbBox = FindRowNeighbour(move, boxes, out Coord nbCoord);
            char targetCell = mapSpan.GetValueAt(nbCoord);
            if (targetCell is '#')
                return;

            if (nbBox is not null)
            {
                if (dir is Direction.Right && targetCell is ']')
                {
                    mapSpan.Draw2DGridTight();
                    throw new InvalidDataException($"The target box has its right side next to this box's right side");
                }

                if (dir is Direction.Left && targetCell is '[')
                {
                    mapSpan.Draw2DGridTight();
                    throw new InvalidDataException($"The target box has its left side next to this box's left side");
                }

                nbBox.Move(move, dir, mapSpan, boxes);
                if (nbBox.Occupies(nbCoord))
                {
                    CheckLockedPosition(mapSpan, boxes);
                    return;
                }
            }
        }

        UpdatePosition(LeftWing + move);
    }

    private void UpdatePosition(Coord newCoord)
    {
        lastPosition = LeftWing;
        LeftWing = newCoord;
        Moved = true;
    }

    private WideBox? FindSharedNeighbour(Coord target, Coord move, List<WideBox> boxes, string identifier)
    {
        WideBox? neighbour = boxes.Find(box => box.Occupies(target));
        if (neighbour is null)
        {
            if (!boxes.Exists(box => box.Occupies(target + move) && box.Moved))
            {
                throw new NullReferenceException($"Couldn't find {identifier}neighbour at {target} or {target + move}");
            }

            return null;
        }

        return neighbour;
    }

    private WideBox? FindRowNeighbour(Coord move, List<WideBox> boxes, out Coord nbCoord)
    {
        nbCoord = move.Col is 1 ? RightWing + move : LeftWing + move;
        var target = nbCoord;

        return boxes.Find(box => box.Occupies(target));
    }

    public void Reset()
    {
        LeftWing = lastPosition;
        ResetMovedState();
    }
    public void ResetMovedState() => Moved = false;
    public Coord[] GetMapData(bool isReset = false)
    {
        var previousLeft = lastPosition;
        var previousRight = lastPosition with { Col = previousLeft.Col + 1 };
        var currentLeft = LeftWing;
        var currentRight = LeftWing with { Col = currentLeft.Col + 1 };

        Coord[] result = [previousLeft, previousRight, currentLeft, currentRight];
        if (isReset)
        {
            (result[0], result[1], result[2], result[3]) = (result[2], result[3], result[0], result[1]);
        }

        return result;
    }

    private void SetLock() => IsLocked = true;

    public void CheckLockedPosition(Span2D<char> mapSpan, List<WideBox> boxes)
    {
        if (IsLockedPosition(mapSpan, boxes))
            SetLock();
    }

    private bool IsLockedPosition(Span2D<char> mapSpan, List<WideBox> boxes)
    {
        if (!IsLockedOrWall(mapSpan, LeftWing.Left, boxes) && !IsLockedOrWall(mapSpan, RightWing.Right, boxes))
        {
            return false;
        }

        return IsBlocked(mapSpan, boxes);
    }

    private bool IsBlocked(Span2D<char> mapSpan, List<WideBox> boxes)
    {
        bool hasHorizontalBlock = false;
        bool hasVerticalBlock = false;
        List<Coord> neighbourCoords = [LeftWing.Left, LeftWing.Up, RightWing.Up, RightWing.Right, RightWing.Down, LeftWing.Down];

        int index = 0;
        while (!hasHorizontalBlock && !hasVerticalBlock)
        {
            Coord current = neighbourCoords[index];
            if (IsLockedOrWall(mapSpan, current, boxes))
            {
                if (current == LeftWing.Left || current == RightWing.Right)
                {
                    hasHorizontalBlock = true;
                }
                else
                {
                    hasVerticalBlock = true;
                }
            }

            if (++index >= neighbourCoords.Count)
                break;
        }

        return hasHorizontalBlock && hasVerticalBlock;
    }

    private bool HasLockedNeighbour(List<WideBox> boxes)
        => boxes.Exists(box => (LeftWing.Neighbours.Any(box.Occupies)
                                || RightWing.Neighbours.Any(box.Occupies))
                                && box.IsLocked);
    private bool IsLockedNeighbour(Coord nb, List<WideBox> boxes) => boxes.Exists(box => box.Occupies(nb) && box.IsLocked);
    private bool IsLockedOrWall(Span2D<char> mapSpan, Coord nb, List<WideBox> boxes) => mapSpan.GetValueAt(nb) is '#' || IsLockedNeighbour(nb, boxes);
    public int GetLeftLastRow() => lastPosition.Row;
    public int GetLeftLastCol() => lastPosition.Col;
    public Coord GetLeftLastPosition() => lastPosition;

    public bool Occupies(Coord coord) => coord == LeftWing || coord == RightWing;
}
