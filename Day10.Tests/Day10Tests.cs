using AdventUtilities;

namespace Day10.Tests;

public class Day10Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day10/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day10/input.txt");

    [Fact]
    public void PeakFinder_PartOne_TestData_Should_Return_36()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = PeakFinder.PartOne(input);

        //Assert
        Assert.Equal(36, result);
    }

    [Fact]
    public void PeakFinder_PartOne_Puzzle_Should_Return_430()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = PeakFinder.PartOne(input);

        //Assert
        Assert.Equal(430, result);
    }

    [Fact]
    public void PeakFinder_PartTwo_TestData_Should_Return_81()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = PeakFinder.PartTwo(input);

        //Assert
        Assert.Equal(81, result);
    }
}
