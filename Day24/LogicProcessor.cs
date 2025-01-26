namespace Day24;

using System;

public static class LogicProcessor
{
    public static long PartOne(string filePath)
    {
        Dictionary<string, Wire> wires = Parse(filePath, out List<Gate> gates);
        int zWires = gates.Count(gate => gate.OutputWire.StartsWith('z'));

        CircuitBoard cb = new(wires, gates, zWires);

        return cb.ZValue();
    }

    private static Dictionary<string, Wire> Parse(string filePath, out List<Gate> gates)
    {
        gates = [];
        Dictionary<string, Wire> wires = [];

        using StreamReader sr = new(filePath);
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                var parts = line.Split(": ");
                wires[parts[0]] = new(parts[0], (BitValue)(parts[1][0] - '0'));

                line = sr.ReadLine();
            }

            line = sr.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                var parts = line.Split();
                wires[parts[0]] = wires.TryGetValue(parts[0], out Wire storedWire1)
                                ? storedWire1
                                : new(parts[0]);
                wires[parts[2]] = wires.TryGetValue(parts[2], out Wire storedWire2)
                                ? storedWire2
                                : new(parts[2]);

                Logical log = parts[1] switch
                {
                    "AND" => Logical.And,
                    "OR" => Logical.Or,
                    "XOR" => Logical.Xor,
                    _ => throw new ArgumentException($"Parsing error: [{parts[1]}] is not a bitwise operation.")
                };

                gates.Add(new(log, wires[parts[0]], wires[parts[2]], parts[4]));

                line = sr.ReadLine();
            }
        }

        return wires;
    }
}
