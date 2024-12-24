using AdventUtilities;
using Day14;

namespace Day14.Tests;

public class Day14Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day14/example1.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day14/input.txt");

    [Fact]
    public void Solver_PartOne_TestData_Should_Return_12()
    {
        //Arrange


        //Act
        long result = Solver.PartOne(testPath, 7, 11);

        //Assert
        Assert.Equal(12, result);
    }
}
