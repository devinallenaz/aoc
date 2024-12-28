using AocHelpers;
using AocHelpers.Solvers;
using Wire = (string operation, string[] operands);

public class Day24 : Solver
{
    public override int Day => 24;

    private static Dictionary<string, bool> Values { get; } = new();
    private static List<(string name, int value)> XNames { get; } = new();
    private static List<(string name, int value)> YNames { get; } = new();
    private static List<(string name, int value)> ZNames { get; } = new();

    private static void LoadValues(long x, long y)
    {
        foreach (var xName in XNames)
        {
            Values[xName.name] = (x & (1l << xName.value)) == 1l << xName.value;
        }

        foreach (var yName in YNames)
        {
            Values[yName.name] = (y & (1l << yName.value)) == 1l << yName.value;
        }
    }

    private static Dictionary<string, Func<bool, bool, bool>> Operations = new()
    {
        { "AND", (a, b) => a && b },
        { "OR", (a, b) => a || b },
        { "XOR", (a, b) => a ^ b },
    };

    private static bool CalculateWire(Wire wire, Dictionary<string, Wire> wires)
    {
        return Operations[wire.operation](GetOperand(wire.operands[0], wires), GetOperand(wire.operands[1], wires));
    }

    private static bool GetOperand(string name, Dictionary<string, Wire> wires)
    {
        bool value;

        if (name.StartsWith('x') || name.StartsWith('y'))
        {
            value = Values[name];
        }
        else
        {
            value = CalculateWire(wires[name], wires);
        }

        return value;
    }

    private Dictionary<string, Wire> Setup(string input)
    {
        Values.Clear();
        XNames.Clear();
        YNames.Clear();
        ZNames.Clear();
        var wires = new Dictionary<string, Wire>();
        var sections = input.SplitSections();
        foreach (var line in sections[0].SplitLines())
        {
            var name = line[..3];
            Values.Add(name, line[5] == '1');
            if (name.StartsWith('x'))
            {
                XNames.Add((name, int.Parse(name[1..])));
            }

            if (name.StartsWith('y'))
            {
                YNames.Add((name, int.Parse(name[1..])));
            }
        }

        foreach (var line in sections[1].SplitLines())
        {
            var parts = line.SplitAndTrim();
            var operand1 = parts[0];
            var operation = parts[1];
            var operand2 = parts[2];
            var name = parts[4];
            wires[name] = (operation, [operand1, operand2]);
            if (name.StartsWith('z'))
            {
                ZNames.Add((name, int.Parse(name[1..])));
            }
        }

        return wires;
    }


    //Problem 1
    public override object ExpectedOutput1 => 2024l;

    public override object Solve1(string input)
    {
        var wires = Setup(input);

        return PerformCalculation(wires);
    }

    private static long PerformCalculation(Dictionary<string, Wire> wires)
    {
        var output = 0l;
        foreach (var z in ZNames)
        {
            if (CalculateWire(wires[z.name], wires))
            {
                output += 1l << z.value;
            }
        }

        return output;
    }

    //Problem 2
    public override object ExpectedOutput2 => base.ExpectedOutput2;
    private static char[] InputOutputCharacters { get; } = ['x', 'y', 'z'];

    private static bool IsInputOrOutput(string wireName)
    {
        return InputOutputCharacters.Contains(wireName[0]);
    }

    public override object Solve2(string input)
    {
        var wires = Setup(input);
        var wrongWires = new HashSet<string>();
        var hightestZ = ZNames.MaxBy(z => z.value).name;
        foreach (var kvp in wires)
        {
            var result = kvp.Key;
            var operand1 = kvp.Value.operands[0];
            var operand2 = kvp.Value.operands[1];
            var operation = kvp.Value.operation;
            if (result.StartsWith("z") && operation != "XOR" && result != hightestZ)
            {
                wrongWires.Add(result);
            }

            if (operation == "XOR"
                && !IsInputOrOutput(result)
                && !IsInputOrOutput(operand1)
                && !IsInputOrOutput(operand2)
               )
            {
                wrongWires.Add(result);
            }

            if (operation == "AND" && !kvp.Value.operands.Contains("x00"))
            {
                foreach (var kvp2 in wires)
                {
                    if (kvp2.Value.operands.Contains(result) && kvp2.Value.operation != "OR")
                    {
                        wrongWires.Add(result);
                    }
                }
            }

            if (operation == "XOR")
            {
                foreach (var kvp2 in wires)
                {
                    if (kvp2.Value.operands.Contains(result) && kvp2.Value.operation == "OR")
                    {
                        wrongWires.Add(result);
                    }
                }
            }
        }

        return string.Join(",", wrongWires.Order());
    }
}