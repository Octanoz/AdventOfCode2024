using AdventUtilities;

namespace Day08.Tests;

public class Day08Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), @"Day08\example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), @"Day08\input.txt");

    [Fact]
    public void PartOne_TestData_Should_Return_14()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);
        PropagationMarker sut = new(input);

        //Act
        int result = sut.PartOne();

        //Assert
        Assert.Equal(14, result);
    }

    [Fact]
    public void PartOne_Puzzle_Should_Return_398()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);
        PropagationMarker sut = new(input);

        //Act
        int result = sut.PartOne();

        //Assert
        Assert.Equal(398, result);
    }

    [Fact]
    public void PartOne_TestData_Should_Return_34()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);
        PropagationMarker sut = new(input);

        //Act
        int result = sut.PartTwo();

        //Assert
        Assert.Equal(34, result);
    }

    [Fact]
    public void PartOne_Puzzle_Should_Return_1333()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);
        PropagationMarker sut = new(input);

        //Act
        int result = sut.PartTwo();

        //Assert
        Assert.Equal(1333, result);
    }
}
