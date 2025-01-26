namespace Day23.Tests;

using AdventUtilities;

public class Day23Tests
{
    private readonly string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day23/example1.txt");
    private readonly string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day23/input.txt");

    [Fact]
    public void EasterBunnyAdmin_TestData_Should_Return_7()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        int result = EasterBunnyAdmin.PartOne(input);

        //Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void EasterBunnyAdmin_Puzzle_Should_Return_1344()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        int result = EasterBunnyAdmin.PartOne(input);

        //Assert
        Assert.Equal(1344, result);
    }

    [Fact]
    public void EasterBunnyAdmin_TestData_Should_Return_co_de_ka_ta()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        string result = EasterBunnyAdmin.PartTwo(input);

        //Assert
        Assert.Equal("co,de,ka,ta", result);
    }

    [Fact]
    public void EasterBunnyAdmin_Puzzle_Should_Return_ab_al_cq_cr_da_db_dr_fw_ly_mn_od_py_uh()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        string result = EasterBunnyAdmin.PartTwo(input);

        //Assert
        Assert.Equal("ab,al,cq,cr,da,db,dr,fw,ly,mn,od,py,uh", result);
    }
}
