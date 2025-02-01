namespace Day18.Tests;

using AdventUtilities;

public class Day18Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day18/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day18/input.txt");

    [Fact]
    public void Plotter_PartOne_TestData_Should_Return_22()
    {
        //Arrange


        //Act
        int result = Plotter.PartOne(testPath, isTest: true);

        //Assert
        Assert.Equal(22, result);
    }

    [Fact]
    public void Plotter_PartOne_Puzzle_Should_Return_364()
    {
        //Arrange


        //Act
        int result = Plotter.PartOne(puzzlePath);

        //Assert
        Assert.Equal(364, result);
    }
}
