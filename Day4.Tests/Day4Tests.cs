using AdventUtilities;

namespace Day4.Tests;

public class Day4Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day4/example1.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day4/input.txt");

    [Fact]
    public void WordFinder_FindTarget_TestData_Should_Return_18()
    {
        //Arrange
        string target = "XMAS";
        Span<string> input = File.ReadAllLines(testPath);
        WordFinder sut = new(target);

        //Act
        int result = sut.FindTarget(input);

        //Assert
        Assert.Equal(18, result);
    }

    [Fact]
    public void WordFinder_FindTarget_Puzzle_Should_Return_2358()
    {
        //Arrange
        string target = "XMAS";
        Span<string> input = File.ReadAllLines(puzzlePath);
        WordFinder sut = new(target);

        //Act
        int result = sut.FindTarget(input);

        //Assert
        Assert.Equal(2358, result);
    }

    [Fact]
    public void WordFinder_FindCrosses_TestData_Should_Return_9()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = WordFinder.FindCrosses(input);

        //Assert
        Assert.Equal(9, result);
    }

    [Fact]
    public void WordFinder_FindCrosses_Puzzle_Should_Return_1737()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = WordFinder.FindCrosses(input);

        //Assert
        Assert.Equal(1737, result);
    }
}
