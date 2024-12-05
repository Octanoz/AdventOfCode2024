namespace Day1;

using AdventUtilities;
using Xunit;


public class Day01Tests
{
    [Theory]
    [InlineData(new[] { "3   4", "4   3", "2   5", "1   3", "3   9", "3   3" }, 11)]
    public void PartOne_Should_Return_Expected(string[] input, int expected)
    {
        //Arrange


        //Act
        int result = Solver.PartOne(input);

        //Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new[] { "3   4", "4   3", "2   5", "1   3", "3   9", "3   3" }, 31)]
    public void PartTwo_Should_Return_Expected(string[] input, int expected)
    {
        //Arrange


        //Act
        int result = Solver.PartTwo(input);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AlternativeTwo_Should_Return_31()
    {
        //Arrange
        string filePath = Path.Combine(InputData.GetSolutionDirectory(), "Day1/Example1.txt");

        //Act
        int result = Solver.Alternative(filePath);


        //Assert
        Assert.Equal(31, result);
    }
}
