using AdventUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Day07.Tests;

public class Day07Tests
{
    private static readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day07/example1.txt");
    private static readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day07/input.txt");

    [Fact]
    public void OperatorStack_PartOne_TestData_Should_Return_3749()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        long result = OperatorStack.ValidateEquations(input);

        //Assert
        Assert.Equal(3749, result);
    }

    [Fact]
    public void OperatorStack_PartOne_Puzzle_Should_Return_2664460013123()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        long result = OperatorStack.ValidateEquations(input);

        //Assert
        Assert.Equal(2664460013123, result);
    }


    [Fact]
    public void OperatorStack_PartTwo_TestData_Should_Return_11387()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        long result = OperatorStack.ValidateEquations(input, partTwo: true);

        //Assert
        Assert.Equal(11387, result);
    }

    [Fact]
    public void OperatorStack_PartTwo_Puzzle_Should_Return_426214131924213()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        long result = OperatorStack.ValidateEquations(input, partTwo: true);

        //Assert
        Assert.Equal(426214131924213, result);
    }
}
