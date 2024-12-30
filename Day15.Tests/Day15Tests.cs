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
    public void UpdateMap_PartOne_MovingRight_ShouldUpdateRoSpanCorrectly()
    {
        //Arrange
        List<Box> boxes = [new Box(new(3, 3)), new Box(new(3, 4))];
        MapUpdater mapUpdater = new(boxes);
        bool boxesMoved = true;
        char[,] map =
        {
            { '#', '#', '#', '#', '#', '#' },
            { '#', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '#' },
            { '#', '@', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '#' },
            { '#', '#', '#', '#', '#', '#' }
        };

        //Act
        Coord robot = new(3, 1);
        Coord target = new(3, 2);
        boxes.ForEach(box => box.Moved = true);
        mapUpdater.UpdateBoxesMap(map.AsSpan2D(), robot, target, Direction.Right, boxesMoved);

        //Assert
        Assert.Equal(['#', '.', '@', 'O', 'O', '#'], map.AsSpan2D().GetRowSpan(3));
    }

    [Fact]
    public void UpdateMap_PartOne_MovingRight_PushingBoxes_ShouldUpdateRoSpanCorrectly()
    {
        //Arrange
        List<Box> boxes = [new Box(new(2, 3)), new Box(new(2, 4))];
        MapUpdater mapUpdater = new(boxes);
        char[,] map =
        {
            { '#', '#', '#', '#', '#', '#', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '@', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '#', '#', '#', '#', '#', '#' }
        };
        Robot robot = new(new(2, 2), map, [Direction.Right], boxes);

        //Act
        Coord move = Coord.Zero.Right;
        robot.Move(move, Direction.Right);

        //Assert
        Assert.Equal(['#', '.', '.', '@', 'O', 'O', '#'], map.AsSpan2D().GetRowSpan(2));
    }

    [Fact]
    public void UpdateMap_PartOne_MovingUp_ShouldUpdateColumnCorrectly()
    {
        //Arrange
        List<Box> boxes = [new Box(new(1, 2)), new Box(new(2, 2))];
        MapUpdater mapUpdater = new(boxes);
        bool boxesMoved = true;
        char[,] map =
        {
            { '#', '#', '#', '#', '#', '#', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '#', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '#', '#', '#', '#', '#', '#' }
        };

        //Act
        Coord robot = new(4, 2);
        Coord target = new(3, 2);
        boxes.ForEach(box => box.Moved = true);
        mapUpdater.UpdateBoxesMap(map.AsSpan2D(), robot, target, Direction.Up, boxesMoved);

        //Assert
        Assert.Equal('O', map[1, 2]);
        Assert.Equal('O', map[2, 2]);
        Assert.Equal('@', map[3, 2]);
    }

    [Fact]
    public void UpdateMap_PartOne_MovingUpWhilePushing_ShouldUpdateColumnCorrectly()
    {
        //Arrange
        List<Box> boxes = [new Box(new(2, 1)), new Box(new(4, 1))];
        MapUpdater mapUpdater = new(boxes);
        char[,] map =
        {
            { '#', '#', '#', '#', '#', '#', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '@', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '#', '#', '#', '#', '#', '#' }
        };
        Coord previous = new(3, 1);
        Robot robot = new(previous, map, [Direction.Up], boxes);

        //Act
        Coord move = Coord.Zero.Up;
        Coord current = previous + move;
        robot.Move(move, Direction.Up);

        //Assert
        Assert.Equal('#', map[0, 1]);
        Assert.Equal('O', map[1, 1]);
        Assert.Equal('@', map[2, 1]);
    }

    [Fact]
    public void UpdateMap_PartOne_MovingDown_ShouldUpdateColumnCorrectly()
    {
        //Arrange
        List<Box> boxes = [new Box(new(1, 1)), new Box(new(4, 1))];
        MapUpdater mapUpdater = new(boxes);
        bool boxesMoved = true;
        char[,] map =
        {
            { '#', '#', '#', '#', '#', '#', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '@', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '#', '#', '#', '#', '#', '#' }
        };
        Coord previous = new(2, 1);

        //Act
        Coord current = new(3, 1);
        boxes.ForEach(box => box.Moved = true);
        mapUpdater.UpdateBoxesMap(map.AsSpan2D(), previous, current, Direction.Down, boxesMoved);

        //Assert
        Assert.Equal('.', map[2, 1]);
        Assert.Equal('@', map[3, 1]);
        Assert.Equal('O', map[4, 1]);
    }

    [Fact]
    public void UpdateMap_PartOne_MovingLeftOnEmptyRow_ShouldOnlyUpdateRobotPosition()
    {
        //Arrange
        List<Box> boxes = [];
        MapUpdater mapUpdater = new(boxes);
        bool boxesMoved = false;
        char[,] map =
        {
            { '#', '#', '#', '#', '#', '#', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '@', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '.', '.', '.', '.', '.', '#' },
            { '#', '#', '#', '#', '#', '#', '#' }
        };
        Coord previous = new(2, 4);

        //Act
        Coord current = new(2, 3);
        mapUpdater.UpdateBoxesMap(map.AsSpan2D(), previous, current, Direction.Left, boxesMoved);
        var rowSpan = map.AsSpan2D().GetRowSpan(2);

        Assert.Equal(['#', '.', '.', '@', '.', '.', '#'], rowSpan);
    }

    [Fact]
    public void UpdateMap_PartOne_MovingLeftAndPushingBoxes_ShouldOnlyUpdateRobotPosition()
    {
        //Arrange
        List<Box> boxes = [new Box(new(2, 3)), new Box(new(2, 2))];
        MapUpdater mapUpdater = new(boxes);

        char[,] map =
        {
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '@', '.' },
            { '.', '.', '.', '.', '.', '.' },
            { '.', '.', '.', '.', '.', '.' }
        };

        Robot robot = new(new(2, 4), map, [Direction.Left], boxes);
        Coord move = Coord.Zero.Left;
        robot.Move(move, Direction.Left);

        Span<char> rowSpan = map.AsSpan2D().GetRowSpan(2)[..5];

        // mapUpdater.UpdateBoxesMap(rowSpan, robotPosition);

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

    [Fact]
    public void Solver_PartTwo_Puzzle_Should_Return_1463160()
    {
        //Arrange


        //Act
        int result = Solver.PartTwo(puzzlePath);

        //Assert
        Assert.Equal(1463160, result);
    }
}
