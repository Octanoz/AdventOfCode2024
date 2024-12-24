using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day14;

public static class Solver
{
    public static int PartOne(string filePath, int maxRow, int maxCol)
    {
        List<Robot> robotList = RobotData.ParseRobots(filePath);

        int[][] map = GridExtensions.JaggedIntArrayBlank(maxRow, maxCol);

        for (int i = 0; i < robotList.Count; i++)
        {
            Robot currentRobot = robotList[i];
            var (currentRow, currentCol) = currentRobot.Position;

            int rowSpeed = int.Abs(currentRobot.SpeedRow);
            int colSpeed = int.Abs(currentRobot.SpeedCol);

            int newRow = currentRobot.SpeedRow < 0
                        ? (currentRow + (maxRow - rowSpeed) * 100) % maxRow
                        : (currentRow + (rowSpeed * 100)) % maxRow;

            int newCol = currentRobot.SpeedCol < 0
                        ? (currentCol + (maxCol - colSpeed) * 100) % maxCol
                        : (currentCol + (colSpeed * 100)) % maxCol;

            robotList[i] = currentRobot with { Position = new(newRow, newCol) };
        }

        foreach (var robot in robotList)
        {
            var (row, col) = robot.Position;
            map[row][col]++;
        }

        map.DrawIntJaggedGrid();

        int rowSplit = maxRow / 2;
        int colSplit = maxCol / 2;

        var map2D = map.ConvertJaggedTo2D();

        Span2D<int> mapSpan = map2D;
        var quadrant1 = mapSpan[..rowSplit, ..colSplit];
        var quadrant2 = mapSpan[..rowSplit, (colSplit + 1)..];
        var quadrant3 = mapSpan[(rowSplit + 1).., ..colSplit];
        var quadrant4 = mapSpan[(rowSplit + 1).., (colSplit + 1)..];

        int[] robotsPerQuadrant = [0, 0, 0, 0];
        for (int i = 1; i <= 4; i++)
        {
            Console.WriteLine($"Quadrant {i}\n");

            var current = i switch
            {
                1 => quadrant1,
                2 => quadrant2,
                3 => quadrant3,
                4 => quadrant4
            };

            current.DrawInt2D();
            for (int row = 0; row < current.Height; row++)
            {
                foreach (var num in current.GetRow(row))
                {
                    robotsPerQuadrant[i - 1] += num;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Quadrant {i + 1}: {robotsPerQuadrant[i]}");
        }

        int result = robotsPerQuadrant.Aggregate(1, (acc, next) => acc * next);

        return result;
    }

    public static void PartTwo(string filePath, int maxRow, int maxCol)
    {
        List<Robot> robotList = RobotData.ParseRobots(filePath);

        char[][] map = GridExtensions.JaggedCharArrayBlank(maxRow, maxCol);
        for (int turns = 0; turns < 9000; turns++)
        {
            for (int i = 0; i < robotList.Count; i++)
            {
                Robot currentRobot = robotList[i];
                var (currentRow, currentCol) = currentRobot.Position;

                int rowSpeed = int.Abs(currentRobot.SpeedRow);
                int colSpeed = int.Abs(currentRobot.SpeedCol);

                int newRow = currentRobot.SpeedRow < 0
                            ? (currentRow + (maxRow - rowSpeed) * 1) % maxRow
                            : (currentRow + (rowSpeed * 1)) % maxRow;

                int newCol = currentRobot.SpeedCol < 0
                            ? (currentCol + (maxCol - colSpeed) * 1) % maxCol
                            : (currentCol + (colSpeed * 1)) % maxCol;

                robotList[i] = currentRobot with { Position = new(newRow, newCol) };
            }

            Array.ForEach(map, arr => Array.Fill(arr, '.'));

            foreach (var robot in robotList)
            {
                var (row, col) = robot.Position;
                map[row][col] = '#';
            }

            Span2D<char> mapSpan = map.ConvertJaggedTo2D();


            if (turns > 8000 && HasRobotChain(mapSpan))
            {
                mapSpan.Draw2DGridTightSlow();
                Console.WriteLine(turns);
            }
        }
    }

    public static bool HasRobotChain(Span2D<char> map)
    {
        int maxChain = 0;
        for (int i = 0; i < map.Height; i++)
        {
            int currentChain = 0;
            foreach (var letter in map.GetRow(i))
            {
                if (letter is '.')
                {
                    maxChain = int.Max(maxChain, currentChain);
                    currentChain = 0;
                    continue;
                }

                currentChain++;
            }
        }

        if (maxChain > 20)
        {
            return true;
        }

        return false;
    }

}
