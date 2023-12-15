using System.Text.RegularExpressions;

namespace Aoc2023.Models;

public class LensInstruction
{
    private static int Hash(string input)
    {
        return input.Aggregate(0, (s, c) => ((s + c) * 17) % 256);
    }

    private static Regex matcher = new Regex(@"(\w+)([=-])(\d)?");
    private int TargetBox { get; }
    private char Operation { get; }
    private Lens Lens { get; }

    public LensInstruction(string init)
    {
        var groups = matcher.Match(init).Groups;
        var label = groups[1].Value;
        this.TargetBox = Hash(label);
        this.Operation = groups[2].Value.Single();
        var focalLength = this.Operation == '=' ? int.Parse(groups[3].Value) : 0;
        this.Lens = new Lens(label, focalLength);
    }

    public void PerformOperation(List<Lens>[] boxes)
    {
        var box = boxes[this.TargetBox];
        if (this.Operation == '-')
        {
            box.RemoveAll(l => l.Label == this.Lens.Label);
        }
        else
        {
            var existingLens = box.FirstOrDefault(l => l.Label == this.Lens.Label);
            if (existingLens != null)
            {
                var index =box.IndexOf(existingLens);
                box.Remove(existingLens);
                box.Insert(index, this.Lens);
            }
            else
            {
                box.Add(this.Lens);
            }
        }
        
    }
}

public record Lens(string Label, int FocalLength)
{
}