using CommunityToolkit.HighPerformance;

namespace AdventUtilities;

public static class Span2DExtensions
{
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

    public static void DrawGridSideways(this Span2D<char> grid)
    {
        for (int col = 0; col < grid.Width; col++) // Iterate over columns
        {
            foreach (var item in grid.GetColumn(col))
            {
                Console.Write(item);
            }
            Console.WriteLine(); // New line after each column (now row in sideways view)
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
}
