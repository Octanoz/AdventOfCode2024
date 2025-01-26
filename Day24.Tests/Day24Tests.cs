namespace Day24.Tests;

using AdventUtilities;

public class Day24Tests
{
    private readonly string testPath1 = Path.Combine(InputData.GetSolutionDirectory(), "Day24/example1.txt");
    private readonly string testPath2 = Path.Combine(InputData.GetSolutionDirectory(), "Day24/example2.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day24/input.txt");

    [Fact]
    public void LogicProcessor_TestData1_PartOne_Should_Return_4()
    {
        //Arrange


        //Act
        long result = LogicProcessor.PartOne(testPath1);

        //Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void LogicProcessor_TestData2_PartOne_Should_Return_2024()
    {
        //Arrange


        //Act
        long result = LogicProcessor.PartOne(testPath2);

        //Assert
        Assert.Equal(2024, result);
    }

    [Fact]
    public void LogicProcessor_Puzzle_PartOne_Should_Return_65740327379952()
    {
        //Arrange


        //Act
        long result = LogicProcessor.PartOne(puzzlePath);

        //Assert
        Assert.Equal(65740327379952, result);
    }
}
