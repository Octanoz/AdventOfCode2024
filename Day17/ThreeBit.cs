namespace Day17;

public class ThreeBit(int[] registers, params (int, int)[] input)
{
    private readonly int[] registers = registers;
    private readonly (int, int)[] program = input;
    private readonly List<int> output = [];
    private readonly int registerA = registers[0];
    public int index;

    public void SetRegister(int reg, int val) => registers[reg] = val;
    public int GetRegister(int reg) => registers[reg];
    public int GetA() => registers[0];
    public int GetB() => registers[1];
    public int GetC() => registers[2];
    public int SetA(int x) => registers[0] = x;
    public int SetB(int x) => registers[1] = x;
    public int SetC(int x) => registers[2] = x;

    public (int, int) NextBatch() => program[index];
    public void IncrementPointer() => index++;
    public void Jump(int x) => index = x;
    public string Print() => String.Join(",", output);
    public string Print2() => String.Join("", output);

    public string Run(bool isPartTwo = false)
    {
        while (index < program.Length)
        {
            var (code, operand) = NextBatch();

            switch (code)
            {
                case 0:
                    CodeZero(operand);
                    break;
                case 1:
                    CodeOne(operand);
                    break;
                case 2:
                    CodeTwo(operand);
                    break;
                case 3:
                    CodeThree(operand);
                    break;
                case 4:
                    CodeFour(operand);
                    break;
                case 5:
                    CodeFive(operand);
                    break;
                case 6:
                    CodeSix(operand);
                    break;
                case 7:
                    CodeSeven(operand);
                    break;
                default:
                    throw new ArgumentException($"Invalid number for the code received: {code}");
            }
        }

        return Print();
    }

    public void CodeZero(int operand)
    {
        int numerator = GetA();
        int power = ComboValue(operand);
        int denominator = (int)Math.Pow(2, power);

        int division = numerator / denominator;

        int result = 0;
        checked
        {
            result = division > int.MaxValue ? int.MaxValue : division;
        }

        SetA(result);
        IncrementPointer();
    }

    public void CodeOne(int operand) //Literal
    {
        int result = 0;
        checked
        {
            result = GetB() ^ operand;
        }

        SetB(result);
        IncrementPointer();
    }

    public void CodeTwo(int operand) //Combo
    {
        int combo = ComboValue(operand);
        int result = combo % 8;

        SetB(result);
        IncrementPointer();
    }

    public void CodeThree(int operand) //Literal
    {
        if (GetA() is 0)
        {
            IncrementPointer();
            return;
        }

        int result = operand / 2;
        Jump(result);
    }

    public void CodeFour(int operand) //Ignored
    {
        int result = 0;
        checked
        {
            result = GetB() ^ GetC();
        }

        SetB(result);
        IncrementPointer();
    }

    public void CodeFive(int operand) //Combo
    {
        int combo = ComboValue(operand);
        int result = combo % 8;

        output.Add(result);
        IncrementPointer();
    }

    public void CodeSix(int operand) //Combo
    {
        int numerator = GetA();
        int power = ComboValue(operand);
        int denominator = (int)Math.Pow(2, power);

        int division = numerator / denominator;

        int result = 0;
        checked
        {
            result = division > int.MaxValue ? int.MaxValue : (int)division;
        }

        SetB(result);
        IncrementPointer();
    }

    public void CodeSeven(int operand) //Combo
    {
        int numerator = GetA();
        int power = ComboValue(operand);
        int denominator = (int)Math.Pow(2, power);

        int division = numerator / denominator;

        int result = 0;
        checked
        {
            result = division > int.MaxValue ? int.MaxValue : (int)division;
        }

        SetC(result);
        IncrementPointer();
    }

    private int ComboValue(int operand) => operand switch
    {
        <= 3 => operand,
        4 => GetA(),
        5 => GetB(),
        6 => GetC(),
        _ => -1
    };

    private static int Pow2(long exponent) => 1 << (int)exponent; // Faster for powers of 2
}
