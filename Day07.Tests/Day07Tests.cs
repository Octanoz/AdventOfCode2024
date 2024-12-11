using AdventUtilities;

namespace Day07.Tests;

public class Day07Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day07/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day07/challenge.txt");

    [Fact]
    public void OperatorStack_PartOne_Should_Return_3749()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);
        OperatorStack sut = new();

        //Act
        long result = sut.PartOne(input);

        //Assert
        Assert.Equal(3749, result);
    }
}
