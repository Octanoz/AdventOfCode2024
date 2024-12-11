using AdventUtilities;

namespace Day06.Tests;

public class Day06Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day06/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day06/input.txt");

    [Fact]
    public void Tracker_PartOne_TestData_Should_Return_41()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);
        Tracker sut = new(input);

        //Act
        int result = sut.PartOne();

        //Assert
        Assert.Equal(41, result);
    }

    [Fact]
    public void Tracker_PartOne_Puzzle_Should_Return_4752()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);
        Tracker sut = new(input);

        //Act
        int result = sut.PartOne();

        //Assert
        Assert.Equal(4752, result);
    }

    [Fact]
    public void Tracker_PartTwo_TestData_Should_Return_6()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);
        Tracker sut = new(input);

        //Act
        int result = sut.PartTwo();

        //Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Tracker_PartTwo_Puzzle_Should_Return_1719()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);
        Tracker sut = new(input);

        //Act
        int result = sut.PartTwo();

        //Assert
        Assert.Equal(1719, result);
    }
}
