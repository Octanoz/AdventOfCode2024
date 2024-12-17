using AdventUtilities;

namespace Day11.Tests;

public class Day11Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day11/example1.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day11/input.txt");

    [Theory]
    [InlineData(6, 22)]
    [InlineData(25, 55312)]
    public void Blink_PartOne_TestData_Should_Return_Expected(int blinkCount, int expected)
    {
        //Arrange
        string input = File.ReadAllText(testPath);
        Blink sut = new(input);

        //Act
        int result = sut.PartOne(blinkCount);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Blink_PartOne_Puzzle_Should_Return_186424()
    {
        //Arrange
        string input = File.ReadAllText(puzzlePath);
        Blink sut = new(input);

        //Act
        int result = sut.PartOne(25);

        //Assert
        Assert.Equal(186424, result);
    }
}
