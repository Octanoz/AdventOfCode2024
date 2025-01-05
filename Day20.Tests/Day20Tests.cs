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
}
