using AdventUtilities;

namespace Day13.Tests;

public class Day13Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day13/example1.txt");
    private readonly string testPath2 = Path.Combine(InputData.GetSolutionDirectory(), "Day13/example2.txt");
    private readonly string testPath3 = Path.Combine(InputData.GetSolutionDirectory(), "Day13/example3.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day13/input.txt");

    [Fact]
    public void Machine1_PartOne_Should_Return_280()
    {
        //Arrange


        //Act
        int result = Solver.PartOne(testPath2);

        //Assert
        Assert.Equal(280, result);
    }

    [Fact]
    public void Machine4_PartOne_Should_Return_200()
    {
        //Arrange


        //Act
        int result = Solver.PartOne(testPath3);

        //Assert
        Assert.Equal(200, result);

    }

    [Fact]
    public void PartOne_TestData_Should_Return_480()
    {
        //Arrange


        //Act
        int result = Solver.PartOne(testPath);

        //Assert
        Assert.Equal(480, result);
    }

    [Fact]
    public void PartOne_Puzzle_Should_Return_29201()
    {
        //Arrange


        //Act
        int result = Solver.PartOne(puzzlePath);

        //Assert
        Assert.Equal(29201, result);
    }

    [Fact]
    public void PartTwo_Puzzle_Should_Return_104140871044942()
    {
        //Arrange


        //Act
        long result = Solver.PartTwo(puzzlePath);

        //Assert
        Assert.Equal(104140871044942, result);
    }
}
