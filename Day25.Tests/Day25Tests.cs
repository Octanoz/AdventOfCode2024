namespace Day25.Tests;

using AdventUtilities;

public class Day25Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day25/example1.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day25/input.txt");

    [Fact]
    public void Locksmith_TestData_Should_Return_3()
    {
        //Arrange


        //Act
        int result = Locksmith.PartOne(testPath);

        //Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void Locksmith_TestData_Should_Return_3327()
    {
        //Arrange


        //Act
        int result = Locksmith.PartOne(puzzlePath);

        //Assert
        Assert.Equal(3327, result);
    }
}
