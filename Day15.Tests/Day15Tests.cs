using AdventUtilities;
using CommunityToolkit.HighPerformance;

namespace Day15.Tests;

public class Day15Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day15/example1.txt");
    private readonly string testPath2 = Path.Combine(InputData.GetSolutionDirectory(), "Day15/example2.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day15/input.txt");


    [Fact]
    public void Solver_PartOne_TestPath_Should_Return_2028()
    {
        //Arrange

        //Act
        int result = Solver.PartOne(testPath);

        //Assert
        Assert.Equal(2028, result);
    }

    [Fact]
    public void Solver_PartOne_TestPath2_Should_Return_10092()
    {
        //Arrange

        //Act
        int result = Solver.PartOne(testPath2);

        //Assert
        Assert.Equal(10092, result);
    }

    [Fact]
    public void UpdateMap_MovingRight_ShouldUpdateRoSpanCorrectly()
    {
        //Arrange
        List<Box> boxes = [new(new(2, 4)), new(new(2, 5))];
        MapUpdater mapUpdater = new(boxes);
        char[,] map =
        {
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '@', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' }
        };

        //Act
        Span<char> row = map.AsSpan2D().GetRowSpan(2)[1..];
        Coord robot = new(2, 2);
        mapUpdater.UpdateMapRight(row, robot);

        //Assert
        Assert.Equal(['.', '.', '@', 'O', 'O', '.'], map.AsSpan2D().GetRowSpan(2));
    }

    [Fact]
    public void UpdateMap_MovingRight_PushingBoxes_ShouldUpdateRoSpanCorrectly()
    {
        //Arrange
        List<Box> boxes = [new(new(2, 3)), new(new(2, 4))];
        char[,] map =
        {
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '@', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' }
        };
        Robot robot = new(new(2, 2), map, [Direction.Right], boxes, []);

        //Act
        Coord move = Coord.Zero.Right;
        robot.Move(move, Direction.Right, boxes);

        //Assert
        Assert.Equal(['.', '.', '.', '@', 'O', 'O'], map.AsSpan2D().GetRowSpan(2));
    }

    [Fact]
    public void UpdateMapUp_MovingUp_ShouldUpdateColumnCorrectly()
    {
        //Arrange
        List<Box> boxes = [new(new(0, 2)), new(new(3, 2))];
        MapUpdater mapUpdater = new(boxes);
        char[,] map =
        {
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '@', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' }
        };

        //Act
        mapUpdater.UpdateMapUp(map.AsSpan2D(), new(2, 2));

        //Assert
        Assert.Equal('.', map[2, 2]);
        Assert.Equal('O', map[0, 2]);
        Assert.Equal('@', map[1, 2]);
    }

    [Fact]
    public void UpdateMapUp_MovingUpWhilePushing_ShouldUpdateColumnCorrectly()
    {
        //Arrange
        List<Box> boxes = [new(new(2, 1)), new(new(4, 1))];
        MapUpdater mapUpdater = new(boxes);
        char[,] map =
        {
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '@', '.', '.' },
            { '.', '.', '.', '.' }
        };
        Robot robot = new(new(3, 1), map, [Direction.Up], boxes, []);

        //Act
        Coord move = Coord.Zero.Up;
        robot.Move(move, Direction.Up, boxes);
        mapUpdater.UpdateMapUp(map.AsSpan2D(), robot.Position);

        //Assert
        Assert.Equal('.', map[0, 1]);
        Assert.Equal('O', map[1, 1]);
        Assert.Equal('@', map[2, 1]);
    }

    [Fact]
    public void UpdateMapDown_MovingDown_ShouldUpdateColumnCorrectly()
    {
        //Arrange
        List<Box> boxes = [new(new(1, 1)), new(new(4, 1))];
        MapUpdater mapUpdater = new(boxes);
        char[,] map =
        {
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '@', '.', '.' },
            { '.', '.', '.', '.' },
            { '.', '.', '.', '.' }
        };

        //Act
        mapUpdater.UpdateMapDown(map.AsSpan2D(), new(2, 1));

        //Assert
        Assert.Equal('.', map[2, 1]);
        Assert.Equal('@', map[3, 1]);
        Assert.Equal('O', map[4, 1]);
    }

    [Fact]
    public void UpdateMap_MovingLeftOnEmptyRow_ShouldOnlyUpdateRobotPosition()
    {
        //Arrange
        List<Box> boxes = [];
        MapUpdater mapUpdater = new(boxes);
        char[,] map =
        {
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '@', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' }
        };

        Span<char> rowSpan = map.AsSpan2D().GetRowSpan(2)[..5];
        Coord robotPosition = new(2, 4);

        mapUpdater.UpdateMapLeft(rowSpan, robotPosition);

        Assert.Equal(['.', '.', '.', '@', '.'], rowSpan);
    }

    [Fact]
    public void UpdateMap_MovingLeftAndPushingBoxes_ShouldOnlyUpdateRobotPosition()
    {
        List<Box> boxes = [new(new(2, 3)), new(new(2, 2))];
        MapUpdater mapUpdater = new(boxes);

        char[,] map =
        {
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '@', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' }
        };

        Robot robot = new(new(2, 4), map, [Direction.Left], boxes, []);
        Coord move = Coord.Zero.Left;
        robot.Move(move, Direction.Left, boxes);

        Span<char> rowSpan = map.AsSpan2D().GetRowSpan(2)[..5];
        Coord robotPosition = new(2, 4);

        mapUpdater.UpdateMapLeft(rowSpan, robotPosition);

        Assert.Equal(['.', 'O', 'O', '@', '.'], rowSpan);
    }

    [Fact]
    public void Solver_PartOne_Puzzle_Should_Return_1421727()
    {
        //Arrange


        //Act
        int result = Solver.PartOne(puzzlePath);

        //Assert
        Assert.Equal(1421727, result);
    }

}
