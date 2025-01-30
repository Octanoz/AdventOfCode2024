namespace Day24;

using System.Collections;

public class CircuitBoard(Dictionary<string, Wire> wires, List<Gate> gates, int maxZ)
{
    private readonly List<Gate> gates = gates;
    private readonly BitArray zWires = new(maxZ);

    public Dictionary<string, Wire> Wires { get; } = wires;

    private void ProcessGates()
    {
        PriorityQueue<Gate, int> gatesQueue = new();
        int priority;
        foreach (var gate in gates)
        {
            priority = gate.GetWireValues();
            gatesQueue.Enqueue(gate, priority);
        }

        while (gatesQueue.TryDequeue(out Gate? currentGate, out int currentPriority))
        {
            if (currentGate.ProcessOutput(out BitValue bv))
            {
                string wire = currentGate.OutputWire;
                if (wire.StartsWith('z'))
                {
                    int index = int.Parse(wire[1..]);
                    zWires[index] = (int)bv == 1;
                }
                else if (!Wires.TryGetValue(wire, out Wire? storedWire))
                {
                    Wires[wire] = new(wire, bv);
                }
                else
                    storedWire.BitValue = bv;

                continue;
            }

            priority = currentPriority + currentGate.GetWireValues();
            gatesQueue.Enqueue(currentGate, priority);
        }
    }

    private void ProcessGatesPart2()
    {
        PriorityQueue<Gate, int> gatesQueue = new();
        int priority;
        foreach (var gate in gates)
        {
            if (gate.OutputWire.StartsWith('z'))
            {
                int index = int.Parse(gate.OutputWire[1..]);
                priority = 200 + index;
            }
            else
                priority = gate.GetWireValues();

            gatesQueue.Enqueue(gate, priority);
        }

        while (gatesQueue.Count > maxZ && gatesQueue.TryDequeue(out Gate? currentGate, out int currentPriority))
        {
            string wire = currentGate.OutputWire;
            if (currentGate.ProcessOutput(out BitValue bv))
            {
                Wires[wire].BitValue = bv;
                continue;
            }

            priority = currentPriority + currentGate.GetWireValues();
            gatesQueue.Enqueue(currentGate, priority);
        }

        ProcessZWires(gatesQueue);
    }

    private void ProcessZWires(PriorityQueue<Gate, int> gatesQueue)
    {
        int carryBit = 0;
        while (gatesQueue.Count is not 0)
        {
            var currentGate = gatesQueue.Dequeue();
            int index = int.Parse(currentGate.OutputWire[1..]);

            if (currentGate.ProcessOutput(out BitValue bv))
            {
                zWires[index] = carryBit switch
                {
                    1 => bv is BitValue.Off,
                    0 => bv is BitValue.On,
                    _ => throw new ArgumentException($"The carry bit can only be 1 or 0, not {carryBit}")
                };

                carryBit = (currentGate.InWire1.BitValue, currentGate.InWire2.BitValue) switch
                {
                    (BitValue.On, BitValue.On) => 1,
                    _ => 0
                };
            }
            else
                throw new ArgumentException($"Gate for {currentGate.OutputWire} cannot be processed, which should not be possible at this stage of the processing.");
        }
    }

    public long ZValue()
    {
        ProcessGates();

        return ConvertZToLong(maxZ - 1);
    }

    public string SwapOutputs()
    {
        ProcessGatesPart2();

        int wiresToSwap = 8;
        List<string> incorrectWires = ValidateGateConnections();

        return incorrectWires.Count == wiresToSwap ? String.Join(',', incorrectWires)
                                                   : throw new InvalidOperationException($"Expected exactly {wiresToSwap} incorrect wires but found {incorrectWires.Count}.");
    }

    public List<string> ValidateGateConnections()
    {
        List<string> incorrectWires = [];
        foreach (var gate in gates)
        {
            string output = gate.OutputWire;
            switch (gate.Log)
            {
                case Logical.Xor when !HasXYZWire(gate): //XOR should involve at least one x, y or z wire
                case Logical.And when !IsAndGateValid(gate): //unless this gate is for the first wires, should feed into OR gate
                case Logical.Xor when FeedsIntoOrGate(gate): // XOR should NOT feed into OR gate
                //Gates with z wire output are always XOR except for the carry bit.
                case Logical _ when output.StartsWith('z') && gate.Log is not Logical.Xor && output != $"z{maxZ - 1}":
                    incorrectWires.Add(output);
                    break;
            }
        }

        incorrectWires.Sort();
        return incorrectWires;
    }

    private bool HasXYZWire(Gate gate) => gate.InWire1.Name[0] is 'x' or 'y'
                                       || gate.InWire2.Name[0] is 'x' or 'y'
                                       || gate.OutputWire.StartsWith('z');

    private bool IsAndGateValid(Gate gate) => gate.InWire1.Name is "x00"
                                           || gate.InWire2.Name is "x00"
                                           || gates.Any(g => (g.InWire1.Name == gate.OutputWire
                                                           || g.InWire2.Name == gate.OutputWire)
                                                           && g.Log is Logical.Or);

    private bool FeedsIntoOrGate(Gate gate) => gates.Any(g => (g.InWire1.Name == gate.OutputWire
                                                            || g.InWire2.Name == gate.OutputWire)
                                                            && g.Log is Logical.Or);

    private long ConvertZToLong(int index)
    {
        long sum = 0;
        for (int i = 0; i <= index; i++)
        {
            if (zWires[i])
            {
                sum |= (1L << i);
            }
        }

        return sum;
    }
}
