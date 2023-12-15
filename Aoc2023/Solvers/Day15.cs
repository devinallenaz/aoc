using Aoc2023.Models;
using AocHelpers;
using AocHelpers.Solvers;

public class Day15 : Solver
{
    public override int Day => 15;

    private int Hash(string input)
    {
        return input.Aggregate(0, (s, c) => ((s + c) * 17) % 256);
    }

    //Problem 1
    public override object ExpectedOutput1 => 1320;

    public override object Solve1(string input)
    {
        return input.SplitCommas().Sum(s => Hash(s));
    }

    //Problem 2
    public override object ExpectedOutput2 => 145;

    public override object Solve2(string input)
    {
        var instructions = input.SplitCommas().Select(s => new LensInstruction(s));
        var boxes = new List<Lens>[256];
        for (var i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new List<Lens>();
        }

        foreach (var instruction in instructions)
        {
            instruction.PerformOperation(boxes);
        }

        return boxes.WithIndex().Sum((b) => b.item.WithIndex().Sum((l) => (b.index + 1) * (l.index + 1) * l.item.FocalLength));
    }
}