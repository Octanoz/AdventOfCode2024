using System.Text;
using CommunityToolkit.HighPerformance;

namespace AdventUtilities;

public static class GridExtensions
{
    public static T[,] New2DGrid<T>(string[] input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        T[,] grid = new T[rows, cols];

        Func<char, T> convertFunc = GetCharConvertFunc<T>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = convertFunc(input[i][j]);
            }
        }

        return grid;
    }

    public static T[,] New2DGrid<T>(Span<string> input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        T[,] grid = new T[rows, cols];

        Func<char, T> convertFunc = GetCharConvertFunc<T>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = convertFunc(input[i][j]);
            }
        }

        return grid;
    }

    public static T[,] New2DGrid<T>(string[] input, int elementLength)
    {
        string[][] chunks = input.Select(s => s
                                        .Select(c => c)
                                        .Chunk(elementLength)
                                        .Select(cArray => new string(cArray))
                                        .ToArray())
                                    .ToArray();

        int rows = chunks.Length;
        int cols = chunks[0].Length;
        T[,] grid = new T[rows, cols];

        Func<string, T> convertFunc = GetStringConvertFunc<T>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = convertFunc(chunks[i][j]);
            }
        }

        return grid;
    }

    public static T[,] New2DGridWithDimensions<T>(Span<string> input, out int rows, out int cols)
    {
        rows = input.Length;
        cols = input[0].Length;
        T[,] grid = new T[rows, cols];

        Func<char, T> convertFunc = GetCharConvertFunc<T>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = convertFunc(input[i][j]);
            }
        }

        return grid;
    }

    public static T[,] New2DGridWithDimensions<T>(string[] input, out int rows, out int cols)
    {
        rows = input.Length;
        cols = input[0].Length;
        T[,] grid = new T[rows, cols];

        Func<char, T> convertFunc = GetCharConvertFunc<T>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = convertFunc(input[i][j]);
            }
        }

        return grid;
    }

    public static char[,] New2DGridBlank(int rows, int cols)
    {
        char[,] grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = '.';
            }
        }

        return grid;
    }

    private static Func<char, T> GetCharConvertFunc<T>()
    {
        return Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Char => (c) => (T)(object)c,
            TypeCode.Int32 => (c) => (T)(object)(c - '0'),
            TypeCode.Int64 => (c) => (T)(object)(long)(c - '0'),
            _ => throw new NotSupportedException($"Type {typeof(T)} is not supported.")
        };
    }

    private static Func<string, T> GetStringConvertFunc<T>()
    {
        return Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Int32 => (s) => (T)(object)int.Parse(s),
            TypeCode.Int64 => (s) => (T)(object)long.Parse(s),
            _ => throw new NotSupportedException($"Type {typeof(T)} is not supported.")
        };
    }

    public static char[][] JaggedCharArray(string[] input) => input.Select(s => s.ToCharArray()).ToArray();

    public static int[][] JaggedIntArray(string[] input, char divider) =>
        input.Select(s => s.Split(divider))
             .Select(sArray => sArray.Select(int.Parse).ToArray())
             .ToArray();

    public static char[][] JaggedCharArrayBlank(int maxRow, int maxCol) =>
        Enumerable.Range(0, maxRow)
                  .Select(_ => Enumerable.Repeat('.', maxCol).ToArray())
                  .ToArray();

    public static int[][] JaggedIntArrayBlank(int maxRow, int maxCol) =>
        Enumerable.Range(0, maxRow)
                  .Select(_ => Enumerable.Repeat(0, maxCol).ToArray())
                  .ToArray();
    public static int[][] JaggedIntArrayDivider(string[] input, char divider) =>
        input.Select(s => s.Split(divider))
             .Select(sArray => sArray.Select(int.Parse).ToArray())
             .ToArray();

    public static void Draw2DGrid<T>(this T[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{grid[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    public static void Draw2DGridTight<T>(this T[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{grid[i, j]}");
            }
            Console.WriteLine();
        }
    }

    public static string Write2DGridTight<T>(this T[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        StringBuilder sb = new();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append($"{grid[i, j]}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static void CompareToOther<T>(this T[,] first, T[,] second)
    {
        int rows = first.GetLength(0);
        int cols = first.GetLength(1);

        if (second.GetLength(0) != rows || second.GetLength(1) != cols)
        {
            Console.WriteLine("Grids are not of equal size, cannot print side-by-side");
            return;
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{first[i, j]} ");
            }

            Console.Write("\t\t");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{second[i, j]} ");
            }

            Console.WriteLine();
        }
    }

    public static void DrawJaggedGrid<T>(this T[][] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{grid[i][j]} ");
            }
            Console.WriteLine();
        }
    }

    public static void DrawJaggedGridTight<T>(this T[][] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{grid[i][j]}");
            }
            Console.WriteLine();
        }
    }

    public static void DrawIntJaggedGrid(this int[][] grid)
    {
        int maxRow = grid.Length;
        int maxCol = grid[0].Length;

        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                int current = grid[row][col];

                if (current is 0)
                {
                    Console.Write(' ');
                }
                else Console.Write(current);
            }

            Console.WriteLine();
        }
    }

    public static T[,] ConvertJaggedTo2D<T>(this T[][] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        T[,] result = new T[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = grid[i][j];
            }
        }

        return result;
    }

    public static T[][] Convert2DToJagged<T>(this T[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        T[][] result = new T[rows][];
        for (int i = 0; i < rows; i++)
        {
            result[i] = new T[cols];
            for (int j = 0; j < cols; j++)
            {
                result[i][j] = grid[i, j];
            }
        }

        return result;
    }

    public static Span2D<T> ConvertToSpan2D<T>(this T[][] grid) => grid.ConvertJaggedTo2D().AsSpan2D();
}