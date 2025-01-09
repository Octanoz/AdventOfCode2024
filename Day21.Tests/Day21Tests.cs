namespace Day21.Tests;

using AdventUtilities;

using Day21;

public class Day21Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day21/example1.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day21/input.txt");

    [Fact]
    public void KeypadManager_TestData_Should_Return_126384()
    {
        //Arrange
        SequenceBuilder seqBuilder = new();
        seqBuilder.CacheAllShortestSequences();
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = KeypadManager.PartOne(input);

        //Assert
        Assert.Equal(126384, result);
    }

    [Fact]
    public void KeypadManager_Puzzle_Should_Return_182844()
    {
        //Arrange
        SequenceBuilder seqBuilder = new();
        seqBuilder.CacheAllShortestSequences();
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = KeypadManager.PartOne(input);

        //Assert
        Assert.Equal(182844, result);
    }

    [Fact]
    public void KeypadManager_Puzzle_Should_Return_226179529377982()
    {
        //Arrange
        SequenceBuilder seqBuilder = new();
        seqBuilder.CacheAllShortestSequences();
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        ulong result = KeypadManager.PartTwo(input);

        //Assert
        Assert.Equal(226179529377982UL, result);
    }
}
