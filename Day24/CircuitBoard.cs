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
                if (!Wires.TryGetValue(wire, out Wire? storedWire))
                {
                    Wires[wire] = new(wire, bv);
                }
                else
                    storedWire.BitValue = bv;

                if (wire.StartsWith('z'))
                {
                    int index = int.Parse(wire[1..]);
                    zWires[index] = (int)bv == 1;
                }

                continue;
            }

            priority = currentPriority + currentGate.GetWireValues();
            gatesQueue.Enqueue(currentGate, priority);
        }
    }

    public long ZValue()
    {
        ProcessGates();

        return zWires.Cast<bool>()
                     .Select((bit, i) => bit ? 1L << i : 0)
                     .Sum();
    }
}
