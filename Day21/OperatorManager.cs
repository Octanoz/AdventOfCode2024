namespace Day21;

public static class OperatorManager
{
    public static int PartOne(string[] input)
    {
        int result = 0;
        foreach (var line in input)
        {
            NumpadOperator numOp = new(line.ToCharArray());
            string numInputs = numOp.EnterCode();

            KeypadOperator keyOp = new(numInputs);
            string keyInputs = keyOp.EnterCode();

            KeypadOperator hooman = new(keyInputs);
            string firstInput = hooman.EnterCode();
            int length = firstInput.Length;


            int numericPart = int.Parse(new string(line.Where(Char.IsDigit).ToArray()));
            result += numericPart * firstInput.Length;

            Console.WriteLine($"{line}:\nNumpad: {numInputs}\nKeypad2: {keyInputs}\nHooman: {firstInput}");
            Console.WriteLine($"Numeric part times input steps: {numericPart} x {length} = {numericPart * length}");
        }

        return result;
    }
}
