using AdventUtilities;

namespace Day12.Tests;

public class Day12Tests
{
    private readonly string testPath1 = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example1.txt");
    private readonly string testPath2 = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example2.txt");
    private readonly string testPath3 = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example3.txt");
    private readonly string testPath4 = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example4.txt");
    private readonly string testPath5 = Path.Combine(InputData.GetSolutionDirectory(), "Day12/example5.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day12/input.txt");

    [Fact]
    public void FencePricing_PartOne_TestData1_Should_Return_140()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath1);


        //Act
        int result = FencePricing.PartOne(input);

        //Assert
        Assert.Equal(140, result);
    }

    [Fact]
    public void FencePricing_PartOne_TestData2_Should_Return_772()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath2);


        //Act
        int result = FencePricing.PartOne(input);

        //Assert
        Assert.Equal(772, result);
    }

    [Fact]
    public void FencePricing_PartOne_TestData3_Should_Return_1930()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath3);


        //Act
        int result = FencePricing.PartOne(input);

        //Assert
        Assert.Equal(1930, result);
    }

    [Fact]
    public void FencePricing_PartOne_Puzzle_Should_Return_1415378()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = FencePricing.PartOne(input);

        //Assert
        Assert.Equal(1415378, result);
    }

    [Fact]
    public void FencePricing_PartTwo_TestData1_Should_Return_80()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath1);


        //Act
        int result = FencePricing.PartTwo(input);

        //Assert
        Assert.Equal(80, result);
    }

    [Fact]
    public void FencePricing_PartTwo_TestData2_Should_Return_436()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath2);


        //Act
        int result = FencePricing.PartTwo(input);

        //Assert
        Assert.Equal(436, result);
    }

    [Fact]
    public void FencePricing_PartTwo_TestData3_Should_Return_1206()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath3);


        //Act
        int result = FencePricing.PartTwo(input);

        //Assert
        Assert.Equal(1206, result);
    }

    [Fact]
    public void FencePricing_PartTwo_TestData4_Should_Return_236()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath3);


        //Act
        int result = FencePricing.PartTwo(input);

        //Assert
        Assert.Equal(236, result);
    }

    [Fact]
    public void FencePricing_PartTwo_TestData4_Should_Return_368()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath3);


        //Act
        int result = FencePricing.PartTwo(input);

        //Assert
        Assert.Equal(368, result);
    }
}
