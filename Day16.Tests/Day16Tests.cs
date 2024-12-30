namespace Day16.Tests;

using AdventUtilities;

public class Day16Tests
{
    string testPath1 = Path.Combine(InputData.GetSolutionDirectory(), "Day16/example1.txt");
    string testPath2 = Path.Combine(InputData.GetSolutionDirectory(), "Day16/example2.txt");
    string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day16/input.txt");


    [Fact]
    public void MazeRunner_PartOne_TestData1_Should_Return_7036()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath1);

        //Act
        int result = MazeRunner.PartOne(input);

        //Assert
        Assert.Equal(7036, result);
    }

    [Fact]
    public void MazeRunner_PartOne_TestData2_Should_Return_11048()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath2);

        //Act
        int result = MazeRunner.PartOne(input);

        //Assert
        Assert.Equal(11048, result);
    }


    [Fact]
    public void MazeRunner_PartOne_Puzzle_Should_Return_133584()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = MazeRunner.PartOne(input);

        //Assert
        Assert.Equal(133584, result);
    }

    [Fact]
    public void MazeRunner_PartTwo_TestData1_Should_Return_45()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath1);

        //Act
        int result = MazeRunner.PartOne(input);

        //Assert
        Assert.Equal(7036, result);
    }

    [Fact]
    public void MazeRunner_PartTwo_TestData2_Should_Return_64()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath2);

        //Act
        int result = MazeRunner.PartOne(input);

        //Assert
        Assert.Equal(11048, result);
    }

    [Fact]
    public void MazeRunner_PartTwo_Puzzle_Should_Return_622()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = MazeRunner.PartTwo(input);

        //Assert
        Assert.Equal(622, result);
    }




}
