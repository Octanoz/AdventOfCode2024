namespace Day2;

using Xunit;
using AdventUtilities;

public class Day2Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day2/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day2/input.txt");

    [Fact]
    public void ReportReader_PartOne_TestData_Should_Return_2()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = ReportReader.PartOne(input);

        //Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void ReportReader_PartOne_Puzzle_Should_Return_287()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = ReportReader.PartOne(input);

        //Assert
        Assert.Equal(287, result);
    }

    [Fact]
    public void ReportReader_PartTwo_TestData_Should_Return_4()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = ReportReader.PartTwo(input);

        //Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void ReportReader_PartTwo_Puzzle_Should_Return_354()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = ReportReader.PartTwo(input);

        //Assert
        Assert.Equal(354, result);
    }
}
