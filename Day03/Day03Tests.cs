namespace Day3;

using Xunit;
using AdventUtilities;


public class Day03Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day3/example1.txt");
    private static readonly string testPath2 = Path.Combine(InputData.GetSolutionDirectory(), "Day3/example2.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day3/input.txt");

    [Fact]
    public void Scanner_PartOne_TestData_Should_Return_161()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = Scanner.PartOne(input);

        //Assert
        Assert.Equal(161, result);
    }

    [Fact]
    public void Scanner_PartOne_Puzzle_Should_Return_182780583()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = Scanner.PartOne(input);

        //Assert
        Assert.Equal(182780583, result);
    }

    [Fact]
    public void Scanner_PartTwo_TestData_Should_Return_48()
    {
        //Arrange
        string input = File.ReadAllText(testPath2);

        //Act
        int result = Scanner.PartTwo(input);

        //Assert
        Assert.Equal(48, result);
    }

    [Fact]
    public void Scanner_PartTwo_Puzzle_Should_Return_90772405()
    {
        //Arrange
        string input = File.ReadAllText(puzzlePath);

        //Act
        int result = Scanner.PartTwo(input);

        //Assert
        Assert.Equal(90772405, result);
    }
}
