namespace AdventUtilities;

using CommunityToolkit.HighPerformance;

public static class Span2DExtensions
{
    public static void Draw2DGridXY<T>(this Span2D<T> grid)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                Console.Write($"{grid[y, x]} ");
            }

            Console.WriteLine();
        }
    }

    public static void Draw2DGridTightXY<T>(this Span2D<T> grid)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                Console.Write($"{grid[y, x]}");
            }

            Console.WriteLine();
        }
    }

    public static void Draw2DGrid<T>(this Span2D<T> grid)
    {
        for (int i = 0; i < grid.Height; i++)
        {
            foreach (var item in grid.GetRow(i))
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();
        }
    }

    public static void Draw2DGridTight<T>(this Span2D<T> grid)
    {
        for (int i = 0; i < grid.Height; i++)
        {
            foreach (var item in grid.GetRow(i))
            {
                Console.Write($"{item}");
            }

            Console.WriteLine();
        }
    }

    public static void Draw2DGridTightSlow<T>(this Span2D<T> grid)
    {
        for (int i = 0; i < grid.Height; i++)
        {
            foreach (var item in grid.GetRow(i))
            {
                Console.Write(item);
            }

            Console.WriteLine();
            Thread.Sleep(500);
        }
    }

    public static void DrawInt2D(this Span2D<int> grid)
    {
        for (int i = 0; i < grid.Height; i++)
        {
            foreach (var num in grid.GetRow(i))
            {
                if (num is 0)
                {
                    Console.Write("  ");
                    continue;
                }

                Console.Write($"{num} ");
            }

            Console.WriteLine();
        }
    }

    public static void DrawCharGrid(this Span2D<char> grid)
    {
        for (int i = 0; i < grid.Height; i++)
        {
            foreach (var item in grid.GetRow(i))
            {
                if (item == '.')
                {
                    Console.Write(' ');
                    continue;
                }

                Console.Write(item);
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Writes the rows of 2 equally sized Span2D<T> side by side to the console, separated by a space.
    /// </summary>
    /// <param name="first">Span2D to be printed on the left.</param>
    /// <param name="second">Span2D to be printed on the right.</param>
    public static void CompareToOther<T>(this Span2D<T> first, Span2D<T> second)
    {
        if (first.Height != second.Height || first.Width != second.Width)
        {
            Console.WriteLine("Grids are not of equal size, cannot print side-by-side");
            return;
        }

        for (int i = 0; i < first.Height; i++)
        {
            foreach (var item in first.GetRow(i))
            {
                Console.Write($"{item}");
            }

            Console.Write(' ');

            foreach (var item in second.GetRow(i))
            {
                Console.Write($"{item}");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Writes the rows of 2 equally sized Span2D<T> side by side to the console, separated by a chosen number (0 to 10) of spaces.
    /// </summary>
    /// <param name="first">Span2D to be printed on the left.</param>
    /// <param name="second">Span2D to be printed on the right.</param>
    /// <param name="spaces">Number of spaces given as integer.</param>
    public static void CompareToOther<T>(this Span2D<T> first, Span2D<T> second, int spaces)
    {
        if (first.Height != second.Height || first.Width != second.Width)
        {
            Console.WriteLine("Grids are not of equal size, cannot print side-by-side");
            return;
        }

        for (int i = 0; i < first.Height; i++)
        {
            foreach (var item in first.GetRow(i))
            {
                Console.Write($"{item}");
            }


            if (spaces > 0)
            {
                Console.Write($"{Enumerable.Repeat(' ', spaces <= 10 ? spaces : 10)}");
            }

            foreach (var item in second.GetRow(i))
            {
                Console.Write($"{item}");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Writes the rows of 2 equally sized Span2D<T> side by side to the console, separated by a custom string of up to 10 chars.
    /// Tabs can be inserted using &quot;separationTab&quot; for 2 or 3 add the number to the string.
    /// Alternatively, you can use &quot;<tab>&quot; and repeat tab up to 3 times within the chevrons.
    /// &quot;None&quot; is also an option but prefer to use 0 which will skip the write action.
    /// </summary>
    /// <param name="first">Span2D to be printed on the left.</param>
    /// <param name="second">Span2D to be printed on the right.</param>
    /// <param name="separationString">A string of up to 10 chars to seaprate the two rows. &quot;SeparationTab&quot; and &quot;<TabTab>&quot; supported.</param>
    public static void CompareToOther<T>(this Span2D<T> first, Span2D<T> second, string separationString)
    {
        var separator = separationString.ToLower() switch
        {
            "separationtab" => "\t",
            "separationtab2" => "\t\t",
            "separationtab3" => "\t\t\t",
            "<tab>" => "\t",
            "<tabtab>" => "\t\t",
            "<tabtabtab>" => "\t\t\t",
            "none" => "",
            var s when s.Length <= 10 => separationString,
            _ => " "
        };

        if (first.Height != second.Height || first.Width != second.Width)
        {
            Console.WriteLine("Grids are not of equal size, cannot print side-by-side");
            return;
        }

        for (int i = 0; i < first.Height; i++)
        {
            foreach (var item in first.GetRow(i))
            {
                Console.Write($"{item}");
            }

            Console.Write(separator);

            foreach (var item in second.GetRow(i))
            {
                Console.Write($"{item}");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Set a value at the specified coordinates if the Span2D supports the Type used.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="value">The Span2D Type value.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    public static void SetAt<T>(this ref Span2D<T> gridSpan, T value, Coord coord) => gridSpan[coord.Row, coord.Col] = value;

    /// <summary>
    /// Set a char value at the specified coordinates in the Span2D.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="letter">The specified char value.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    public static void SetCharAt(this ref Span2D<char> gridSpan, char letter, Coord coord)
        => gridSpan[coord.Row, coord.Col] = letter;

    /// <summary>
    /// Set a integer value at the specified coordinates in the Span2D.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="number">The specified integer</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    public static void SetIntAt(this ref Span2D<int> gridSpan, int number, Coord coord)
        => gridSpan[coord.Row, coord.Col] = number;

    public static void SetAt<T>(this ref Span2D<T> gridSpan, T value, CoordXY coord) => gridSpan[coord.Y, coord.X] = value;

    public static void SetCharAt(this ref Span2D<char> gridSpan, char letter, CoordXY coord)
        => gridSpan[coord.Y, coord.X] = letter;

    public static void SetIntAt(this ref Span2D<int> gridSpan, int number, CoordXY coord)
        => gridSpan[coord.Y, coord.X] = number;

    /// <summary>
    /// Wipes characters on a given row after the specified column
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    public static void WipeCharAfterCol(this ref Span2D<char> gridSpan, Coord coord)
        => gridSpan.GetRowSpan(coord.Row)[(coord.Col + 1)..].Fill('.');

    /// <summary>
    /// Wipes characters on a given row after the specified column until the specified (1-based) index from the end.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    /// <param name="end">Last index to wipe using implicit indexer access</param>
    public static void WipeCharAfterCol(this ref Span2D<char> gridSpan, Coord coord, int end)
        => gridSpan.GetRowSpan(coord.Row)[(coord.Col + 1)..^end].Fill('.');

    /// <summary>
    /// Wipes characters before the specified column in the given row, starting from the beginning of the row.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    public static void WipeCharBeforeCol(this ref Span2D<char> gridSpan, Coord coord)
        => gridSpan.GetRowSpan(coord.Row)[..coord.Col].Fill('.');

    /// <summary>
    /// Wipes characters before the specified column in the given row, starting from a specified index.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    /// <param name="start">The column index to start wiping from.</param>
    public static void WipeCharBeforeCol(this ref Span2D<char> gridSpan, Coord coord, int start)
        => gridSpan.GetRowSpan(coord.Row)[start..coord.Col].Fill('.');

    /// <summary>
    /// Wipes characters in a given column from the top down to the specified row.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    public static void WipeCharBeforeRow(this ref Span2D<char> gridSpan, Coord coord)
    {
        for (int i = 0; i < coord.Row; i++)
        {
            gridSpan.SetCharAt('.', coord with { Row = i });
        }
    }

    /// <summary>
    /// Wipes characters in a given column from the top down to the specified row. An offset is added to the starting index.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    /// <param name="offset"></param>
    public static void WipeCharBeforeRow(this ref Span2D<char> gridSpan, Coord coord, int offset)
    {
        for (int i = offset; i < coord.Row; i++)
        {
            gridSpan.SetCharAt('.', coord with { Row = i });
        }
    }

    /// <summary>
    /// Wipes characters in a given column from the cell following the specified row down to the end of the grid.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid of 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    public static void WipeCharAfterRow(this ref Span2D<char> gridSpan, Coord coord)
    {
        for (int i = coord.Row + 1; i < gridSpan.Height; i++)
        {
            gridSpan.SetCharAt('.', coord with { Row = i });
        }
    }

    /// <summary>
    /// Wipes the characters in a given column from but not including the specified row down to the specified number of cells counted from the end of the grid (1-based).
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the position.</param>
    /// <param name="end">Integer giving the number of cells before the end of the grid to stop the wipe.</param>
    public static void WipeCharAfterRow(this ref Span2D<char> gridSpan, Coord coord, int end)
    {
        for (int i = coord.Row + 1; i < gridSpan.Height - end; i++)
        {
            gridSpan.SetCharAt('.', coord with { Row = i });
        }
    }


    /// <summary>
    /// Returns the element at the specified coordinates of the current Span2D.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="coord">The Coord class coordinates (Row and Col) of the element.</param>
    /// <returns>Element at [coord.Row, coord.Col]</returns>
    public static T GetValueAt<T>(this Span2D<T> gridSpan, Coord coord) => gridSpan[coord.Row, coord.Col];

    /// <summary>
    /// Returns the element at the specified x,y coordinates of the current Span2D.
    /// </summary>
    /// <param name="gridSpan">The Span2D representing the grid or 2D array.</param>
    /// <param name="x">X-coordinate of the element.</param>
    /// <param name="y">Y-coordinate of the element.</param>
    /// <returns>Element at the Span2D[y,x] position</returns>
    public static T GetValueAtXY<T>(this Span2D<T> gridSpan, int x, int y) => gridSpan[y, x];

    public static List<Coord> StoreWallCoordsInCol(this Span2D<char> gridSpan, int col)
    {
        List<Coord> result = [];
        int index = 0;
        foreach (var letter in gridSpan.GetColumn(col))
        {
            if (letter is '#')
            {
                result.Add(new(index, col));
            }

            index++;
        }

        return result;
    }

    public static List<Coord> StoreWallCoordsInRow(this Span2D<char> gridSpan, int row)
    {
        List<Coord> result = [];
        int index = 0;
        foreach (var letter in gridSpan.GetRow(row))
        {
            if (letter is '#')
            {
                result.Add(new(row, index));
            }

            index++;
        }

        return result;
    }

    public static int CountDots(this Span2D<char> gridSpan)
    {
        int dots = 0;
        for (int i = 0; i < gridSpan.Height; i++)
        {
            Span<char> row = gridSpan.GetRowSpan(i);
            dots += System.MemoryExtensions.Count(row, '.');
        }

        return dots;
    }
}
