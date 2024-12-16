using AdventUtilities;

namespace Day09.Tests;

public class Day09Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day09/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day09/input.txt");

    [Fact]
    public void Defragmenter_PartOne_TestData_Should_Return_1928()
    {
        //Arrange
        string input = File.ReadAllText(testPath);
        Defragmenter sut = new(input);

        //Act
        long result = sut.PartOne();

        //Assert
        Assert.Equal(1928, result);
    }

    [Fact]
    public void Defragmenter_PartOne_Puzzle_Should_Return_6258319840548()
    {
        //Arrange
        string input = File.ReadAllText(puzzlePath);
        Defragmenter sut = new(input);

        //Act
        long result = sut.PartOne();

        //Assert
        Assert.Equal(6258319840548, result);
    }

    [Fact]
    public void Defragmenter_PartTwo_TestData_Should_Return_2858()
    {
        //Arrange
        string input = File.ReadAllText(testPath);
        SpanDefragmentation sut = new(input);

        //Act
        long result = sut.PartTwo();

        //Assert
        Assert.Equal(2858, result);
    }
}
