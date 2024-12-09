using AdventUtilities;

namespace Day05.Tests;

public class Day05Tests
{
    private static string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day05/example1.txt");
    private static string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day05/input.txt");

    [Fact]
    public void OrderValidator_PartOne_TestData_Should_Return_143()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);
        OrderValidator sut = new(input);

        //Act
        int result = sut.Process();

        //Assert
        Assert.Equal(143, result);
    }

    [Fact]
    public void OrderValidator_PartOne_Puzzle_Should_Return_4790()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);
        OrderValidator sut = new(input);

        //Act
        int result = sut.Process();

        //Assert
        Assert.Equal(4790, result);
    }

    [Fact]
    public void OrderValidator_PartTwo_TestData_Should_Return_123()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);
        OrderValidator sut = new(input);

        //Act
        int result = sut.Process(true);

        //Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void OrderValidator_PartTwo_Puzzle_Should_Return_6319()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);
        OrderValidator sut = new(input);

        //Act
        int result = sut.Process(true);

        //Assert
        Assert.Equal(6319, result);
    }
}
