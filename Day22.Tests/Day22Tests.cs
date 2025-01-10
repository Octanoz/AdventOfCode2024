namespace Day22.Tests;

using AdventUtilities;

public class Day22Tests
{
    private string testPath = Path.Combine(InputData.GetSolutionDirectory(), "Day22/example1.txt");
    private string puzzlePath = Path.Combine(InputData.GetSolutionDirectory(), "Day22/input.txt");

    [Fact]
    public void NumberCooking_ProcessSecretNumber_123_Should_Return_15887950()
    {
        //Arrange
        int start = 123;

        //Act
        long result = NumberCooking.ProcessSecretNumber(start);

        //Assert
        Assert.Equal(15887950, result);
    }

    [Fact]
    public void NumberCooking_ProcessSecretNumber_123_Twice_Should_Return_16495136()
    {
        //Arrange
        int start = 123;

        //Act
        long firstResult = NumberCooking.ProcessSecretNumber(start);
        long result = NumberCooking.ProcessSecretNumber(firstResult);

        //Assert
        Assert.Equal(16495136, result);
    }

    [Theory]
    [InlineData([123, 2, 16495136])]
    [InlineData([123, 3, 527345L])]
    [InlineData([123, 4, 704524])]
    [InlineData([123, 5, 1553684])]
    [InlineData([123, 6, 12683156])]
    [InlineData([123, 7, 11100544])]
    [InlineData([123, 8, 12249484])]
    [InlineData([123, 9, 7753432])]
    [InlineData([123, 10, 5908254])]
    public void NumberCooking_ProcessSecretTimes_123_Times_Should_Return_Expected(int secret, int times, long expected)
    {
        //Arrange

        //Act
        long result = NumberCooking.ProcessSecretTimes(secret, times);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NumberCooking_ProcessSecretTimes_1_2000_Should_Return_8685429()
    {
        //Arrange
        int secret = 1;
        int times = 2000;

        //Act
        long result = NumberCooking.ProcessSecretTimes(secret, times);

        //Assert
        Assert.Equal(8685429, result);
    }

    [Fact]
    public void NumberCooking_ProcessSecretTimes_10_2000_Should_Return_4700978()
    {
        //Arrange
        int secret = 10;
        int times = 2000;

        //Act
        long result = NumberCooking.ProcessSecretTimes(secret, times);

        //Assert
        Assert.Equal(4700978, result);
    }

    [Fact]
    public void NumberCooking_ProcessSecretTimes_100_2000_Should_Return_15273692()
    {
        //Arrange
        int secret = 100;
        int times = 2000;

        //Act
        long result = NumberCooking.ProcessSecretTimes(secret, times);

        //Assert
        Assert.Equal(15273692, result);
    }

    [Fact]
    public void NumberCooking_ProcessSecretTimes_2024_2000_Should_Return_8667524()
    {
        //Arrange
        int secret = 2024;
        int times = 2000;

        //Act
        long result = NumberCooking.ProcessSecretTimes(secret, times);

        //Assert
        Assert.Equal(8667524, result);
    }

    [Fact]
    public void Negotiate_TestData_PartOne_Should_Return_37327623()
    {
        //Arrange
        string[] input = File.ReadAllLines(testPath);

        //Act
        long result = Negotiate.PartOne(input);

        //Assert
        Assert.Equal(37327623, result);
    }

    [Fact]
    public void Negotiate_Puzzle_PartOne_Should_Return_19150344884()
    {
        //Arrange
        string[] input = File.ReadAllLines(puzzlePath);

        //Act
        long result = Negotiate.PartOne(input);

        //Assert
        Assert.Equal(19150344884, result);
    }
}
