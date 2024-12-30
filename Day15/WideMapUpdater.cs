using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15;

public class WideMapUpdater(List<WideBox> boxes)
{
    internal void UpdateMap(Span2D<char> mapSpan, Coord lastPosition, Coord position, bool boxMoved)
    {
        if (boxMoved)
        {
            WideWipe(mapSpan, out var movedBoxes);
            Repopulate(mapSpan, movedBoxes);
        }

        mapSpan.SetCharAt('.', lastPosition);
        mapSpan.SetCharAt('@', position);
    }

    public void WideBoxMoveUpdate(Span2D<char> mapSpan, WideBox box, params Coord[] coords)
    {
        var (lastLeft, lastRight, currentLeft, currentRight) =
            (coords[0], coords[1], coords[2], coords[3]);

        mapSpan.SetCharAt('.', lastLeft);
        mapSpan.SetCharAt('.', lastRight);
        mapSpan.SetCharAt('[', currentLeft);
        mapSpan.SetCharAt(']', currentRight);

        box.Reset();
    }

    private void WideWipe(Span2D<char> mapSpan, out List<WideBox> movedBoxes)
    {
        movedBoxes = boxes.Where(box => box.Moved).ToList();

        if (movedBoxes.Count is 1)
        {
            RemoveSingle(mapSpan, movedBoxes[0]);
            return;
        }

        var (topRow, leftCol, rightCol, bottomRow) = GetArea(movedBoxes);

        var mapSlice = mapSpan.Slice(topRow, leftCol, int.Max(1, bottomRow - topRow), int.Max(1, rightCol - leftCol));

        //Boxes that didn't move but got removed while wiping the area of the ones that did
        List<WideBox> collateralBoxes = GetCollateralBoxes(movedBoxes, topRow, leftCol, rightCol, bottomRow);
        movedBoxes = [.. movedBoxes, .. collateralBoxes];
        int index = 0;
        do
        {
            var rowSpan = mapSlice.GetRowSpan(index);
            rowSpan.Replace('[', '.');
            rowSpan.Replace(']', '.');
            index++;
        } while (index < mapSlice.Height);
    }

    private void RemoveSingle(Span2D<char> mapSpan, WideBox box)
    {
        var previousLeft = box.GetLeftLastPosition();
        var previousRight = previousLeft with { Col = previousLeft.Col + 1 };

        mapSpan.SetCharAt('.', previousLeft);
        mapSpan.SetCharAt('.', previousRight);
    }

    private List<WideBox> GetCollateralBoxes(List<WideBox> movedBoxes, params int[] dimensions)
    {
        var (topRow, leftCol, rightCol, bottomRow) =
        (dimensions[0], dimensions[1], dimensions[2], dimensions[3]);

        return boxes.Except(movedBoxes)
                    .Where(box => box.LeftWing.Row >= topRow && box.LeftWing.Row <= bottomRow
                       && (box.RightWing.Col >= leftCol || box.LeftWing.Col <= rightCol)).ToList();
    }

    private static void Repopulate(Span2D<char> mapSpan, List<WideBox> removedBoxes)
    {
        foreach (var wideBox in removedBoxes)
        {
            mapSpan.SetCharAt('[', wideBox.LeftWing);
            mapSpan.SetCharAt(']', wideBox.RightWing);
            wideBox.ResetMovedState();
        }
    }

    private (int, int, int, int) GetArea(List<WideBox> movedBoxes)
    {
        int topRow = movedBoxes.Select(box => int.Min(box.LeftWing.Row, box.GetLeftLastRow())).Min();
        int leftCol = movedBoxes.Select(box => int.Min(box.LeftWing.Col, box.GetLeftLastCol())).Min();
        int rightCol = movedBoxes.Select(box => int.Max(box.RightWing.Col + 1, box.GetLeftLastCol() + 1)).Max();
        int bottomRow = movedBoxes.Select(box => int.Max(box.LeftWing.Row + 1, box.GetLeftLastRow() + 1)).Max();

        return (topRow, leftCol, rightCol, bottomRow);
    }
}
