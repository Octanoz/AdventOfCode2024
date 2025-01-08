using Day21;

public class Day21Tests
{
    [Fact]
    public void NumpadOperator_029A_Should_Return_Possible_Solution()
    {
        //Arrange
        NumpadOperator numOp = new(['0', '2', '9', 'A']);

        //Act
        string result = numOp.EnterCode();

        //Assert
        Assert.Contains(result, (HashSet<string>)["<A^A>^^AvvvA", "<A^A^>^AvvvA", "<A^A^^>AvvvA"]);
    }

    [Fact]
    public void Keypad_2_029A_Should_Return_Possible_Solution()
    {
        //Arrange
        NumpadOperator numOp = new(['0', '2', '9', 'A']);
        string numpadDirections = numOp.EnterCode();
        KeypadOperator keyOp = new(numpadDirections);

        //Act
        string result = keyOp.EnterCode();

        //Assert
        Assert.Contains(result, (HashSet<string>)["v<<A>>^A<A>AvA<^AA>Av<AAA>^A", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A", "v<<A>^>A<A>AvA^<AA>Av<AAA^>A"]);
    }

    [Fact]
    public void Human_980A_Should_Return_Possible_Solution()
    {
        //Arrange
        NumpadOperator numOp = new(['9', '8', '0', 'A']);
        string numpadDirections = numOp.EnterCode();
        KeypadOperator keyOp = new(numpadDirections);
        string keyInputs = keyOp.EnterCode();
        KeypadOperator human = new(keyInputs);

        //Act
        string result = human.EnterCode();

        //Assert
        Assert.Contains(result, (HashSet<string>)["<v<A>>^AAAvA^A<vA<AA>>^AvAA<^A>A<v<A>A>^AAAvA<^A>A<vA>^A<A>A"]);
    }

    [Fact]
    public void Keypad_2_179A_Should_Return_Possible_Solution()
    {
        //Arrange
        NumpadOperator numOp = new(['1', '7', '9', 'A']);
        string numpadDirections = numOp.EnterCode();
        KeypadOperator keyOp = new(numpadDirections);
        string keyInputs = keyOp.EnterCode();
        KeypadOperator human = new(keyInputs);

        //Act
        string result = human.EnterCode();

        //Assert
        Assert.Contains(result, (HashSet<string>)["<v<A>>^A<vA<A>>^AAvAA<^A>A<v<A>>^AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A"]);
    }

    [Fact]
    public void Keypad_2_456A_Should_Return_Possible_Solution()
    {
        //Arrange
        NumpadOperator numOp = new(['4', '5', '6', 'A']);
        string numpadDirections = numOp.EnterCode();
        KeypadOperator keyOp = new(numpadDirections);
        string keyInputs = keyOp.EnterCode();
        KeypadOperator human = new(keyInputs);

        //Act
        string result = human.EnterCode();

        //Assert
        Assert.Contains(result, (HashSet<string>)["<v<A>>^AA<vA<A>>^AAvAA<^A>A<vA>^A<A>A<vA>^A<A>A<v<A>A>^AAvA<^A>A"]);
    }

    [Fact]
    public void Keypad_2_379A_Should_Return_Possible_Solution()
    {
        //Arrange
        NumpadOperator numOp = new(['3', '7', '9', 'A']);
        string numpadDirections = numOp.EnterCode();
        KeypadOperator keyOp = new(numpadDirections);
        string keyInputs = keyOp.EnterCode();
        KeypadOperator human = new(keyInputs);

        //Act
        string result = human.EnterCode();

        //Assert
        Assert.Contains(result, (HashSet<string>)["<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A"]);
    }
}
