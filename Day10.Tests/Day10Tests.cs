using AdventUtilities;

namespace Day10.Tests;

public class Day10Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day10/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day10/input.txt");

    [Fact]
    public void PeakFinder_PartOne_Should_Return_35()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = PeakFinder.PartOne(input);

        //Assert
        Assert.Equal(35, result);
    }
}
