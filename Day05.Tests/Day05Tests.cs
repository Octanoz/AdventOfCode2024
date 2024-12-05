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
        OrderValidator sut = new();

        //Act
        int result = sut.PartOne(input);

        //Assert
        Assert.Equal(143, result);
    }
}
