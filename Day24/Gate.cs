namespace Day24;

public class Gate(Logical log, Wire inWire1, Wire inWire2, string outputWire)
{
    public Logical Log { get; } = log;
    public Wire InWire1 { get; private set; } = inWire1;
    public Wire InWire2 { get; private set; } = inWire2;
    public string OutputWire { get; } = outputWire;

    private int ProcessOutputValue(int wire1, int wire2) => Log switch
    {
        Logical.And => (wire1 & wire2),
        Logical.Or => (wire1 | wire2),
        Logical.Xor => (wire1 ^ wire2),
        _ => throw new InvalidDataException($"Logical enum has an invalid value: {Log}.")
    };

    public void SetWire1(BitValue bv) => InWire1.BitValue = bv;
    public void SetWire2(BitValue bv) => InWire2.BitValue = bv;

    public void SetWires(BitValue bv1, BitValue bv2)
    {
        SetWire1(bv1);
        SetWire2(bv2);
    }

    public int GetWireValues() => (int)InWire1.BitValue + (int)InWire2.BitValue;

    public bool ProcessOutput(out BitValue bv)
    {
        if (InWire1.BitValue is BitValue.NotSet || InWire2.BitValue is BitValue.NotSet)
        {
            bv = BitValue.NotSet;
            return false;
        }

        bv = (BitValue)ProcessOutputValue((int)InWire1.BitValue, (int)InWire2.BitValue);
        return true;
    }
}

public enum Logical
{
    And,
    Or,
    Xor
}
