namespace Day17;

public class ThreeBit(long[] registers, params (int, int)[] input)
{
    private readonly long[] registers = registers;
    private readonly (int, int)[] program = input;
    private List<long> output = [];
    private readonly long registerA = registers[0];
    public long index;

    public void SetRegister(long reg, long val) => registers[reg] = val;
    public long GetRegister(long reg) => registers[reg];
    public long GetA() => registers[0];
    public long GetB() => registers[1];
    public long GetC() => registers[2];
    public long SetA(long x) => registers[0] = x;
    public long SetB(long x) => registers[1] = x;
    public long SetC(long x) => registers[2] = x;

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

        return isPartTwo ? Print2() : Print();
    }

    public void CodeZero(int operand)
    {
        long numerator = GetA();
        long power = ComboValue(operand);
        long denominator = (long)Math.Pow(2, power);

        long division = numerator / denominator;

        long result = 0;
        checked
        {
            result = division > long.MaxValue ? long.MaxValue : (long)division;
        }

        SetA(result);
        IncrementPointer();
    }

    public void CodeOne(int operand) //Literal
    {
        long result = 0;
        checked
        {
            result = GetB() ^ operand;
        }

        SetB(result);
        IncrementPointer();
    }

    public void CodeTwo(int operand) //Combo
    {
        long combo = ComboValue(operand);
        long result = combo % 8;

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
        long result = 0;
        checked
        {
            result = GetB() ^ GetC();
        }

        SetB(result);
        IncrementPointer();
    }

    public void CodeFive(int operand) //Combo
    {
        long combo = ComboValue(operand);
        long result = combo % 8;

        output.Add(result);
        IncrementPointer();
    }

    public void CodeSix(int operand) //Combo
    {
        long numerator = GetA();
        long power = ComboValue(operand);
        long denominator = (long)Math.Pow(2, power);

        long division = numerator / denominator;

        long result = 0;
        checked
        {
            result = division > long.MaxValue ? long.MaxValue : (long)division;
        }

        SetB(result);
        IncrementPointer();
    }

    public void CodeSeven(int operand) //Combo
    {
        long numerator = GetA();
        long power = ComboValue(operand);
        long denominator = (long)Math.Pow(2, power);

        long division = numerator / denominator;

        long result = 0;
        checked
        {
            result = division > long.MaxValue ? long.MaxValue : (long)division;
        }

        SetC(result);
        IncrementPointer();
    }

    private long ComboValue(int operand) => operand switch
    {
        <= 3 => operand,
        4 => GetA(),
        5 => GetB(),
        6 => GetC(),
        _ => -1
    };
}
