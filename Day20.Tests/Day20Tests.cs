namespace Day20.Tests;

using AdventUtilities;

using TableContext = (char[,] Table, int MaxRow, int MaxCol);

public class Day20Tests
{
    string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day20/example1.txt");
    string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day20/input.txt");

    [Fact]
    public void Mapper_FindMarkers_TestData_Should_Return_Correct_Coordinates_For_Start_And_Exit()
    {
        //Arrange
        Coord correctStart = new(3, 1);
        Coord correctExit = new(7, 5);

        string[] input = File.ReadAllLines(testPath);
        char[,] charGrid = GridExtensions.New2DGridWithDimensions<char>(input, out int maxRow, out int maxCol);
        TableContext tc = new(charGrid, maxRow, maxCol);

        //Act
        var (start, exit) = Mapper.FindMarkers(charGrid);

        //Assert
        Assert.Equal(correctStart, start);
        Assert.Equal(correctExit, exit);
    }

    [Fact]
    public void Mapper_PartOne_TestData_Should_Return_Correct_Number_Of_Cheats_Per_Seconds_Improved()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        Dictionary<int, int> cheatsCount = Mapper.PartOne(input, true);

        //Assert
        Assert.Equal(14, cheatsCount[2]);
        Assert.Equal(14, cheatsCount[4]);
        Assert.Equal(2, cheatsCount[6]);
        Assert.Equal(4, cheatsCount[8]);
        Assert.Equal(2, cheatsCount[10]);
        Assert.Equal(3, cheatsCount[12]);
        Assert.Equal(1, cheatsCount[20]);
        Assert.Equal(1, cheatsCount[36]);
        Assert.Equal(1, cheatsCount[38]);
        Assert.Equal(1, cheatsCount[40]);
        Assert.Equal(1, cheatsCount[64]);
    }

    [Fact]
    public void Mapper_PartOne_Puzzle_Should_Return_Correct_Number_Of_Cheats_Per_Second()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        Dictionary<int, int> cheatsCount = Mapper.PartOne(input);

        //Assert
        Assert.Equal(1311, cheatsCount.Values.Sum());
    }

    [Fact]
    public void Mapper_PartTwo_TestData_Should_Return_Correct_Number_Of_Cheats_Per_Seconds_Improved()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        Dictionary<int, int> cheatsCount = Mapper.PartTwo(input, true);

        //Assert
        Assert.Equal(32, cheatsCount[50]);
        Assert.Equal(31, cheatsCount[52]);
        Assert.Equal(29, cheatsCount[54]);
        Assert.Equal(39, cheatsCount[56]);
        Assert.Equal(25, cheatsCount[58]);
        Assert.Equal(23, cheatsCount[60]);
        Assert.Equal(20, cheatsCount[62]);
        Assert.Equal(19, cheatsCount[64]);
        Assert.Equal(12, cheatsCount[66]);
        Assert.Equal(14, cheatsCount[68]);
        Assert.Equal(12, cheatsCount[70]);
        Assert.Equal(22, cheatsCount[72]);
        Assert.Equal(4, cheatsCount[74]);
        Assert.Equal(3, cheatsCount[76]);
    }

    [Fact]
    public void Mapper_PartTwo_Puzzle_Should_Return_Correct_Number_Of_Cheats_Per_Second()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        Dictionary<int, int> cheatsCount = Mapper.PartTwo(input);

        //Assert
        Assert.Equal(961364, cheatsCount.Values.Sum());
    }


}
