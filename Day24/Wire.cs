namespace Day24;

public class Wire(string name, BitValue bitValue = BitValue.NotSet)
{
    public string Name { get; } = name;
    public BitValue BitValue { get; set; } = bitValue;
}

public enum BitValue
{
    Off,
    On,
    NotSet
}
