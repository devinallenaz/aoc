using Aoc2022.Models;
using AocHelpers;
using AocHelpers.Solvers;

namespace Aoc2022.Solvers;

public class Day21 : Solver
{
    public override int Day => 21;
    public override object ExpectedOutput1 => 152L;

    public override object Solve1(string input)
    {
        var monkeys = YellingMonkeysFromData(input);
        return monkeys["root"].GetValue(monkeys);
    }

    public override object ExpectedOutput2 => 301L;

    public override object Solve2(string input)
    {
        var monkeys = YellingMonkeysFromData(input);
        return monkeys["root"].Solve(monkeys);
    }


    private static Dictionary<string, YellingMonkey> YellingMonkeysFromData(string input)
    {
        var lines = input.SplitLines();
        var monkeys = new Dictionary<string, YellingMonkey>();
        foreach (var line in lines)
        {
            var parts = line.Split(": ");
            var name = parts[0];
            if (long.TryParse(parts[1], out long value))
            {
                monkeys.Add(name, new YellingMonkey(value));
            }
            else
            {
                var parts2 = parts[1].Split();
                monkeys.Add(name, new YellingMonkey(parts2[0], parts2[1], parts2[2]));
            }
        }

        return monkeys;
    }
}